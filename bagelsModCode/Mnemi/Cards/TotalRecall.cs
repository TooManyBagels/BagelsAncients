using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace bagelsMod.bagelsModCode.Mnemi.Cards;

[Pool(typeof(EventCardPool))]
public class TotalRecall() : BagelsModCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var card = PileType.Discard.GetPile(Owner).Cards.Where(c => !c.Keywords.Contains(CardKeyword.Unplayable))
            .ToList().StableShuffle(Owner.RunState.Rng.Shuffle).ToArray()[0];
        for (var i = 0; i < DynamicVars.Cards.BaseValue; i++)
        {
            if (CombatManager.Instance.IsOverOrEnding || card is not null) return;
            if (card.TargetType == TargetType.AnyEnemy)
            {
                var target = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
                await CardCmd.AutoPlay(choiceContext, card.CreateDupe(Owner), target);
            }
            else
                await CardCmd.AutoPlay(choiceContext, card.CreateDupe(Owner), null);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.BaseValue++;
    }
}