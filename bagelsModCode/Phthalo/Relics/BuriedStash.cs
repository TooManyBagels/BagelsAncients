using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class BuriedStash : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ("Turns", 3),
        new EnergyVar(12)
    ];

    public override async Task BeforeCombatStart()
    {
        await PlayerCmd.SetEnergy(DynamicVars.Energy.IntValue, Owner);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner && player.MaxEnergy != 0 &&
            player.PlayerCombatState.TurnNumber <= DynamicVars["Turns"].IntValue)
            await PlayerCmd.LoseEnergy(player.PlayerCombatState.MaxEnergy, Owner);
    }
    
    public override Decimal ModifyEnergyGain(Player player, Decimal amount)
    {
        if (!CombatManager.Instance.IsInProgress || Owner.PlayerCombatState.TurnNumber > DynamicVars["Turns"].IntValue || amount == DynamicVars.Energy.IntValue || amount < 0)
            return amount;
        Flash();
        return player != Owner ? amount : 0;
    }

    public override bool ShouldPlayerResetEnergy(Player player)
    {
        return player != Owner || Owner.PlayerCombatState.TurnNumber > DynamicVars["Turns"].IntValue;
    }
}