using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace bagelsMod.bagelsModCode.Menitas.Relics;

[Pool(typeof(EventRelicPool))]
public class DragonEgg : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override bool TryModifyCardRewardAlternatives(
        Player player,
        CardReward cardReward,
        List<CardRewardAlternative> alternatives)
    {    
        if (this.Owner != player)
            return false;
        alternatives.RemoveAll(x => x.OptionId == "Skip");
        return true;
    }
    
    public override bool TryModifyCardRewardOptionsLate(
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options)
    {
        if (player != this.Owner)
            return false;
        foreach (var cardReward in cardRewards)
        {
            var card1 = cardReward.Card;
            if (!card1.IsUpgradable)
            {
                continue;
            }
            var card2 = Owner.RunState.CloneCard(card1);
            CardCmd.Upgrade(card2);
            cardReward.ModifyCard(card2, this);
        }
        return true;
    }
}