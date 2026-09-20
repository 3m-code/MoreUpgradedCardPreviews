using System;

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.Core;
using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardRemovalSelectionScreenPatch : DeckCardSelectUpgradePreviewPatch, IModPatches
{
    private CardRemovalSelectionScreenPatch(NDeckCardSelectScreen screen) : base(screen) {}

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "moreupgradedrewardspreviews.card_removal_selection_screen",
                targetType: typeof(NDeckCardSelectScreen),
                methodName: nameof(NDeckCardSelectScreen.Create),
                patchType: typeof(CardRemovalSelectionScreenPatch),
                isCritical: true,
                description: "Adds the upgrade preview toggle to the deck card selection screen - removal."
            )
        );
    }

    public static void Postfix(NDeckCardSelectScreen __result, CardSelectorPrefs prefs)
    {
        if (prefs.Prompt.GetRawText() != CardSelectorPrefs.RemoveSelectionPrompt.GetRawText()) return;
        
        var patch = new CardRemovalSelectionScreenPatch(__result);
        __result.Ready += patch.Attach;
    }

    protected override bool IsEnabled()
    {
        return CardRemovalSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardRemovalSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(handler);
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardRemovalSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(handler);
    }
}