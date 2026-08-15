using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using bagelsMod.bagelsModCode.Potions;
using MegaCrit.Sts2.Core.Models;

namespace bagelsMod.bagelsModCode.Relics;

[Pool(typeof(EventRelicPool))]
public class BlessedCauldron : BagelsModRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != this.Owner || room?.RoomType != RoomType.Elite)
            return false;
        rewards.Add(new PotionReward(ModelDb.Potion<MomentOfClarity>().ToMutable(), player));
        return true;
    }
}