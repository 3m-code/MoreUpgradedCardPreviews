using MoreUpgradedCardPreviews.Core;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class
    CardDuplicateSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardDuplicateSelectionScreenSettingsConfig Instance = new();

    private CardDuplicateSelectionScreenSettingsConfig() : base(
        "card_duplicate_selection_screen_settings",
        "card_duplicate_selection_screen_settings.json",
        static settings => settings.UpgradePreviewEnabled,
        static (settings, value) => settings.UpgradePreviewEnabled = value,
        static () => new UpgradePreviewSettings())
    {
    }
}