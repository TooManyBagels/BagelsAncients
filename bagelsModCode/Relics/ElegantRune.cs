using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace bagelsMod.bagelsModCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class ElegantRune() : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    public override async Task AfterObtained()
    {
        await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, this.DynamicVars.Cards.IntValue))).ToList<CardModel>());
        
        await Cmd.CustomScaledWait(0.3f, 0.5f);
        int i;
        for (i = 0; i < 2; ++i)
        {
            CardModel card = this.Owner.RunState.Rng.Niche.NextItem<CardModel>(PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => !c.IsUpgradable)));
            if (card != null)
            {
                CardCmd.Downgrade(card);
                CardCmd.Preview(card, style: CardPreviewStyle.MessyLayout);
                await Cmd.CustomScaledWait(0.3f, 0.5f);
            }
        }
    }
}