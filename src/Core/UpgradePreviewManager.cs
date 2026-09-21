using System;

namespace MoreUpgradedRewardsPreviews.Core;

/// <summary>
/// Global state for the upgrade preview toggle.
/// </summary>
public class UpgradePreviewManager
{
    private static readonly UpgradePreviewManager _instance = new();

    private bool _isShowingUpgrades;

    public event Action<bool>? OnToggleStateChanged;

    private UpgradePreviewManager()
    {
        _isShowingUpgrades = false;
    }

    public static UpgradePreviewManager Instance => _instance;

    /// <summary>
    /// Gets whether upgrade previews are currently enabled.
    /// </summary>
    public bool IsShowingUpgrades => _isShowingUpgrades;

    /// <summary>
    /// Sets the upgrade preview state.
    /// </summary>
    public void SetShowingUpgrades(bool show)
    {
        if (_isShowingUpgrades == show) return;
        _isShowingUpgrades = show;
        
        //Main.Logger.Info($"Upgrade preview state changed to: {show}");
        OnToggleStateChanged?.Invoke(show);
    }

    /// <summary>
    /// Toggles the upgrade preview state.
    /// </summary>
    public void ToggleShowingUpgrades()
    {
        SetShowingUpgrades(!_isShowingUpgrades);
    }
}