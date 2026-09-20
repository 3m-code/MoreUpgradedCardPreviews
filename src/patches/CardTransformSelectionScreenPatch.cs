using System;

using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.Core;
using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardTransformSelectionScreenPatch : VanillaUpgradePreviewPatch<NDeckTransformSelectScreen>
{
    private CardTransformSelectionScreenPatch(NDeckTransformSelectScreen screen) : base(screen) {}

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "deck_transform_selection_screen_upgrade_preview",
                targetType: typeof(NDeckTransformSelectScreen),
                methodName: nameof(NDeckTransformSelectScreen._Ready),
                patchType: typeof(CardTransformSelectionScreenPatch),
                isCritical: true,
                description: "Controls the vanilla upgrade preview toggle on the card transform screen."
            )
        );
    }

    public static void Postfix(NDeckTransformSelectScreen __instance)
    {
        new CardTransformSelectionScreenPatch(__instance).Attach();
    }

    protected override bool IsEnabled() => CardTransformSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();

    protected override void SubscribeToSettingChanges(Action<bool> handler) => CardTransformSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(handler);

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler) => CardTransformSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(handler);
}