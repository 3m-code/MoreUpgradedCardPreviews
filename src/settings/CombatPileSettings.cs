using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Utils.Persistence;

namespace MoreUpgradedRewardsPreviews.Settings;

public sealed class CombatPileSettings
{
    public bool DrawPileEnabled { get; set; } = true;
    public bool DiscardPileEnabled { get; set; } = true;
    public bool ExhaustPileEnabled { get; set; } = true;
}

public static class CombatPileSettingsConfig
{
    private const string DataKey = "combat_pile_settings";
    private const string FileName = "combat_pile_settings.json";

    private static readonly ModSettingsValueBinding<CombatPileSettings, bool> DrawPileBinding 
        = new(
            ModInfo.Id,
            DataKey,
            SaveScope.Global,
            static settings => settings.DrawPileEnabled,
            static (settings, value) => settings.DrawPileEnabled = value
            );

    private static readonly ModSettingsValueBinding<CombatPileSettings, bool> DiscardPileBinding 
        = new(
            ModInfo.Id,
            DataKey,
            SaveScope.Global,
            static settings => settings.DiscardPileEnabled,
            static (settings, value) => settings.DiscardPileEnabled = value
            );

    private static readonly ModSettingsValueBinding<CombatPileSettings, bool> ExhaustPileBinding 
        = new(
            ModInfo.Id,
            DataKey,
            SaveScope.Global,
            static settings => settings.ExhaustPileEnabled,
            static (settings, value) => settings.ExhaustPileEnabled = value
            );

    public static void Register()
    {
        RegisterData();
        RegisterSettingsPage();
    }
    
    private static void RegisterData()
    {
        using (RitsuLibFramework.BeginModDataRegistration(ModInfo.Id))
        {
            var store = RitsuLibFramework.GetDataStore(ModInfo.Id);

            store.Register(
                key: DataKey,
                fileName: FileName,
                scope: SaveScope.Global,
                defaultFactory: () => new CombatPileSettings(),
                autoCreateIfMissing: true);
        }
    }

    private static void RegisterSettingsPage()
    {
        RitsuLibFramework.RegisterModSettings(
            ModInfo.Id,
            page =>
            {
                page.WithTitle(ModSettingsText.Literal("Combat Piles"))
                    .WithModDisplayName(ModSettingsText.Literal(ModInfo.DisplayName))
                    .WithDescription(ModSettingsText.Literal("View upgrades of in-combat card piles."))
                    .WithVisibleOnHostSurfaces(ModSettingsHostSurface.All)
                    .AddSection("upgrade_preview", section => section
                            .WithTitle(ModSettingsText.Literal("Upgrade Preview"))
                            .AddToggle("draw_pile", ModSettingsText.Literal("View Upgrades in Draw Pile"), DrawPileBinding)
                            .AddToggle("discard_pile", ModSettingsText.Literal("View Upgrades in Discard Pile"), DiscardPileBinding)
                            .AddToggle("exhaust_pile", ModSettingsText.Literal("View Upgrades in Exhaust Pile"), ExhaustPileBinding));
            },
            DataKey);
    }

    public static bool IsUpgradePreviewEnabled(PileType pileType)
    {
        return pileType switch
        {
            PileType.Draw => DrawPileBinding.Read(),
            PileType.Discard => DiscardPileBinding.Read(),
            PileType.Exhaust => ExhaustPileBinding.Read(),
            _ => false
        };
    }
}