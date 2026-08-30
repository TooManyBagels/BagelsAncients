using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Mnemi.Relics;

[Pool(typeof(EventRelicPool))]
public class Scrapbook : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Retain)
    ];

    public override Task BeforeSideTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        var hand = PileType.Hand.GetPile(Owner);
        var card = Owner.RunState.Rng.Shuffle.NextItem(hand.Cards.Where(c => !c.Keywords.Contains(CardKeyword.Unplayable)).ToList());
        if (card == null) return base.BeforeSideTurnEndEarly(choiceContext, side, participants);
        card.GiveSingleTurnRetain();
        if(!card.EnergyCost.CostsX || !card.Keywords.Contains(CardKeyword.Unplayable))
            card.EnergyCost.AddUntilPlayed(-1);
        return base.BeforeSideTurnEndEarly(choiceContext, side, participants);
    }
}