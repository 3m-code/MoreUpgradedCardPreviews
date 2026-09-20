using System;

using Godot;

using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MoreUpgradedRewardsPreviews.Core;

public abstract class VanillaUpgradePreviewPatch<TScreen> where TScreen : Control
{
    private readonly TScreen _screen;

    private NTickbox? _vanillaToggle;
    private Node? _vanillaToggleParent;
    private int _vanillaToggleIndex = -1;

    private Action<bool>? _settingChangedHandler;
    private Action? _treeExitingHandler;

    protected VanillaUpgradePreviewPatch(TScreen screen) { _screen = screen;}

    protected TScreen Screen => _screen;

    protected abstract bool IsEnabled();

    protected abstract void SubscribeToSettingChanges(Action<bool> handler);

    protected abstract void UnsubscribeFromSettingChanges(Action<bool> handler);

    public void Attach()
    {
        _vanillaToggle = _screen.GetNodeOrNull<NTickbox>("%Upgrades");
        if (_vanillaToggle == null) return;

        var parent = _vanillaToggle.GetParent();
        if (parent == null) return;

        _vanillaToggleParent = parent;
        _vanillaToggleIndex = _vanillaToggle.GetIndex();

        _settingChangedHandler = OnSettingChanged;
        _treeExitingHandler = Cleanup;

        SubscribeToSettingChanges(_settingChangedHandler);
        _screen.TreeExiting += _treeExitingHandler;

        ApplySetting(IsEnabled());
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

        if (_vanillaToggle.GetParent() == _vanillaToggleParent)
        {
            _vanillaToggleParent.RemoveChild(_vanillaToggle);
        }
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
            UnsubscribeFromSettingChanges(_settingChangedHandler);
            _settingChangedHandler = null;
        }

        _treeExitingHandler = null;
    }
}