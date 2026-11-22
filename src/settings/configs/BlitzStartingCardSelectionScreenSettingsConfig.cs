using MoreUpgradedCardPreviews.Core;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class BlitzStartingCardSelectionScreenSettingsConfig
    : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly BlitzStartingCardSelectionScreenSettingsConfig Instance = new();

    private BlitzStartingCardSelectionScreenSettingsConfig() : base(
        "blitz_starting_card_selection_screen_settings",
        "blitz_starting_card_selection_screen_settings.json",
        static settings => settings.UpgradePreviewEnabled,
        static (settings, value) => settings.UpgradePreviewEnabled = value,
        static () => new UpgradePreviewSettings())
    {
    }
}