using MoreUpgradedCardPreviews.Core;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class
    CardGeneratorSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardGeneratorSelectionScreenSettingsConfig Instance = new();

    private CardGeneratorSelectionScreenSettingsConfig() : base(
        "card_generator_selection_screen_settings",
        "card_generator_selection_screen_settings.json",
        static settings => settings.UpgradePreviewEnabled,
        static (settings, value) => settings.UpgradePreviewEnabled = value,
        static () => new UpgradePreviewSettings())
    {
    }
}