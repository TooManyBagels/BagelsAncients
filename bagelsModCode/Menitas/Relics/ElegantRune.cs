using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace bagelsMod.bagelsModCode.Menitas.Relics;

[Pool(typeof(EventRelicPool))]
public class ElegantRune : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3), new DynamicVar("DowngradeNum", 2)];

    public override async Task AfterObtained()
    {
        await CardPileCmd.RemoveFromDeck( (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, this.DynamicVars.Cards.IntValue))).ToList());
        
        await Cmd.CustomScaledWait(0.3f, 0.5f);
        int i;
        for (i = 0; i < 2; ++i)
        {
            var card = this.Owner.RunState.Rng.Niche.NextItem(PileType.Deck.GetPile(this.Owner).Cards.Where(c => !c.IsUpgradable));
            if (card == null)
            {
                break;
            }
            CardCmd.Downgrade(card);
            CardCmd.Preview(card, style: CardPreviewStyle.MessyLayout);
            await Cmd.CustomScaledWait(0.3f, 0.5f);
        }
    }
}