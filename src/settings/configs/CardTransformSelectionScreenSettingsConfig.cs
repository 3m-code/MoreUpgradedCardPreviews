using MoreUpgradedRewardsPreviews.Core;

namespace MoreUpgradedRewardsPreviews.Settings.Configs;

public sealed class CardTransformSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardTransformSelectionScreenSettingsConfig Instance = new();

    private CardTransformSelectionScreenSettingsConfig()
        : base(
            pageId: "card_transform",
            dataKey: "card_transform_selection_screen_settings",
            fileName: "card_transform_selection_screen_settings.json",
            pageTitle: "Card Transform",
            pageDescription: "View upgrades of cards on the transform screen.",
            settingLabel: "View Upgrades in Card Transform",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}