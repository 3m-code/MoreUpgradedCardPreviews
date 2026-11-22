using System;
using System.Reflection;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MoreUpgradedCardPreviews.Compatibility;
using MoreUpgradedCardPreviews.Core;
using MoreUpgradedCardPreviews.Settings.Configs;
using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace MoreUpgradedCardPreviews.Patches;

[MinGameVersion("0.113.1")]
public sealed class BlitzStartingCardSelectionScreenPatch : UpgradePreviewPatch<NSimpleCardSelectScreen>
{
    private const string PromptTable = "ascension";
    private const string PromptKey = "BLITZ.NEOW.selectionScreenPrompt";

    private static readonly FieldInfo? PrefsField = typeof(NSimpleCardSelectScreen).GetField(
        "_prefs", BindingFlags.Instance | BindingFlags.NonPublic);

    private static bool _missingPrefsFieldReported;

    private BlitzStartingCardSelectionScreenPatch(NSimpleCardSelectScreen screen) : base(screen)
    {
    }

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(new ModPatchInfo(
            $"{ModInfo.Id}.blitz_starting_card_selection",
            typeof(NSimpleCardSelectScreen),
            nameof(NSimpleCardSelectScreen._Ready),
            typeof(BlitzStartingCardSelectionScreenPatch),
            false,
            "Adds upgrade previews to Blitz's initial card selection."));
    }

    public static void Postfix(NSimpleCardSelectScreen __instance)
    {
        try
        {
            if (PrefsField == null)
            {
                if (!_missingPrefsFieldReported)
                {
                    _missingPrefsFieldReported = true;
                    Main.Logger.Error(
                        "Blitz starting card selection: NSimpleCardSelectScreen._prefs not found. " +
                        "The game API may have changed.");
                }

                return;
            }

            if (PrefsField.GetValue(__instance) is not CardSelectorPrefs prefs)
                return;

            if (!string.Equals(prefs.Prompt.LocTable, PromptTable, StringComparison.Ordinal) ||
                !string.Equals(prefs.Prompt.LocEntryKey, PromptKey, StringComparison.Ordinal))
                return;

            Main.Logger.Info("Attaching Blitz starting card upgrade preview.");
            new BlitzStartingCardSelectionScreenPatch(__instance).Attach();
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to attach Blitz starting card upgrade preview: {ex}");
        }
    }

    protected override bool IsEnabled()
    {
        return BlitzStartingCardSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        BlitzStartingCardSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(handler);
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        BlitzStartingCardSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(handler);
    }

    protected override NPeekButton? GetPeekButton()
    {
        return Screen.GetNodeOrNull<NPeekButton>("%PeekButton");
    }

    protected override void ApplyPreview(bool showingUpgrades)
    {
        var grid = Screen.GetNodeOrNull<NCardGrid>("%CardGrid");
        if (grid == null)
            return;

        grid.IsShowingUpgrades = showingUpgrades;
    }
}