using System;
using System.Linq;
using Godot;

using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.Core;
using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardRewardSelectionScreenPatch : UpgradePreviewPatch<NCardRewardSelectionScreen>, IModPatches
{
    private CardRewardSelectionScreenPatch(NCardRewardSelectionScreen screen) : base(screen) {}

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
        try
        {
            new CardRewardSelectionScreenPatch(__instance).Attach();
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to add upgrade preview toggle to {__instance.GetType().Name}: {ex}");
        }
    }

    protected override bool IsEnabled()
    {
        return CardRewardSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardRewardSelectionScreenSettingsConfig.Instance.UpgradePreviewEnabledChanged += handler;
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardRewardSelectionScreenSettingsConfig.Instance.UpgradePreviewEnabledChanged -= handler;
    }

    protected override void ApplyPreview(bool showingUpgrades)
    {
        var cardRow = Screen.GetNodeOrNull<Control>("UI/CardRow");
        if (cardRow == null) return;

        var children = cardRow.GetChildren();
        foreach (var holder in children.OfType<NGridCardHolder>())
        {
            if (!GodotObject.IsInstanceValid(holder)) continue;
            if (!showingUpgrades && !holder.CardModel.IsUpgradable) continue;

            holder.SetIsPreviewingUpgrade(showingUpgrades);
        }
    }
}