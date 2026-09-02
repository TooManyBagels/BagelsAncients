using bagelsMod.bagelsModCode.Mnemi.Enchantments;
using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace bagelsMod.bagelsModCode.Mnemi.Cards;

[Pool(typeof(EventCardPool))]
public class TotalRecall() : BagelsModCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Replay", 2)];
    
    public override bool CanBeGeneratedInCombat => false;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        ..HoverTipFactory.FromEnchantment<Copied>(),
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var card = PileType.Discard.GetPile(Owner).Cards.Where(c => !c.Keywords.Contains(CardKeyword.Unplayable) && c.GetEnchantedReplayCount() < 1)
            .ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault();
        if (CombatManager.Instance.IsOverOrEnding || card is null) return;
        card.BaseReplayCount += DynamicVars["Replay"].IntValue;
        if (card.TargetType == TargetType.AnyEnemy)
        {
            var target = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            await CardCmd.AutoPlay(choiceContext, card, target);
        }
        else
            await CardCmd.AutoPlay(choiceContext, card, null);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Replay"].BaseValue++;
    }
}