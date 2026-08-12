using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return;
        this.Flash();
        ThornsPower thornsPower = await PowerCmd.Apply<ThornsPower>(
            (PlayerChoiceContext)new ThrowingPlayerChoiceContext(), this.Owner.Creature,
            this.DynamicVars["ThornsPower"].BaseValue, this.Owner.Creature, (CardModel)null);
        PlatingPower platingPower = await PowerCmd.Apply<PlatingPower>(
            (PlayerChoiceContext)new ThrowingPlayerChoiceContext(), this.Owner.Creature,
            this.DynamicVars["PlatingPower"].BaseValue, this.Owner.Creature, (CardModel)null);
    }
}