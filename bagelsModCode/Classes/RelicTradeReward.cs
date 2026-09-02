using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;

namespace bagelsMod.bagelsModCode.Classes;

public class RelicTradeReward : RelicReward
{
    private readonly RelicModel _relic;
    
    public override LocString Description
    {
        get
        {
            var description = new LocString("card_reward_ui", "BAGELSMOD-TRADE.title");
            description.Add("trade", _relic.Title);
            return description;
        }
    }

    public RelicTradeReward(RelicModel relicToTrade, Player player) : base(player)
    {
        _relic = relicToTrade;
    }

    protected override async Task<bool> OnSelect()
    {
        var relicReceived =  RelicFactory.PullNextRelicFromFront(Player).ToMutable();
        Log.Info($"Player {Player.NetId} traded {_relic.Id} for {relicReceived.Id} from relic reward");
        await RelicCmd.Replace(_relic, relicReceived);
        return true;
    }
    
    public override void OnSkipped()
    { 
    }
}