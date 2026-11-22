using System;

namespace MoreUpgradedCardPreviews.Core;

/// <summary>
///     Global state for the upgrade preview toggle.
/// </summary>
public class UpgradePreviewManager
{
    private UpgradePreviewManager()
    {
        IsShowingUpgrades = false;
    }

    public static UpgradePreviewManager Instance { get; } = new();

    /// <summary>
    ///     Gets whether upgrade previews are currently enabled.
    /// </summary>
    public bool IsShowingUpgrades { get; private set; }

    public event Action<bool>? OnToggleStateChanged;

    /// <summary>
    ///     Sets the upgrade preview state.
    /// </summary>
    public void SetShowingUpgrades(bool show)
    {
        if (IsShowingUpgrades == show) return;
        IsShowingUpgrades = show;

        //Main.Logger.Info($"Upgrade preview state changed to: {show}");
        OnToggleStateChanged?.Invoke(show);
    }

    /// <summary>
    ///     Toggles the upgrade preview state.
    /// </summary>
    public void ToggleShowingUpgrades()
    {
        SetShowingUpgrades(!IsShowingUpgrades);
    }
}