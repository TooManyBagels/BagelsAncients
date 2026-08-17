using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
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
        var rewardCount = DynamicVars["RewardCount"].IntValue;
        for (var i = 0; i < rewardCount; ++i)
        {
            var options = CardCreationOptions.ForNonCombatWithDefaultOdds([ModelDb.CardPool<ColorlessCardPool>()])
                .WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);

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