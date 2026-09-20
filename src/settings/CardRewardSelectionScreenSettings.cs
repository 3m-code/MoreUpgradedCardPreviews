using STS2RitsuLib;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Utils.Persistence;

namespace MoreUpgradedRewardsPreviews.Settings;

public sealed class CardRewardSelectionScreenSettings
{
    public bool UpgradePreviewEnabled  { get; set; } = true;
}

public static class CardRewardSelectionScreenSettingsConfig
{
    private const string DataKey = "card_reward_selection_screen_settings";
    private const string FileName = "card_reward_selection_screen_settings.json";

    private static readonly ModSettingsValueBinding<CardRewardSelectionScreenSettings, bool> UpgradePreviewBinding 
        = new(
            ModInfo.Id,
            DataKey,
            SaveScope.Global,
            static settings => settings.UpgradePreviewEnabled,
            static (settings, value) => settings.UpgradePreviewEnabled = value);

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
                defaultFactory: () => new CardRewardSelectionScreenSettings(),
                autoCreateIfMissing: true);
        }
    }

    private static void RegisterSettingsPage()
    {
        RitsuLibFramework.RegisterModSettings(
            ModInfo.Id,
            page =>
            {
                page.WithTitle(ModSettingsText.Literal("Card Rewards"))
                    .WithModDisplayName(ModSettingsText.Literal(ModInfo.DisplayName))
                    .WithDescription(ModSettingsText.Literal("View upgrades of post-combat card rewards."))
                    .WithVisibleOnHostSurfaces(ModSettingsHostSurface.All)
                    .AddSection("upgrade_preview", section => section
                            .WithTitle(ModSettingsText.Literal("Upgrade Preview"))
                            .AddToggle("enabled", ModSettingsText.Literal("View Upgrades in Card Rewards"), UpgradePreviewBinding));
            },
            DataKey);
    }
    
    public static bool IsUpgradePreviewEnabled()
    {
        return UpgradePreviewBinding.Read();
    }
}