using System.Reflection;

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

using STS2RitsuLib;
using STS2RitsuLib.Interop;

using MoreUpgradedRewardsPreviews.Patches;
using MoreUpgradedRewardsPreviews.Settings;

namespace MoreUpgradedRewardsPreviews;

[ModInitializer(nameof(Initialize))]
public static class Main
{
    public static Logger Logger { get; private set; } = null!;

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();

        Logger = RitsuLibFramework.CreateLogger(ModInfo.Id);
        ModTypeDiscoveryHub.RegisterModAssembly(ModInfo.Id, assembly);
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);

        ModSettingsRegistry.Register();

        var patcher = RitsuLibFramework.CreatePatcher(ModInfo.Id, "main");
        //In-combat
        CombatPileScreenPatch.AddTo(patcher);
        CardGeneratorSelectionScreenPatch.AddTo(patcher);
        
        //Post-combat
        CardRewardSelectionScreenPatch.AddTo(patcher);
        
        //Vanilla card selection screens
        CardUpgradeSelectionScreenPatch.AddTo(patcher);
        CardTransformSelectionScreenPatch.AddTo(patcher);
        
        //Card selection screens
        CardRemovalSelectionScreenPatch.AddTo(patcher);
        CardDuplicateSelectionScreenPatch.AddTo(patcher);
        CardEnchantSelectionScreenPatch.AddTo(patcher);
        
        RitsuLibFramework.ApplyRequiredPatcher(patcher, DisableMod);
    }

    private static void DisableMod()
    {
        Logger.Error("Failed to apply required patches. Disabling mod.");
    }
}