using MoreUpgradedRewardsPreviews.Core;

namespace MoreUpgradedRewardsPreviews.Settings.Configs;

public sealed class CardEnchantSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<CardEnchantSelectionScreenSettings>
{
    public static readonly CardEnchantSelectionScreenSettingsConfig Instance = new();

    private CardEnchantSelectionScreenSettingsConfig()
        : base(
            pageId: "card_enchant",
            dataKey: "card_enchant_selection_screen_settings",
            fileName: "card_enchant_selection_screen_settings.json",
            pageTitle: "Card Enchant",
            pageDescription: "View upgrades of cards on the enchant screen.",
            settingLabel: "View Upgrades in Card Enchants",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new CardEnchantSelectionScreenSettings())
    {}
}