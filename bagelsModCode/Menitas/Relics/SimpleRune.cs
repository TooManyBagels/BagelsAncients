using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Menitas.Relics;

[Pool(typeof(EventRelicPool))]
public class SimpleRune : BagelsModRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    public override Task AfterObtained()
    {
        foreach (var card in PileType.Deck.GetPile(Owner).Cards.ToList())
        {
            if (card.Rarity == CardRarity.Basic && (card.Tags.Contains(CardTag.Strike) || card.Tags.Contains(CardTag.Defend)))
            {
                CardCmd.Upgrade(card);
            }
        }

        return Task.CompletedTask;
    }
}