using System.Linq;

using Godot;

using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.UI;
using MoreUpgradedRewardsPreviews.Settings;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardRewardSelectionScreenPatch : IModPatches
{
    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "moreupgradedrewardspreviews.card_reward_selection_screen",
                targetType: typeof(NCardRewardSelectionScreen),
                methodName: "_Ready",
                patchType: typeof(CardRewardSelectionScreenPatch),
                isCritical: true,
                description: "Adds upgrade preview toggle to card rewards."
            )
        );
    }

    public static void Postfix(NCardRewardSelectionScreen __instance)
    {
        if (!CardRewardSelectionScreenSettingsConfig.IsUpgradePreviewEnabled())
        {
            Main.Logger.Info($"Upgrade preview toggle disabled for NCardRewardSelectionScreen. No toggle created.");
            return;
        }
        
        UpgradePreviewToggle.TryCreate(__instance, showingUpgrades => ApplyUpgradePreview(__instance, showingUpgrades));
    }

    private static void ApplyUpgradePreview(NCardRewardSelectionScreen screen, bool showingUpgrades)
    {
        if (!GodotObject.IsInstanceValid(screen)) return;
        
        var cardRow = screen.GetNodeOrNull<Control>("UI/CardRow");
        if (cardRow == null) return;

        foreach (var holder in cardRow.GetChildren().OfType<NGridCardHolder>())
        {
            if (!showingUpgrades || holder.CardModel.IsUpgradable) holder.SetIsPreviewingUpgrade(showingUpgrades);
        }
    }
}