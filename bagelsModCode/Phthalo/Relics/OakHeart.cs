using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class OakHeart : BagelsModRelic
{
    private int _numApplied;
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ("Amount", 15)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>()
    ];

    private int NumApplied
    {
        get => _numApplied;
        set
        {
            AssertMutable();
            _numApplied = value;
        }
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;
        await ModifyPowerIfNecessary();
    }
    
    public override async Task AfterCurrentHpChanged(Creature creature, Decimal _)
    {
        if (!CombatManager.Instance.IsInProgress)
            return;
        await ModifyPowerIfNecessary();
    }
    
    public override Task AfterCombatEnd(CombatRoom _)
    {
        NumApplied = 0;
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }

    private async Task ModifyPowerIfNecessary()
    {
        var creature = Owner.Creature;
        var newValue = (creature.MaxHp - creature.CurrentHp) / DynamicVars["Amount"].IntValue;
        if (NumApplied != newValue)
        {
            var diff = newValue - NumApplied;
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), creature, diff, creature, null);
            await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), creature, diff, creature, null);
            NumApplied = newValue;
        }
    }
}