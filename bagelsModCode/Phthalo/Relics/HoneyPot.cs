using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class HoneyPot : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(1),
        new HealVar(3)
    ];

    public override async Task AfterCardChangedPiles(
        CardModel card,
        PileType oldPileType,
        AbstractModel? clonedBy)
    {
        if (Owner.Creature.IsDead || card.Owner != Owner)
            return;
        if (card.Pile is null || card.Pile.Type != PileType.Deck)
            return;
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        Flash();
    }
    
    public override bool TryModifyCardRewardAlternatives(
        Player player,
        CardReward cardReward,
        List<MegaCrit.Sts2.Core.Entities.CardRewardAlternatives.CardRewardAlternative> alternatives)
    {
        if (this.Owner != player)
            return false;
        alternatives.Add(new MegaCrit.Sts2.Core.Entities.CardRewardAlternatives.CardRewardAlternative("TAKE ALL", () => TakeAll(cardReward), PostAlternateCardRewardAction.EndSelectionAndCompleteReward));
        return true;
    }

    private async Task TakeAll(CardReward cardReward)
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardReward.Cards, PileType.Deck), 2);
    }
}