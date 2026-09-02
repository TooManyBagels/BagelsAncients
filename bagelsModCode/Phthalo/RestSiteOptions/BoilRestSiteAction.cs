using bagelsMod.bagelsModCode.Phthalo.Relics;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;

namespace bagelsMod.bagelsModCode.Phthalo.RestSiteOptions;

public class BoilRestSiteAction(Player owner) : CustomRestSiteOption(owner)
{
    public override string OptionId => "BOIL";
    
    public override Task<bool> OnSelect()
    {
        Owner.GetRelic<BoilingKettle>()?.Boil();
        return Task.FromResult(true);
    }
}