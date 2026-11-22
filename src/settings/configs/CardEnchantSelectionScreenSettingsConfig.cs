using MoreUpgradedCardPreviews.Core;

namespace MoreUpgradedCardPreviews.Settings.Configs;

public sealed class
    CardEnchantSelectionScreenSettingsConfig : SingleUpgradePreviewSettingsConfig<UpgradePreviewSettings>
{
    public static readonly CardEnchantSelectionScreenSettingsConfig Instance = new();

    private CardEnchantSelectionScreenSettingsConfig() : base(
        "card_enchant_selection_screen_settings",
        "card_enchant_selection_screen_settings.json",
        static settings => settings.UpgradePreviewEnabled,
        static (settings, value) => settings.UpgradePreviewEnabled = value,
        static () => new UpgradePreviewSettings())
    {
    }
}