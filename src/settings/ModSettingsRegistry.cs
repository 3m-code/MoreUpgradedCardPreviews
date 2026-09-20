namespace MoreUpgradedRewardsPreviews.Settings;

public static class ModSettingsRegistry
{
    public static void Register()
    {
        CardRewardSelectionScreenSettingsConfig.Register();
        CardGeneratorSelectionScreenSettingsConfig.Register();
        CardRemovalSelectionScreenSettingsConfig.Register();
        CombatPileSettingsConfig.Register();
    }
}