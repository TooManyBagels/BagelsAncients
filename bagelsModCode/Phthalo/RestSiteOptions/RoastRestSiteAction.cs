using bagelsMod.bagelsModCode.Phthalo.Relics;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;

namespace bagelsMod.bagelsModCode.Phthalo.RestSiteOptions;

public class RoastRestSiteAction(Player owner) : CustomRestSiteOption(owner)
{
    public override string OptionId => "Roast";
    
    public override Task<bool> OnSelect()
    {
        Owner.GetRelic<RoastedChestnuts>()?.Roast();
        return Task.FromResult(true);
    }
}