namespace MoreUpgradedRewardsPreviews.Settings;

public static class ModSettingsRegistry
{
    public static void Register()
    {
        CardRewardSelectionScreenSettingsConfig.Register();
        CardGeneratorSelectionScreenSettingsConfig.Register();
        CombatPileSettingsConfig.Register();
    }
}