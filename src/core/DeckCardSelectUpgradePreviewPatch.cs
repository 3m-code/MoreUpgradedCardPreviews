using Godot;

using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

using MoreUpgradedRewardsPreviews.UI;

namespace MoreUpgradedRewardsPreviews.Core;

public abstract class DeckCardSelectUpgradePreviewPatch(NDeckCardSelectScreen screen) : UpgradePreviewPatch<NDeckCardSelectScreen>(screen)
{
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

        Screen.MoveChild(toggleContainer, previewContainer.GetIndex());
    }

    protected override bool CanAttach()
    {
        return Screen.GetNodeOrNull<NTickbox>("%Upgrades") == null;
    }
}