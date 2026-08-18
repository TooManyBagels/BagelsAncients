using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace bagelsMod.bagelsModCode.Menitas.Relics;

[Pool(typeof(EventRelicPool))]
public class PhantasmalArmor : BagelsModRelic
{
    private bool _triggeredThisTurn;

    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7, ValueProp.Unpowered)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<BlurPower>()
    ];

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (Owner.Creature.CombatState == null || Owner.Creature.CombatState.CurrentSide == Owner.Creature.Side || target != Owner.Creature || result.UnblockedDamage <= 0 || _triggeredThisTurn)
            return; 
        _triggeredThisTurn = true;
        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
        await PowerCmd.Apply<BlurPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature, null);
    }
    
    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
            return Task.CompletedTask;
        _triggeredThisTurn = false;
        return Task.CompletedTask;
    }
}