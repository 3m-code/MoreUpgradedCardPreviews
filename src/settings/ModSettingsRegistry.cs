using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Settings;

public static class ModSettingsRegistry
{
    public static void Register()
    {
        CardRewardSelectionScreenSettingsConfig.Instance.Register();
        CardRemovalSelectionScreenSettingsConfig.Instance.Register();
        CardGeneratorSelectionScreenSettingsConfig.Instance.Register();
        CombatPileSettingsConfig.Instance.Register();
    }
}