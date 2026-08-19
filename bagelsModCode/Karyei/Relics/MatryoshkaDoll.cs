using bagelsMod.bagelsModCode.Karyei.Cards;
using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace bagelsMod.bagelsModCode.Karyei.Relics;

[Pool(typeof(EventRelicPool))]
public class MatryoshkaDoll : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<Obsession>();

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        var card = Owner.RunState.CreateCard<Obsession>(Owner);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(card, PileType.Deck)], 2f);
    }

    public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        var rewardsTemp = new List<Reward>(rewards);

        if (player != Owner || room == null || !room.RoomType.IsCombatRoom() ||room.RoomType == RoomType.Boss && player.RunState.CurrentActIndex >= player.RunState.Acts.Count - 1)
            return false;
        foreach (var reward in rewardsTemp)
        {
            switch (reward)
            {
                case PotionReward: 
                    rewards.Add(new PotionReward(player));
                    break;
                case RelicReward:
                    rewards.Add(new RelicReward(player));
                    break;
                case CardReward:
                    rewards.Add(new CardReward(CardCreationOptions.ForRoom(player, room.RoomType), 3, player));
                    break;
                default:
                    rewards.Add(reward);
                    break;
            }
        }
        return true;
    }
    
    public override Task AfterModifyingRewards()
    {
        this.Flash();
        return Task.CompletedTask;
    }
}