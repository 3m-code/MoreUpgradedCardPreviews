using System;
using System.Linq;

using Godot;

using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.Core;
using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardGeneratorSelectionScreenPatch : UpgradePreviewPatch<NChooseACardSelectionScreen>, IModPatches
{
    private CardGeneratorSelectionScreenPatch(NChooseACardSelectionScreen screen) : base(screen) {}

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
            new CardGeneratorSelectionScreenPatch(__instance).Attach();
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to add upgrade preview toggle to {__instance.GetType().Name}: {ex}");
        }
    }

    protected override bool IsEnabled()
    {
        return CardGeneratorSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardGeneratorSelectionScreenSettingsConfig.Instance.UpgradePreviewEnabledChanged += handler;
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardGeneratorSelectionScreenSettingsConfig.Instance.UpgradePreviewEnabledChanged -= handler;
    }

    protected override void ApplyPreview(bool showingUpgrades)
    {
        var cardRow = Screen.GetNodeOrNull<Control>("CardRow");
        if (cardRow == null) return;

        foreach (var holder in cardRow.GetChildren().OfType<NGridCardHolder>())
        {
            if (!GodotObject.IsInstanceValid(holder)) continue;
            if (!holder.CardModel.IsUpgradable) continue;

            holder.SetIsPreviewingUpgrade(showingUpgrades);
        }
    }

    protected override NPeekButton? GetPeekButton()
    {
        return Screen.GetNodeOrNull<NPeekButton>("%PeekButton");
    }
}