using System.Reflection;

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

using STS2RitsuLib;
using STS2RitsuLib.Interop;

//using MoreUpgradedRewardsPreviews.Patches;

namespace MoreUpgradedRewardsPreviews;

[ModInitializer(nameof(Initialize))]
public static class Main
{
    private const string ModId = "moreupgradedrewardspreviews";

    public static Logger Logger { get; private set; } = null!;

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();

        Logger = RitsuLibFramework.CreateLogger(ModId);
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);

        var patcher = RitsuLibFramework.CreatePatcher(ModId, "main");
        //CardRewardSelectionScreenPatch.AddTo(patcher);
        RitsuLibFramework.ApplyRequiredPatcher(patcher, DisableMod);
    }

    private static void DisableMod()
    {
        Logger.Error("Failed to apply required patches. Disabling mod.");
    }
}