using System;

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.Core;
using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardDuplicateSelectionScreenPatch : DeckCardSelectUpgradePreviewPatch, IModPatches
{
    private CardDuplicateSelectionScreenPatch(NDeckCardSelectScreen screen) : base(screen) {}

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "moreupgradedrewardspreviews.card_duplicate_selection_screen",
                targetType: typeof(NDeckCardSelectScreen),
                methodName: nameof(NDeckCardSelectScreen.Create),
                patchType: typeof(CardDuplicateSelectionScreenPatch),
                isCritical: true,
                description: "Adds the upgrade preview toggle to the deck card selection screen - duplicate."
            )
        );
    }
    
    public static void Postfix(NDeckCardSelectScreen __result, CardSelectorPrefs prefs)
    {
        var dollysMirrorPrompt  = new LocString("relics", $"{ModelDb.GetId<DollysMirror>().Entry}.selectionScreenPrompt");
        if (prefs.Prompt.GetRawText() != dollysMirrorPrompt.GetRawText()) return; 
            
        var patch = new CardDuplicateSelectionScreenPatch(__result);
        __result.Ready += patch.Attach;
    }

    protected override bool IsEnabled()
    {
        return CardDuplicateSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardDuplicateSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(handler);
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardDuplicateSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(handler);
    }
}