using System;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MoreUpgradedRewardsPreviews.Settings.Configs;
using STS2RitsuLib.Patching.Models;
using STS2RitsuLib.Patching.Core;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardUpgradeSelectionScreenPatch
{
    private readonly NDeckUpgradeSelectScreen _screen;

    private NTickbox? _vanillaToggle;
    private Node? _vanillaToggleParent;
    private int _vanillaToggleIndex = -1;

    private Action<bool>? _settingChangedHandler;
    private Action? _treeExitingHandler;

    private CardUpgradeSelectionScreenPatch(NDeckUpgradeSelectScreen screen) {_screen = screen;}

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "deck_upgrade_selection_screen_upgrade_preview",
                targetType: typeof(NDeckUpgradeSelectScreen),
                methodName: nameof(NDeckUpgradeSelectScreen._Ready),
                patchType: typeof(CardUpgradeSelectionScreenPatch),
                isCritical: true,
                description: "Controls the vanilla upgrade preview toggle on the card upgrade screen."
            )
        );
    }

    public static void Postfix(NDeckUpgradeSelectScreen __instance)
    {
        new CardUpgradeSelectionScreenPatch(__instance).Attach();
    }

    private void Attach()
    {
        _vanillaToggle = _screen.GetNodeOrNull<NTickbox>("%Upgrades");
        if (_vanillaToggle == null) return;

        var parent = _vanillaToggle.GetParent();
        if (parent == null) return;

        _vanillaToggleParent = parent;
        _vanillaToggleIndex = _vanillaToggle.GetIndex();

        _settingChangedHandler = OnSettingChanged;
        _treeExitingHandler = Cleanup;

        CardUpgradeSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(_settingChangedHandler);

        _screen.TreeExiting += _treeExitingHandler;

        ApplySetting(CardUpgradeSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled());
    }

    private void OnSettingChanged(bool enabled)
    {
        if (!GodotObject.IsInstanceValid(_screen)) return;

        ApplySetting(enabled);
    }

    private void ApplySetting(bool enabled)
    {
        if (enabled)
        {
            RestoreVanillaToggle();
            return;
        }

        RemoveVanillaToggle();
    }

    private void RemoveVanillaToggle()
    {
        if (_vanillaToggle == null || _vanillaToggleParent == null) return;

        SetPreview(false);

        if (_vanillaToggle.GetParent() == _vanillaToggleParent) _vanillaToggleParent.RemoveChild(_vanillaToggle);
    }

    private void RestoreVanillaToggle()
    {
        if (_vanillaToggle == null || _vanillaToggleParent == null) return;
        if (_vanillaToggle.GetParent() != null) return;

        _vanillaToggle.IsTicked = false;
        _vanillaToggleParent.AddChild(_vanillaToggle);
        _vanillaToggleParent.MoveChild(_vanillaToggle, _vanillaToggleIndex);
    }

    private void SetPreview(bool showingUpgrades)
    {
        var grid = _screen.GetNodeOrNull<NCardGrid>("%CardGrid");
        if (grid == null) return;

        grid.IsShowingUpgrades = showingUpgrades;
    }

    private void Cleanup()
    {
        if (_settingChangedHandler != null)
        {
            CardUpgradeSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(_settingChangedHandler);
            _settingChangedHandler = null;
        }

        _treeExitingHandler = null;
    }
}