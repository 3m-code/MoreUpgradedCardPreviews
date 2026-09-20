using MegaCrit.Sts2.Core.Entities.Cards;

namespace MoreUpgradedRewardsPreviews.Settings;

public static class CombatPileSettings
{
    public static bool DrawPileEnabled { get; set; } = true;

    public static bool DiscardPileEnabled { get; set; } = true;

    public static bool ExhaustPileEnabled { get; set; } = true;

    public static bool IsEnabled(PileType pileType)
    {
        return pileType switch
        {
            PileType.Draw => DrawPileEnabled,
            PileType.Discard => DiscardPileEnabled,
            PileType.Exhaust => ExhaustPileEnabled,
            _ => false
        };
    }
}