using System;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MoreUpgradedCardPreviews.Core;
using MoreUpgradedCardPreviews.Settings.Configs;
using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace MoreUpgradedCardPreviews.Patches;

public sealed class CardUpgradeSelectionScreenPatch : VanillaUpgradePreviewPatch<NDeckUpgradeSelectScreen>
{
    private CardUpgradeSelectionScreenPatch(NDeckUpgradeSelectScreen screen) : base(screen)
    {
    }

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                $"{ModInfo.Id}.deck_upgrade_selection_screen",
                typeof(NDeckUpgradeSelectScreen),
                nameof(NDeckUpgradeSelectScreen._Ready),
                typeof(CardUpgradeSelectionScreenPatch),
                true,
                "Controls the vanilla upgrade preview toggle on the card upgrade screen."
            )
        );
    }

    public static void Postfix(NDeckUpgradeSelectScreen __instance)
    {
        new CardUpgradeSelectionScreenPatch(__instance).Attach();
    }

    protected override bool IsEnabled()
    {
        return CardUpgradeSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardUpgradeSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(handler);
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardUpgradeSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(handler);
    }
}