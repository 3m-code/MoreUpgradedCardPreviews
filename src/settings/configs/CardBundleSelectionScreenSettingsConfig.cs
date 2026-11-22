using MoreUpgradedCardPreviews.Core;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class CardBundleSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardBundleSelectionScreenSettingsConfig Instance = new();

    private CardBundleSelectionScreenSettingsConfig() : base(
        "card_bundle_selection_screen_settings",
        "card_bundle_selection_screen_settings.json",
        static settings => settings.UpgradePreviewEnabled,
        static (settings, value) => settings.UpgradePreviewEnabled = value,
        static () => new UpgradePreviewSettings())
    {
    }
}