using MoreUpgradedRewardsPreviews.Core;

namespace MoreUpgradedRewardsPreviews.Settings.Configs;

public sealed class CardUpgradeSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardUpgradeSelectionScreenSettingsConfig Instance = new();

    private CardUpgradeSelectionScreenSettingsConfig()
        : base(
            pageId: "card_upgrade",
            dataKey: "card_upgrade_selection_screen_settings",
            fileName: "card_upgrade_selection_screen_settings.json",
            pageTitle: "Card Upgrade",
            pageDescription: "View upgrades of cards on the upgrade screen.",
            settingLabel: "View Upgrades in Card Upgrades",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}