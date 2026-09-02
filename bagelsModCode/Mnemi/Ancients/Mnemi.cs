using bagelsMod.bagelsModCode.Mnemi.Relics;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;

namespace bagelsMod.bagelsModCode.Mnemi.Ancients;

[Pool(typeof(AncientEventModel))]
public class Mnemi : CustomAncientModel
{
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3;
    }
    
    protected override OptionPools MakeOptionPools => new OptionPools(
        [
            AncientOption<BigHouse>(),
            AncientOption<FriendshipBracelet>(),
            AncientOption<HeartLocket>(),
        ],
        [
            AncientOption<FamilyRecipe>(),
            AncientOption<Photograph>(),
            AncientOption<ReflectingPool>(),
            AncientOption<RoseColoredGlasses>()
        ],
        [
            AncientOption<SaltShaker>(),
            AncientOption<Gramophone>(),
            AncientOption<Scrapbook>(),
        ]
    );

    public override string CustomScenePath => "res://bagelsMod/scenes/bagelsmod-menitas.tscn";
    public override string CustomMapIconPath => "res://bagelsMod/images/ancients/menitas.png";
    public override string CustomMapIconOutlinePath => "res://bagelsMod/images/ancients/menitas_outline.png";
    public override string CustomRunHistoryIconPath => "res://bagelsMod/images/ancients/menitas-pfp.png";
    public override string CustomRunHistoryIconOutlinePath => "res://bagelsMod/images/ancients/menitas-pfp_outline.png";
}