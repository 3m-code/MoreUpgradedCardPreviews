using MoreUpgradedRewardsPreviews.Core;
using MoreUpgradedRewardsPreviews.Settings;

namespace MoreUpgradedRewardsPreviews.Settings.Configs;

public sealed class CardDuplicateSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardDuplicateSelectionScreenSettingsConfig Instance = new();

    private CardDuplicateSelectionScreenSettingsConfig() : base(
            dataKey: "card_duplicate_selection_screen_settings",
            fileName: "card_duplicate_selection_screen_settings.json",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}