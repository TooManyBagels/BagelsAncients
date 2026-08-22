using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace bagelsMod.bagelsModCode.Menitas.Cards;

[Pool(typeof(EventCardPool))]
public class Improvise() : BagelsModCard(0,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var cardsToAdd = PileType.Draw.GetPile(Owner).Cards
            .Where((c => c.IsUpgradable)).ToList()
            .StableShuffle(Owner.RunState.Rng.CombatCardSelection).Take(DynamicVars.Cards.IntValue);
        foreach(var card in cardsToAdd)
        {
            await CardPileCmd.Add(card, PileType.Hand);
            CardCmd.Upgrade(card);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
}