using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace bagelsMod.bagelsModCode.Karyei.Relics;

[Pool(typeof(EventRelicPool))]
public class SensoryStone : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("RewardCount", 3)];
    
    public override async Task AfterObtained()
    {
        var rewards = new List<Reward>();
        var cardRarityArray = new CardRarity[3]
        {
            CardRarity.Uncommon,
            CardRarity.Uncommon,
            CardRarity.Rare
        };
        foreach (var cardRarity in cardRarityArray)
        {
            var options = CardCreationOptions.ForNonCombatWithUniformOdds([ModelDb.CardPool<ColorlessCardPool>()], (c => c.Rarity == cardRarity)).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
            var reward = new CardReward(options, 3, Owner);
            reward.AfterGenerated += () =>
            {
                foreach (var card in reward.Cards)
                {
                    CardCmd.Upgrade(card);
                }
            };
            rewards.Add(reward);
        }
        await RewardsCmd.OfferCustom(Owner, rewards);
    }
}