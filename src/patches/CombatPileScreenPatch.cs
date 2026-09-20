using System;

using Godot;

using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.Settings;
using MoreUpgradedRewardsPreviews.UI;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CombatPileScreenPatch : IModPatches
{
    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "moreupgradedrewardspreviews.combat_pile_screen",
                targetType: typeof(NCardPileScreen),
                methodName: "_Ready",
                patchType: typeof(CombatPileScreenPatch),
                isCritical: true,
                description: "Adds the upgrade preview toggle to in-combat pile screens."
            )
        );
    }

    public static void Postfix(NCardPileScreen __instance)
    {
        try
        {
            var pileType = __instance.Pile.Type;

            if (!CombatPileSettingsConfig.IsCombatPileUpgradePreviewEnabled(pileType))
            {
                Main.Logger.Info($"Upgrade preview toggle disabled for {pileType}. No toggle created.");
                return;
            }

            var grid = __instance.GetNodeOrNull<NCardGrid>("CardGrid");
            if (grid == null)
            {
                Main.Logger.Error($"Could not find CardGrid on NCardPileScreen for {pileType} pile.");
                return;
            }

            var toggle = UpgradePreviewToggle.TryCreate(
                __instance,
                showingUpgrades =>
                {
                    if (!GodotObject.IsInstanceValid(__instance) || !GodotObject.IsInstanceValid(grid)) return;

                    grid.IsShowingUpgrades = showingUpgrades;
                });

            if (toggle == null)
            {
                Main.Logger.Error($"Failed to create upgrade preview toggle for {pileType} pile.");
                return;
            }

            Main.Logger.Info($"Upgrade preview toggle created for {pileType}.");
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to add upgrade preview toggle to NCardPileScreen: {ex}");
        }
    }
}