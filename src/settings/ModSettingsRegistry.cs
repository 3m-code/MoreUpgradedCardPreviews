using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Settings;

public static class ModSettingsRegistry
{
    public static void Register()
    {
        CombatPileSettingsConfig.Instance.Register();
        CardRewardSelectionScreenSettingsConfig.Instance.Register();
        CardRemovalSelectionScreenSettingsConfig.Instance.Register();
        CardUpgradeSelectionScreenSettingsConfig.Instance.Register();
        CardTransformSelectionScreenSettingsConfig.Instance.Register();
        CardDuplicateSelectionScreenSettingsConfig.Instance.Register();
        CardEnchantSelectionScreenSettingsConfig.Instance.Register();
        CardGeneratorSelectionScreenSettingsConfig.Instance.Register();
    }
}