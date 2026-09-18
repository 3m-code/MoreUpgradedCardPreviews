using System;

namespace MoreUpgradedRewardsPreviews.Core;

/// <summary>
/// Singleton manager for tracking and coordinating upgrade preview state across all screens.
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
    /// Gets the current upgrade preview state.
    /// </summary>
    public bool IsShowingUpgrades => _isShowingUpgrades;

    /// <summary>
    /// Sets the upgrade preview state and notifies listeners.
    /// </summary>
    /// <param name="show">Whether to show upgrade previews.</param>
    private void SetShowingUpgrades(bool show)
    {
        if (_isShowingUpgrades != show)
        {
            _isShowingUpgrades = show;

            Main.Logger.Info($"Upgrade preview state changed to: {show}");

            OnToggleStateChanged?.Invoke(show);
        }
    }

    /// <summary>
    /// Toggles the upgrade preview state.
    /// </summary>
    public void ToggleShowingUpgrades()
    {
        SetShowingUpgrades(!_isShowingUpgrades);
    }

}