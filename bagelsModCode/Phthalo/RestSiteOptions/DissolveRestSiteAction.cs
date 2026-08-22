using bagelsMod.bagelsModCode.Phthalo.Relics;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace bagelsMod.bagelsModCode.Phthalo.RestSiteOptions;

public class DissolveRestSiteAction(Player owner) : CustomRestSiteOption(owner)
{
    public override string OptionId => "DISSOLVE";
    
    public override async Task<bool> OnSelect()
    {
        await RelicCmd.Obtain<ShamblingVines>(Owner);
        return true;
    }
}