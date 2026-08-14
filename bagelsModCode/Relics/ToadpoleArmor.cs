using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace bagelsMod.bagelsModCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class ToadpoleArmor() : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ThornsPower>(5), new PowerVar<PlatingPower>(5)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<PlatingPower>(),
        HoverTipFactory.FromPower<ThornsPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;
        this.Flash();
        await PowerCmd.Apply<ThornsPower>(
            (PlayerChoiceContext)new ThrowingPlayerChoiceContext(), this.Owner.Creature,
            this.DynamicVars["ThornsPower"].BaseValue, this.Owner.Creature, null);
        await PowerCmd.Apply<PlatingPower>(
            (PlayerChoiceContext)new ThrowingPlayerChoiceContext(), this.Owner.Creature,
            this.DynamicVars["PlatingPower"].BaseValue, this.Owner.Creature, null);
    }
}