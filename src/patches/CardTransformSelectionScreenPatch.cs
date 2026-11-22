using System;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MoreUpgradedCardPreviews.Core;
using MoreUpgradedCardPreviews.Settings.Configs;
using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace MoreUpgradedCardPreviews.Patches;

public sealed class CardTransformSelectionScreenPatch : VanillaUpgradePreviewPatch<NDeckTransformSelectScreen>
{
    private CardTransformSelectionScreenPatch(NDeckTransformSelectScreen screen) : base(screen)
    {
    }

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                $"{ModInfo.Id}.deck_transform_selection_screen",
                typeof(NDeckTransformSelectScreen),
                nameof(NDeckTransformSelectScreen._Ready),
                typeof(CardTransformSelectionScreenPatch),
                true,
                "Controls the vanilla upgrade preview toggle on the card transform screen."
            )
        );
    }

    public static void Postfix(NDeckTransformSelectScreen __instance)
    {
        new CardTransformSelectionScreenPatch(__instance).Attach();
    }

    protected override bool IsEnabled()
    {
        return CardTransformSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardTransformSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(handler);
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardTransformSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(handler);
    }
}