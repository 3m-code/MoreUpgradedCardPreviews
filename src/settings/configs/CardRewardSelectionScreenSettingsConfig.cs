using MoreUpgradedRewardsPreviews.Core;

namespace MoreUpgradedRewardsPreviews.Settings.Configs;

public sealed class CardRewardSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardRewardSelectionScreenSettingsConfig Instance = new();

    private CardRewardSelectionScreenSettingsConfig()
        : base(
            pageId: "card_rewards",
            dataKey: "card_reward_selection_screen_settings",
            fileName: "card_reward_selection_screen_settings.json",
            pageTitle: "Card Rewards",
            pageDescription: "View upgrades of post-combat card rewards.",
            settingLabel: "View Upgrades in Card Rewards",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}