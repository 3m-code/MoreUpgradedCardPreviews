using MoreUpgradedCardPreviews.Core;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class
    CardTransformSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardTransformSelectionScreenSettingsConfig Instance = new();

    private CardTransformSelectionScreenSettingsConfig() : base(
        "card_transform_selection_screen_settings",
        "card_transform_selection_screen_settings.json",
        static settings => settings.UpgradePreviewEnabled,
        static (settings, value) => settings.UpgradePreviewEnabled = value,
        static () => new UpgradePreviewSettings())
    {
    }
}