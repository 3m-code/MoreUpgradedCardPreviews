using System;

using Godot;

using MegaCrit.Sts2.Core.Nodes.Combat;

using MoreUpgradedRewardsPreviews.UI;

namespace MoreUpgradedRewardsPreviews.Core;

public abstract class UpgradePreviewPatch<TScreen>(TScreen screen) where TScreen : Control
{
    private UpgradePreviewToggle? _toggle;

    private Action<bool>? _settingChangedHandler;
    private Action? _treeExitingHandler;

    protected TScreen Screen => screen;

    protected abstract bool IsEnabled();

    protected abstract void SubscribeToSettingChanges(Action<bool> handler);

    protected abstract void UnsubscribeFromSettingChanges(Action<bool> handler);

    protected abstract void ApplyPreview(bool showingUpgrades);

    protected virtual NPeekButton? GetPeekButton() { return null; }

    protected virtual bool CanAttach() { return true;}
    
    public void Attach()
    {
        if (!GodotObject.IsInstanceValid(screen)) return;
        if (!CanAttach()) return;
        
        _settingChangedHandler = OnSettingChanged;
        _treeExitingHandler = Cleanup;

        SubscribeToSettingChanges(_settingChangedHandler);
        screen.TreeExiting += _treeExitingHandler;

        UpdateToggle(IsEnabled());
    }

    private void OnSettingChanged(bool enabled)
    {
        if (!GodotObject.IsInstanceValid(screen)) return;

        UpdateToggle(enabled);
    }

    private void UpdateToggle(bool enabled)
    {
        if (enabled)
        {
            CreateToggle();
            return;
        }

        RemoveToggle();
    }

    private void CreateToggle()
    {
        if (_toggle != null) return;
        if (!GodotObject.IsInstanceValid(screen)) return;
        
        var toggle = UpgradePreviewToggle.TryCreate(screen, ApplyPreview);
        if (toggle == null) return;

        GetPeekButton()?.AddTargets(toggle.Container);

        _toggle = toggle;
        OnToggleCreated(toggle);
    }
    
    protected virtual void OnToggleCreated(UpgradePreviewToggle toggle) {}

    private void RemoveToggle()
    {
        if (_toggle == null) return;

        ApplyPreview(false);

        _toggle.Dispose();
        _toggle = null;
    }

    private void Cleanup()
    {
        if (_settingChangedHandler != null)
        {
            UnsubscribeFromSettingChanges(_settingChangedHandler);
            _settingChangedHandler = null;
        }

        RemoveToggle();
        _treeExitingHandler = null;
    }
}