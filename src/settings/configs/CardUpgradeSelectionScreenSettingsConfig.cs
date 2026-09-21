using MoreUpgradedCardPreviews.Core;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class CardUpgradeSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardUpgradeSelectionScreenSettingsConfig Instance = new();

    private CardUpgradeSelectionScreenSettingsConfig()
        : base(
            dataKey: "card_upgrade_selection_screen_settings",
            fileName: "card_upgrade_selection_screen_settings.json",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}