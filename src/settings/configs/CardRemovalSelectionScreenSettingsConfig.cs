using MoreUpgradedRewardsPreviews.Core;

namespace MoreUpgradedRewardsPreviews.Settings.Configs;

public sealed class CardRemovalSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardRemovalSelectionScreenSettingsConfig Instance = new();

    private CardRemovalSelectionScreenSettingsConfig()
        : base(
            pageId: "card_removal",
            dataKey: "card_removal_selection_screen_settings",
            fileName: "card_removal_selection_screen_settings.json",
            pageTitle: "Card Removal",
            pageDescription: "View upgrades of cards on the removal screen.",
            settingLabel: "View Upgrades in Card Removals",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}