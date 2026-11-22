using System;
using System.Linq;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MoreUpgradedCardPreviews.Core;
using MoreUpgradedCardPreviews.Settings.Configs;
using STS2RitsuLib.Patching.Core;
using STS2RitsuLib.Patching.Models;

namespace MoreUpgradedCardPreviews.Patches;

public sealed class CardGeneratorSelectionScreenPatch : UpgradePreviewPatch<NChooseACardSelectionScreen>, IModPatches
{
    private CardGeneratorSelectionScreenPatch(NChooseACardSelectionScreen screen) : base(screen)
    {
    }

    public static void AddTo(ModPatcher patcher)
    {
        patcher.RegisterPatch(
            new ModPatchInfo(
                $"{ModInfo.Id}.card_generator_selection_screen",
                typeof(NChooseACardSelectionScreen),
                "_Ready",
                typeof(CardGeneratorSelectionScreenPatch),
                true,
                "Adds the upgrade preview toggle to the in-combat card generator selection screen."
            )
        );
    }

    public static void Postfix(NChooseACardSelectionScreen __instance)
    {
        try
        {
            new CardGeneratorSelectionScreenPatch(__instance).Attach();
        }
        catch (Exception ex)
        {
            Main.Logger.Error($"Failed to add upgrade preview toggle to {__instance.GetType().Name}: {ex}");
        }
    }

    protected override bool IsEnabled()
    {
        return CardGeneratorSelectionScreenSettingsConfig.Instance.IsUpgradePreviewEnabled();
    }

    protected override void SubscribeToSettingChanges(Action<bool> handler)
    {
        CardGeneratorSelectionScreenSettingsConfig.Instance.UpgradePreviewEnabledChanged += handler;
    }

    protected override void UnsubscribeFromSettingChanges(Action<bool> handler)
    {
        CardGeneratorSelectionScreenSettingsConfig.Instance.UpgradePreviewEnabledChanged -= handler;
    }

    protected override void ApplyPreview(bool showingUpgrades)
    {
        var cardRow = Screen.GetNodeOrNull<Control>("CardRow");
        if (cardRow == null) return;

        var children = cardRow.GetChildren();
        foreach (var holder in children.OfType<NGridCardHolder>())
        {
            if (!GodotObject.IsInstanceValid(holder)) continue;
            if (!showingUpgrades && !holder.CardModel.IsUpgradable) continue;

            holder.SetIsPreviewingUpgrade(showingUpgrades);
        }
    }

    protected override NPeekButton? GetPeekButton()
    {
        return Screen.GetNodeOrNull<NPeekButton>("%PeekButton");
    }
}