using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Settings;

public static class ModSettingsRegistry
{
    public static void Register()
    {
        CombatPileSettingsConfig.Instance.Register();
        CardRewardSelectionScreenSettingsConfig.Instance.Register();
        CardRemovalSelectionScreenSettingsConfig.Instance.Register();
        CardDuplicateSelectionScreenSettingsConfig.Instance.Register();
        CardGeneratorSelectionScreenSettingsConfig.Instance.Register();
    }
}