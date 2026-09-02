using bagelsMod.bagelsModCode.Classes;
using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;

namespace bagelsMod.bagelsModCode.Mnemi.Relics;

[Pool(typeof(EventRelicPool))]
public class HeartLocket : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override async Task AfterObtained()
    {
        var relics = Owner.Relics;
        var list = new List<Reward>();
        foreach (var relic in relics)
            if (relic.Rarity is not (RelicRarity.Ancient or RelicRarity.Starter)) 
                list.Add(new RelicTradeReward(relic, Owner));
        await RewardsCmd.OfferCustom(Owner, list);
    }
}