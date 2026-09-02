using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;

namespace bagelsMod.bagelsModCode.Classes;

public class RelicTradeReward : RelicReward
{
    private readonly RelicModel _relic;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => _relic.HoverTips;
    
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
    
    [PreserveBaseOverrides]
    public override TextureRect CreateIcon()
    {
        TextureRect texture = new TextureRect();
        texture.Texture = _relic.BigIcon;
        texture.Material = (Material) PreloadManager.Cache.GetMaterial("res://materials/ui/relic_mat.tres").Duplicate(true);
        _relic.UpdateTexture(texture);
        texture.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        texture.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        return texture;
    }
}