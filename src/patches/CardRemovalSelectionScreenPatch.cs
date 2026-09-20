using System;

using Godot;

using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.Core;
using MoreUpgradedRewardsPreviews.UI;
using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardRemovalSelectionScreenPatch : UpgradePreviewPatch<NDeckCardSelectScreen>, IModPatches
{
    private CardRemovalSelectionScreenPatch(NDeckCardSelectScreen screen) : base(screen) {}

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "moreupgradedrewardspreviews.card_removal_selection_screen",
                targetType: typeof(NDeckCardSelectScreen),
                methodName: "ConnectSignalsAndInitGrid",
                patchType: typeof(CardRemovalSelectionScreenPatch),
                isCritical: true,
                description: "Adds the upgrade preview toggle to the deck card selection screen - removal."
            )
        );
    }

    public static void Postfix(NDeckCardSelectScreen __instance)
    {
        try
        {
            new CardRemovalSelectionScreenPatch(__instance).Attach();
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to add upgrade preview toggle to {__instance.GetType().Name}: {ex}");
        }
    }

    protected override bool IsEnabled()
    {
        return CardRemovalSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override bool CanAttach()
    {
        return Screen.GetNodeOrNull<NTickbox>("%Upgrades") == null;
    }
    
    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardRemovalSelectionScreenSettingsConfig.Instance.UpgradePreviewEnabledChanged += handler;
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardRemovalSelectionScreenSettingsConfig.Instance.UpgradePreviewEnabledChanged -= handler;
    }

    protected override void ApplyPreview(bool showingUpgrades)
    {
        var grid = Screen.GetNodeOrNull<NCardGrid>("%CardGrid");
        if (grid == null) return;

        grid.IsShowingUpgrades = showingUpgrades;
    }

    protected override void OnToggleCreated(UpgradePreviewToggle toggle)
    {
        var previewContainer = Screen.GetNodeOrNull<Control>("%PreviewContainer");
        if (previewContainer == null) return;

        var toggleContainer = toggle.Container;
        if (!GodotObject.IsInstanceValid(toggleContainer)) return;

        var previewIndex = previewContainer.GetIndex();
        Screen.MoveChild(toggleContainer, previewIndex);
    }
}