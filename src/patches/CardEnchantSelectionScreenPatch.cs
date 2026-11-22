using System;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MoreUpgradedCardPreviews.Core;
using MoreUpgradedCardPreviews.Settings.Configs;
using MoreUpgradedCardPreviews.UI;
using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace MoreUpgradedCardPreviews.Patches;

public sealed class CardEnchantSelectionScreenPatch : UpgradePreviewPatch<NDeckEnchantSelectScreen>, IModPatches
{
    private CardEnchantSelectionScreenPatch(NDeckEnchantSelectScreen screen) : base(screen)
    {
    }

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                $"{ModInfo.Id}.card_enchant_selection_screen",
                typeof(NDeckEnchantSelectScreen),
                nameof(NDeckEnchantSelectScreen._Ready),
                typeof(CardEnchantSelectionScreenPatch),
                true,
                "Adds the upgrade preview toggle to the deck enchant selection screen."
            )
        );
    }

    public static void Postfix(NDeckEnchantSelectScreen __instance)
    {
        new CardEnchantSelectionScreenPatch(__instance).Attach();
    }

    protected override bool IsEnabled()
    {
        return CardEnchantSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardEnchantSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(handler);
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardEnchantSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(handler);
    }

    protected override void ApplyPreview(bool showingUpgrades)
    {
        var grid = Screen.GetNodeOrNull<NCardGrid>("%CardGrid");
        if (grid == null) return;

        grid.IsShowingUpgrades = showingUpgrades;
    }

    protected override NPeekButton? GetPeekButton()
    {
        return Screen.GetNodeOrNull<NPeekButton>("%PeekButton");
    }

    protected override void OnToggleCreated(UpgradePreviewToggle toggle)
    {
        var singlePreview = Screen.GetNodeOrNull<Control>("%EnchantSinglePreviewContainer");
        var multiPreview = Screen.GetNodeOrNull<Control>("%EnchantMultiPreviewContainer");
        if (singlePreview == null || multiPreview == null) return;

        var toggleContainer = toggle.Container;
        if (!GodotObject.IsInstanceValid(toggleContainer)) return;

        var previewIndex = Math.Min(singlePreview.GetIndex(), multiPreview.GetIndex());
        Screen.MoveChild(toggleContainer, previewIndex);
    }
}