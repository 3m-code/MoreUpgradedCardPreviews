using MoreUpgradedCardPreviews.Core;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class
    CardUpgradeSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardUpgradeSelectionScreenSettingsConfig Instance = new();

    private CardUpgradeSelectionScreenSettingsConfig()
        : base(
            "card_upgrade_selection_screen_settings",
            "card_upgrade_selection_screen_settings.json",
            static settings => settings.UpgradePreviewEnabled,
            static (settings, value) => settings.UpgradePreviewEnabled = value,
            static () => new UpgradePreviewSettings())
    {
    }
}