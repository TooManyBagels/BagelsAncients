using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace bagelsMod.bagelsModCode.Menitas.Relics;

[Pool(typeof(EventRelicPool))]
public class GremlinSatchel : BagelsModRelic
{
    private int _turnNum;
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(80),
        new ("GoldReduction", 10)
    ];

    public override Task BeforeCombatStart()
    {
        _turnNum = 0;
        return base.BeforeCombatStart();
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        _turnNum++;
        return base.AfterPlayerTurnStart(choiceContext, player);
    }

    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if(player != Owner || room == null || !room.RoomType.IsCombatRoom() || room.RoomType == RoomType.Boss && player.RunState.CurrentActIndex >= player.RunState.Acts.Count - 1)
            return false;
        var goldGiven =  DynamicVars.Gold.IntValue-_turnNum*DynamicVars["GoldReduction"].IntValue;
        rewards.Add(new GoldReward(goldGiven, player));
        return true;
    }
}