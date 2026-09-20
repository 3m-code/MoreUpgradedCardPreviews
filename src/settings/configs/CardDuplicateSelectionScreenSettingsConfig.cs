using MoreUpgradedRewardsPreviews.Core;

namespace MoreUpgradedRewardsPreviews.Settings.Configs;

public sealed class CardDuplicateSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardDuplicateSelectionScreenSettingsConfig Instance = new();

    private CardDuplicateSelectionScreenSettingsConfig()
        : base(
            pageId: "card_duplicate",
            dataKey: "card_duplicate_selection_screen_settings",
            fileName: "card_duplicate_selection_screen_settings.json",
            pageTitle: "Card Duplicate",
            pageDescription: "View upgrades of cards on the duplicate screen. For example for Dolly's Mirror",
            settingLabel: "View Upgrades in Card Duplicate",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}