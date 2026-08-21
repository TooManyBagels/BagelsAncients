using bagelsMod.bagelsModCode.Phthalo.Relics;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace bagelsMod.bagelsModCode.Phthalo.Ancients;

public class Phthalo : CustomAncientModel
{
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3;
    }

    protected override OptionPools MakeOptionPools => new OptionPools(
        [
            AncientOption<EncasingEmber>(),
            AncientOption<OddlySmoothStone>(),
            AncientOption<DataDisk>(),
        ]
    );
    
    public override string CustomScenePath => "res://bagelsMod/scenes/bagelsmod-menitas.tscn";
    public override string CustomMapIconPath => "res://bagelsMod/images/ancients/menitas.png";
    public override string CustomMapIconOutlinePath => "res://bagelsMod/images/ancients/menitas_outline.png";
    public override string CustomRunHistoryIconPath => "res://bagelsMod/images/ancients/menitas-pfp.png";
    public override string CustomRunHistoryIconOutlinePath => "res://bagelsMod/images/ancients/menitas-pfp_outline.png";
}