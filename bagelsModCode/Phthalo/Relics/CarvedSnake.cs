using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class CarvedSnake : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || player.PlayerCombatState is null) return base.AfterAutoPrePlayPhaseEntered(choiceContext, player);
        var hand = PileType.Hand.GetPile(player).Cards.Where(c => !c.Keywords.Contains(CardKeyword.Unplayable) || !c.EnergyCost.CostsX).ToList();
        var expensiveCost = 0;
        foreach (var c in hand)
        {
            if (c.EnergyCost.Canonical > expensiveCost)
            {
                expensiveCost =  c.EnergyCost.Canonical;
            }
        }

        var card = hand.Where(c => c.EnergyCost.Canonical == expensiveCost).ToList().StableShuffle(Owner.RunState.Rng.CombatCardSelection).Take(1).ToList()[0];
        var newCost = Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
        card.EnergyCost.SetThisCombat(newCost);
        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        return base.AfterAutoPrePlayPhaseEntered(choiceContext, player);
    }
}