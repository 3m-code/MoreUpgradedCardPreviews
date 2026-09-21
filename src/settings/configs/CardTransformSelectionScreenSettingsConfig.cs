using MoreUpgradedCardPreviews.Core;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class CardTransformSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardTransformSelectionScreenSettingsConfig Instance = new();

    private CardTransformSelectionScreenSettingsConfig() : base(
            dataKey: "card_transform_selection_screen_settings",
            fileName: "card_transform_selection_screen_settings.json",
            getEnabled: static settings => settings.UpgradePreviewEnabled,
            setEnabled: static (settings, value) => settings.UpgradePreviewEnabled = value,
            defaultFactory: static () => new UpgradePreviewSettings())
    {}
}