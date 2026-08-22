using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace bagelsMod.bagelsModCode.Menitas.Relics;

[Pool(typeof(EventRelicPool))]
public class SimpleRune : BagelsModRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    public override Task AfterObtained()
    {
        NRun.Instance?.GlobalUi.GridCardPreviewContainer.ForceMaxColumnsUntilEmpty(4);
        foreach (var card in PileType.Deck.GetPile(Owner).Cards.ToList())
        {
            if (card.Rarity == CardRarity.Basic && (card.Tags.Contains(CardTag.Strike) || card.Tags.Contains(CardTag.Defend)))
            {
                CardCmd.Upgrade(card, CardPreviewStyle.GridLayout);
            }
        }

        return Task.CompletedTask;
    }
}