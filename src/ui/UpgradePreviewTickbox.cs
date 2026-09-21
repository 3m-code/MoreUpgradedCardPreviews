using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MoreUpgradedCardPreviews.UI;

public sealed partial class UpgradePreviewTickbox : NTickbox
{
    public override void _Ready()
    {
        ConnectSignals();
    }

    protected override void OnPress(){}
    protected override void OnFocus(){}
    protected override void OnUnfocus(){}

    protected override void OnRelease()
    {
        IsTicked = !IsTicked;
        if(IsTicked)
        {
            SfxCmd.Play("event:/sfx/ui/clicks/ui_checkbox_on");
        }
        else
        {
            SfxCmd.Play("event:/sfx/ui/clicks/ui_checkbox_off");
        }

        EmitSignalToggled(this);
    }
}