using STS2RitsuLib;
using STS2RitsuLib.Settings;
using STS2RitsuLib.Utils.Persistence;

namespace MoreUpgradedRewardsPreviews.Settings;

public sealed class CardRemovalSelectionScreenSettings
{
    public bool UpgradePreviewEnabled  { get; set; } = true;
}

public static class CardRemovalSelectionScreenSettingsConfig
{
    private const string DataKey = "card_removal_selection_screen_settings";
    private const string FileName = "card_removal_selection_screen_settings.json";

    private static readonly ModSettingsValueBinding<CardRemovalSelectionScreenSettings, bool> UpgradePreviewBinding 
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
                defaultFactory: () => new CardRemovalSelectionScreenSettings(),
                autoCreateIfMissing: true);
        }
    }

    private static void RegisterSettingsPage()
    {
        RitsuLibFramework.RegisterModSettings(
            ModInfo.Id,
            page =>
            {
                page.WithTitle(ModSettingsText.Literal("Card Removal"))
                    .WithModDisplayName(ModSettingsText.Literal(ModInfo.DisplayName))
                    .WithDescription(ModSettingsText.Literal("View upgrades of card on removal screen."))
                    .WithVisibleOnHostSurfaces(ModSettingsHostSurface.All)
                    .AddSection("upgrade_preview", section => section
                            .WithTitle(ModSettingsText.Literal("Upgrade Preview"))
                            .AddToggle("enabled", ModSettingsText.Literal("View Upgrades in Card Removals"), UpgradePreviewBinding));
            },
            DataKey);
    }
    
    public static bool IsUpgradePreviewEnabled()
    {
        return UpgradePreviewBinding.Read();
    }
}