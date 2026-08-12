using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class SimpleRune() : BagelsModRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    public override Task AfterObtained()
    {
        foreach (CardModel card in (IEnumerable<CardModel>)PileType.Deck.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if (card.Rarity == CardRarity.Basic && (card.Tags.Contains<CardTag>(CardTag.Strike) || card.Tags.Contains<CardTag>(CardTag.Defend)))
            {
                CardCmd.Upgrade(card);
            }
        }

        return Task.CompletedTask;
    }
}