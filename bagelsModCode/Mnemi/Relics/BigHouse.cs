using bagelsMod.bagelsModCode.Templates;
using BaseLib.Common.Rewards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace bagelsMod.bagelsModCode.Mnemi.Relics;

[Pool(typeof(EventRelicPool))]
public class BigHouse : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(100),
        new MaxHpVar(10)
    ];

    public override async Task AfterObtained()
    {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
        await RewardsCmd.OfferCustom(Owner, GenerateRewards());
    }

    private List<Reward> GenerateRewards()
    {
        var options = new CardCreationOptions([Owner.Character.CardPool], CardCreationSource.Other, CardRarityOddsType.RegularEncounter);
        var list = new List<Reward>
        {
            new PotionReward(Owner),
            new RelicReward(Owner),
            new CardReward(options, 3, Owner),
            new CardTransformReward(Owner) { Upgrade = false, MaxCards = 1 },
            new CardRemovalReward(Owner),
            new CardUpgradeReward(Owner) { Amount = 1 },
        };
        return list;
    }
}