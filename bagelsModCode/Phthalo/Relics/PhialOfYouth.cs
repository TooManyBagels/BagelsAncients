using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class PhialOfYouth : BagelsModRelic
{
    private int _turnNum;
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(7),
        new HealVar(7)
    ];
    
    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        _turnNum++;
        return base.AfterPlayerTurnStart(choiceContext, player);
    }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        var maxHpGain = DynamicVars.MaxHp.IntValue - _turnNum;
        if(maxHpGain > 0) await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.IntValue - _turnNum);
        var healGain = DynamicVars.Heal.IntValue - _turnNum;
        if(healGain > 0) await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.IntValue - _turnNum);
    }
}