using System;
using System.Reflection;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Nodes;
using STS2RitsuLib.Patching.Core;

namespace MoreUpgradedCardPreviews.Compatibility;

public static class GameCompatibility
{
    public static bool IsSupported<TPatch>()
    {
        return IsSupported(typeof(TPatch));
    }

    public static bool IsSupported(Type patchType)
    {
        var minVersion = patchType.GetCustomAttribute<MinGameVersionAttribute>()?.Version;
        if (minVersion == null) return true;

        if (!SemanticVersion.TryFromString(minVersion, out var required) || required == null)
        {
            Main.Logger.Error($"Invalid minimum game version '{minVersion}' for {patchType.Name}.");
            return false;
        }

        var gameVersion = NGame.GetGameVersion();
        if (!SemanticVersion.TryFromString(gameVersion, out var current) || current == null)
        {
            Main.Logger.Info($"Cannot determine game version '{gameVersion}'; disabling {patchType.Name}.");
            return false;
        }

        return current.CompareTo(required) >= 0;
    }

    public static void RegisterPatch<TPatch>(ModPatcher patcher, Action<ModPatcher> register)
    {
        if (!IsSupported<TPatch>())
        {
            Main.Logger.Info($"Skipping {typeof(TPatch).Name}: unsupported game version.");
            return;
        }

        register(patcher);
    }
}