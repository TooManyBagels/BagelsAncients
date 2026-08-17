using bagelsMod.bagelsModCode.Karyei.Relics;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;

namespace bagelsMod.bagelsModCode.Karyei.Ancients;

public class Karyei : CustomAncientModel
{
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }

    protected override OptionPools MakeOptionPools => new OptionPools(
        [
            AncientOption<BlightedMushroom>(),
            AncientOption<DoublePendulum>(),
            AncientOption<MechanicalButterfly>(),
            AncientOption<SensoryStone>()
        ]
    );
    
    public override string CustomScenePath => "res://bagelsMod/scenes/bagelsmod-menitas.tscn";
    public override string CustomMapIconPath => "res://bagelsMod/images/ancients/menitas.png";
    public override string CustomMapIconOutlinePath => "res://bagelsMod/images/ancients/menitas_outline.png";
    public override string CustomRunHistoryIconPath => "res://bagelsMod/images/ancients/menitas-pfp.png";
    public override string CustomRunHistoryIconOutlinePath => "res://bagelsMod/images/ancients/menitas-pfp_outline.png";
}