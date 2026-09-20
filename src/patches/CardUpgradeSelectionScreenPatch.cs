using System;

using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

using MoreUpgradedRewardsPreviews.Core;
using MoreUpgradedRewardsPreviews.Settings.Configs;

namespace MoreUpgradedRewardsPreviews.Patches;

public sealed class CardUpgradeSelectionScreenPatch : VanillaUpgradePreviewPatch<NDeckUpgradeSelectScreen>
{
    private CardUpgradeSelectionScreenPatch(NDeckUpgradeSelectScreen screen) : base(screen) {}

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                id: "deck_upgrade_selection_screen_upgrade_preview",
                targetType: typeof(NDeckUpgradeSelectScreen),
                methodName: nameof(NDeckUpgradeSelectScreen._Ready),
                patchType: typeof(CardUpgradeSelectionScreenPatch),
                isCritical: true,
                description: "Controls the vanilla upgrade preview toggle on the card upgrade screen."
            )
        );
    }

    public static void Postfix(NDeckUpgradeSelectScreen __instance)
    {
        new CardUpgradeSelectionScreenPatch(__instance).Attach();
    }

    protected override bool IsEnabled() => CardUpgradeSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();

    protected override void SubscribeToSettingChanges(Action<bool> handler) => CardUpgradeSelectionScreenSettingsConfig.Instance.SubscribeToUpgradePreviewChanges(handler);

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler) => CardUpgradeSelectionScreenSettingsConfig.Instance.UnsubscribeFromUpgradePreviewChanges(handler);
}