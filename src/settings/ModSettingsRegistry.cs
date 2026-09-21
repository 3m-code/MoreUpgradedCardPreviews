using STS2RitsuLib;
using STS2RitsuLib.Settings;

using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Settings;

public static class ModSettingsRegistry
{
    public static void Register()
    {
        RegisterSettingsData();
        RegisterSettingsPages();
    }
    
    private static void RegisterSettingsData()
    {
        CombatPileSettingsConfig.Instance.Register();
        CardGeneratorSelectionScreenSettingsConfig.Instance.Register();
        
        CardRewardSelectionScreenSettingsConfig.Instance.Register();

        CardUpgradeSelectionScreenSettingsConfig.Instance.Register();
        CardTransformSelectionScreenSettingsConfig.Instance.Register();
        
        CardRemovalSelectionScreenSettingsConfig.Instance.Register();
        CardDuplicateSelectionScreenSettingsConfig.Instance.Register();
        CardEnchantSelectionScreenSettingsConfig.Instance.Register();
    }
    
    private static void RegisterSettingsPages()
    {
        RegisterInCombatPage();
        RegisterPostCombatPage();
        RegisterSelectionScreensPage();
    }
    
        private static void RegisterInCombatPage()
    {
        RitsuLibFramework.RegisterModSettings(
            ModInfo.Id,
            page =>
            {
                page.WithTitle(ModSettingsText.Literal("In-combat"))
                    .WithModDisplayName(ModSettingsText.Literal(ModInfo.DisplayName))
                    .WithDescription(ModSettingsText.Literal("Settings for card selection and piles during combat."))
                    .WithVisibleOnHostSurfaces(ModSettingsHostSurface.All)
                    .AddSection("combat_piles", section => section.WithTitle(ModSettingsText.Literal("Combat Piles"))
                        .AddToggle("draw_pile", ModSettingsText.Literal("View Upgrades in Draw Pile"), CombatPileSettingsConfig.Instance.DrawPileBinding)
                        .AddToggle("discard_pile", ModSettingsText.Literal("View Upgrades in Discard Pile"), CombatPileSettingsConfig.Instance.DiscardPileBinding)
                        .AddToggle("exhaust_pile", ModSettingsText.Literal("View Upgrades in Exhaust Pile"), CombatPileSettingsConfig.Instance.ExhaustPileBinding))
                    .AddSection("card_generator", section => section.WithTitle(ModSettingsText.Literal("Card Generator"))
                        .AddToggle("upgrade_preview", ModSettingsText.Literal("View Upgrades when getting a card"), CardGeneratorSelectionScreenSettingsConfig.Instance.UpgradePreviewBinding));
            },
            "in_combat");
    }

    private static void RegisterPostCombatPage()
    {
        RitsuLibFramework.RegisterModSettings(
            ModInfo.Id,
            page =>
            {
                page.WithTitle(ModSettingsText.Literal("Post-combat"))
                    .WithModDisplayName(ModSettingsText.Literal(ModInfo.DisplayName))
                    .WithDescription(ModSettingsText.Literal("Settings for card rewards after combat."))
                    .WithVisibleOnHostSurfaces(ModSettingsHostSurface.All)
                    .AddSection("card_rewards", section => section.WithTitle(ModSettingsText.Literal("Card Rewards"))
                            .AddToggle("upgrade_preview", ModSettingsText.Literal("View Upgrades in card rewards"), CardRewardSelectionScreenSettingsConfig.Instance.UpgradePreviewBinding));
            },
            "post_combat");
    }

    private static void RegisterSelectionScreensPage()
    {
        RitsuLibFramework.RegisterModSettings(
            ModInfo.Id,
            page =>
            {
                page.WithTitle(ModSettingsText.Literal("Selection screens"))
                    .WithModDisplayName(ModSettingsText.Literal(ModInfo.DisplayName))
                    .WithDescription(ModSettingsText.Literal("Settings for card selection screens."))
                    .WithVisibleOnHostSurfaces(ModSettingsHostSurface.All)
                    .AddSection("vanilla", section => section.WithTitle(ModSettingsText.Literal("Vanilla"))
                            .AddToggle("card_upgrade", ModSettingsText.Literal("View Upgrades in Card Upgrade Screen"), CardUpgradeSelectionScreenSettingsConfig.Instance.UpgradePreviewBinding)
                            .AddToggle("card_transform", ModSettingsText.Literal("View Upgrades in Card Transform Screen"), CardTransformSelectionScreenSettingsConfig.Instance.UpgradePreviewBinding))
                    .AddSection("added", section => section.WithTitle(ModSettingsText.Literal("Added"))
                            .AddToggle("card_removal", ModSettingsText.Literal("View Upgrades in Card Removal Screen"), CardRemovalSelectionScreenSettingsConfig.Instance.UpgradePreviewBinding)
                            .AddToggle("card_duplicate", ModSettingsText.Literal("View Upgrades in Card Duplicate Screen"), CardDuplicateSelectionScreenSettingsConfig.Instance.UpgradePreviewBinding)
                            .AddToggle("card_enchant", ModSettingsText.Literal("View Upgrades in Card Enchant Screen"), CardEnchantSelectionScreenSettingsConfig.Instance.UpgradePreviewBinding));
            },
            "selection_screens");
    }
}