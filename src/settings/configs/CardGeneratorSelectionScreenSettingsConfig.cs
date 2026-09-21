using MoreUpgradedRewardsPreviews.Core;
using MoreUpgradedRewardsPreviews.Settings;

namespace MoreUpgradedRewardsPreviews.Settings.Configs;

public sealed class CardGeneratorSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardGeneratorSelectionScreenSettingsConfig Instance = new();

    private CardGeneratorSelectionScreenSettingsConfig() : base(
            dataKey: "card_generator_selection_screen_settings",
            fileName: "card_generator_selection_screen_settings.json",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}