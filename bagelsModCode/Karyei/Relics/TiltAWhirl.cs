using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Karyei.Relics;

[Pool(typeof(EventRelicPool))]
public class TiltAWhirl : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override async Task AfterAutoPostPlayPhaseEntered(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player != Owner)
        {
            return;
        }
        var hand = PileType.Hand.GetPile(Owner);
        var card = Owner.RunState.Rng.Shuffle.NextItem<CardModel>(hand.Cards.Where(c => !c.Keywords.Contains(CardKeyword.Unplayable)).ToList());
        if (card != null) 
            await CardCmd.AutoPlay(choiceContext, card, null);
        hand = PileType.Hand.GetPile(Owner);
        card = Owner.RunState.Rng.Shuffle.NextItem(hand.Cards.ToList());
        if (card != null) 
            await CardCmd.Exhaust(choiceContext, card);
    }
}