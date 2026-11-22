using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MoreUpgradedCardPreviews.Core;
using MoreUpgradedCardPreviews.Settings.Configs;
using MoreUpgradedCardPreviews.UI;
using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace MoreUpgradedCardPreviews.Patches;

public sealed class CardBundleSelectionScreenPatch : UpgradePreviewPatch<NChooseABundleSelectionScreen>
{
    private const string ToggleNodePath = "UpgradePreviewToggleContainer";
    private const string PreviewContainerPath = "%BundlePreviewContainer";
    private const string PreviewCardsPath = "%Cards";

    private static readonly ConditionalWeakTable<NChooseABundleSelectionScreen, CardBundleSelectionScreenPatch> Active =
        new();

    private readonly Dictionary<NCard, (CardModel Original, CardModel Upgraded)> _previewCards = new();

    private CardBundleSelectionScreenPatch(NChooseABundleSelectionScreen screen) : base(screen)
    {
    }

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(new ModPatchInfo(
            $"{ModInfo.Id}.bundle_selection_ready",
            typeof(NChooseABundleSelectionScreen),
            nameof(NChooseABundleSelectionScreen._Ready),
            typeof(CardBundleSelectionScreenPatch),
            false,
            "Creates the upgrade preview toggle for card bundles."));

        patcher.RegisterPatch(new ModPatchInfo(
            $"{ModInfo.Id}.bundle_selection_open",
            typeof(NChooseABundleSelectionScreen),
            "OnBundleClicked",
            typeof(BundleOpenedHook),
            false,
            "Updates the cards after opening a bundle."));

        patcher.RegisterPatch(new ModPatchInfo(
            $"{ModInfo.Id}.bundle_selection_cancel",
            typeof(NChooseABundleSelectionScreen),
            "CancelSelection",
            typeof(BundleCancelledHook),
            false,
            "Restores original card models before returning to bundles."));

        patcher.RegisterPatch(new ModPatchInfo(
            $"{ModInfo.Id}.bundle_selection_confirm",
            typeof(NChooseABundleSelectionScreen),
            "ConfirmSelection",
            typeof(BundleConfirmedHook),
            false,
            "Restores original card models before confirming a bundle."));

        patcher.RegisterPatch(new ModPatchInfo(
            $"{ModInfo.Id}.bundle_selection_shown",
            typeof(NChooseABundleSelectionScreen),
            nameof(NChooseABundleSelectionScreen.AfterOverlayShown),
            typeof(OverlayShownHook),
            false,
            "Synchronizes toggle visibility after returning from an overlay."));
    }

    public static void Postfix(NChooseABundleSelectionScreen __instance)
    {
        try
        {
            if (Active.TryGetValue(__instance, out _)) return;

            var patch = new CardBundleSelectionScreenPatch(__instance);
            Active.Add(__instance, patch);

            __instance.TreeExiting += patch.OnScreenExiting;
            patch.Attach();
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to attach bundle upgrade toggle: {ex}");
        }
    }

    protected override bool IsEnabled()
    {
        return CardBundleSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardBundleSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(handler);
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardBundleSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(handler);
    }

    protected override NPeekButton? GetPeekButton()
    {
        return Screen.GetNodeOrNull<NPeekButton>("%PeekButton");
    }

    protected override void OnToggleCreated(UpgradePreviewToggle toggle)
    {
        toggle.Container.Visible = IsBundlePreviewVisible();
    }

    private bool IsBundlePreviewVisible()
    {
        return Screen.GetNodeOrNull<Control>(PreviewContainerPath)?.Visible == true;
    }

    private void UpdateToggleVisibility()
    {
        var container = Screen.GetNodeOrNull<Control>(ToggleNodePath);
        if (container != null) container.Visible = IsBundlePreviewVisible();
    }

    private void OnBundleOpened()
    {
        UpdateToggleVisibility();
        if (IsEnabled()) ApplyPreview(UpgradePreviewManager.Instance.IsShowingUpgrades);
    }

    protected override void ApplyPreview(bool showingUpgrades)
    {
        if (!showingUpgrades)
        {
            RestoreOriginalCards();
            return;
        }

        if (!IsBundlePreviewVisible()) return;

        var cardContainer = Screen.GetNodeOrNull<Control>(PreviewCardsPath);
        if (cardContainer == null) return;

        foreach (var child in cardContainer.GetChildren())
        {
            if (child is not NPreviewCardHolder holder || !GodotObject.IsInstanceValid(holder)) continue;

            var card = holder.CardNode;
            if (card == null || !GodotObject.IsInstanceValid(card)) continue;
            if (!holder.IsAncestorOf(card)) continue;

            if (!_previewCards.TryGetValue(card, out var models))
            {
                var original = card.Model;
                if (original == null || !original.IsUpgradable) continue;

                try
                {
                    var upgraded = (CardModel)original.MutableClone();
                    upgraded.UpgradeInternal();

                    models = (original, upgraded);
                    _previewCards.Add(card, models);
                }
                catch (Exception ex)
                {
                    Main.Logger.Error($"Could not build bundle upgrade preview for {original.Id}: {ex}");
                    continue;
                }
            }

            try
            {
                card.Model = models.Upgraded;
                card.ShowUpgradePreview();
            }
            catch (Exception ex)
            {
                card.Model = models.Original;
                _previewCards.Remove(card);
                Main.Logger.Error($"Could not display bundle upgrade preview: {ex}");
            }
        }
    }

    private void RestoreOriginalCards()
    {
        foreach (var (card, models) in _previewCards)
        {
            if (!GodotObject.IsInstanceValid(card)) continue;

            try
            {
                if (!ReferenceEquals(card.Model, models.Upgraded)) continue;

                card.Model = models.Original;
                card.UpdateVisuals(card.DisplayingPile, CardPreviewMode.Normal);
            }
            catch (Exception ex)
            {
                Main.Logger.Error($"Could not restore bundle card preview: {ex}");
            }
        }

        _previewCards.Clear();
    }

    private void BeforeLeavingPreview()
    {
        RestoreOriginalCards();
        var toggle = Screen.GetNodeOrNull<Control>(ToggleNodePath);
        if (toggle != null) toggle.Visible = false;
    }

    private void OnScreenExiting()
    {
        RestoreOriginalCards();
        Active.Remove(Screen);
        Screen.TreeExiting -= OnScreenExiting;
    }

    private static bool TryGet(NChooseABundleSelectionScreen screen, out CardBundleSelectionScreenPatch patch)
    {
        return Active.TryGetValue(screen, out patch!);
    }

    private static class BundleOpenedHook
    {
        public static void Postfix(NChooseABundleSelectionScreen __instance)
        {
            if (TryGet(__instance, out var patch)) patch.OnBundleOpened();
        }
    }

    private static class BundleCancelledHook
    {
        public static void Prefix(NChooseABundleSelectionScreen __instance)
        {
            if (TryGet(__instance, out var patch)) patch.BeforeLeavingPreview();
        }
    }

    private static class BundleConfirmedHook
    {
        public static void Prefix(NChooseABundleSelectionScreen __instance)
        {
            if (TryGet(__instance, out var patch)) patch.BeforeLeavingPreview();
        }
    }

    private static class OverlayShownHook
    {
        public static void Postfix(NChooseABundleSelectionScreen __instance)
        {
            if (TryGet(__instance, out var patch)) patch.UpdateToggleVisibility();
        }
    }
}