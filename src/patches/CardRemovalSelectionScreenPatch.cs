using System;

using Godot;

using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.UI;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardRemovalSelectionScreenPatch : IModPatches
{
    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "moreupgradedrewardspreviews.card_removal_selection_screen",
                targetType: typeof(NDeckCardSelectScreen),
                methodName: "ConnectSignalsAndInitGrid",
                patchType: typeof(CardRemovalSelectionScreenPatch),
                isCritical: true,
                description: "Adds the upgrade preview toggle to the deck card selection screen."
            )
        );
    }

    public static void Postfix(NDeckCardSelectScreen __instance)
    {
        try
        {
            var grid = __instance.GetNodeOrNull<NCardGrid>("%CardGrid");
            var previewContainer = __instance.GetNodeOrNull<Control>("%PreviewContainer");

            if (grid == null || previewContainer == null) return;

            if (__instance.GetNodeOrNull<NTickbox>("%Upgrades") != null) return;

            var toggle = UpgradePreviewToggle.TryCreate(
                __instance,
                showingUpgrades =>
                {
                    if (!GodotObject.IsInstanceValid(grid)) return;

                    grid.IsShowingUpgrades = showingUpgrades;
                });
            if (toggle == null) return;

            var toggleContainer = __instance.GetNodeOrNull<Control>("UpgradePreviewToggleContainer");
            if (toggleContainer == null) return;

            int previewIndex = previewContainer.GetIndex();
            __instance.MoveChild(toggleContainer, previewIndex);
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to add upgrade preview toggle to {__instance.GetType().Name}: {ex}");
        }
    }
}
