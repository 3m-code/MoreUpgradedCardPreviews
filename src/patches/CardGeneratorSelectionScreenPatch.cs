using System;
using System.Linq;

using Godot;

using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Combat;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.UI;
using MoreUpgradedRewardsPreviews.Settings;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardGeneratorSelectionScreenPatch : IModPatches
{
    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "moreupgradedrewardspreviews.card_generator_selection_screen",
                targetType: typeof(NChooseACardSelectionScreen),
                methodName: "_Ready",
                patchType: typeof(CardGeneratorSelectionScreenPatch),
                isCritical: true,
                description: "Adds the upgrade preview toggle to the in-combat card generator selection screen."
            )
        );
    }

    public static void Postfix(NChooseACardSelectionScreen __instance)
    {
        try
        {
            if (!CardGeneratorSelectionScreenSettingsConfig.IsUpgradePreviewEnabled())
            {
                Main.Logger.Info($"Upgrade preview toggle disabled for NCardRewardSelectionScreen. No toggle created.");
                return;
            }
            
            var cardRow = __instance.GetNodeOrNull<Control>("CardRow");
            if (cardRow == null) return;

            var toggle = UpgradePreviewToggle.TryCreate(
                __instance,
                showingUpgrades =>
                {
                    if (!GodotObject.IsInstanceValid(__instance) || !GodotObject.IsInstanceValid(cardRow)) return;

                    foreach (var holder in cardRow.GetChildren().OfType<NGridCardHolder>())
                    {
                        if (GodotObject.IsInstanceValid(holder) && holder.CardModel.IsUpgradable)
                        {
                            holder.SetIsPreviewingUpgrade(showingUpgrades);
                        }
                    }
                });
            
            var peekButton = __instance.GetNodeOrNull<NPeekButton>("%PeekButton");
            if (toggle != null && peekButton != null)
            {
                peekButton.AddTargets(toggle.Container);
            }
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to add upgrade preview toggle to {__instance.GetType().Name}: {ex}");
        }
    }
}