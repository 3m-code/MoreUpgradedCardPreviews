using System;

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Screens;

using MoreUpgradedCardPreviews.Core;
using MoreUpgradedCardPreviews.Settings.Configs;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace MoreUpgradedCardPreviews.Patches;

public sealed class CombatPileScreenPatch : UpgradePreviewPatch<NCardPileScreen>, IModPatches
{
    private CombatPileScreenPatch(NCardPileScreen screen) : base(screen) {}

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: $"{ModInfo.Id}.combat_pile_screen",
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
            new CombatPileScreenPatch(__instance).Attach();
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to add upgrade preview toggle to {__instance.GetType().Name}: {ex}");
        }
    }

    protected override bool IsEnabled()
    {
        return CombatPileSettingsConfig.Instance.IsUpgradePreviewEnabled(Screen.Pile.Type);
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        switch (Screen.Pile.Type)
        {
            case PileType.Draw:
                CombatPileSettingsConfig.Instance.DrawPilePreviewEnabledChanged += handler;
                break;

            case PileType.Discard:
                CombatPileSettingsConfig.Instance.DiscardPilePreviewEnabledChanged += handler;
                break;

            case PileType.Exhaust:
                CombatPileSettingsConfig.Instance.ExhaustPilePreviewEnabledChanged += handler;
                break;
        }
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        switch (Screen.Pile.Type)
        {
            case PileType.Draw:
                CombatPileSettingsConfig.Instance.DrawPilePreviewEnabledChanged -= handler;
                break;

            case PileType.Discard:
                CombatPileSettingsConfig.Instance.DiscardPilePreviewEnabledChanged -= handler;
                break;

            case PileType.Exhaust:
                CombatPileSettingsConfig.Instance.ExhaustPilePreviewEnabledChanged -= handler;
                break;
        }
    }

    protected override void ApplyPreview(bool showingUpgrades)
    {
        var grid = Screen.GetNodeOrNull<NCardGrid>("CardGrid");
        if (grid == null) return;

        grid.IsShowingUpgrades = showingUpgrades;
    }
}