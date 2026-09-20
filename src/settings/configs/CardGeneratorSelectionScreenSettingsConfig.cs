using MoreUpgradedRewardsPreviews.Core;

namespace MoreUpgradedRewardsPreviews.Settings.Configs;

public sealed class CardGeneratorSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardGeneratorSelectionScreenSettingsConfig Instance = new();

    private CardGeneratorSelectionScreenSettingsConfig()
        : base(
            pageId: "card_generators",
            dataKey: "card_generator_selection_screen_settings",
            fileName: "card_generator_selection_screen_settings.json",
            pageTitle: "Card Generators",
            pageDescription: "View upgrades of in-combat card generators. This includes 'Choose 1 of 3' cards and potions.",
            settingLabel: "View Upgrades in Card Generators",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}