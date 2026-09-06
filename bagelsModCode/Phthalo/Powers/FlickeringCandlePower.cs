using bagelsMod.bagelsModCode.Templates;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace bagelsMod.bagelsModCode.Phthalo.Powers;

public class FlickeringCandlePower : BagelsModPower
{
    public override PowerType Type =>
        PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Energy)
    ];
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player && player.MaxEnergy != 0)
            await PlayerCmd.LoseEnergy(player.PlayerCombatState.MaxEnergy, Owner.Player);
    }
    
    public override Decimal ModifyEnergyGain(Player player, Decimal amount)
    {
        if (!CombatManager.Instance.IsInProgress || amount < 0)
            return amount;
        Flash();
        return player != Owner.Player ? amount : 0;
    }

    public override bool ShouldPlayerResetEnergy(Player player)
    {
        return player != Owner.Player;
    }
    
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Enemy)
            return;
        await PowerCmd.Decrement(this);
    }
}