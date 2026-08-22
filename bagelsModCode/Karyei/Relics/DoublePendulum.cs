using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace bagelsMod.bagelsModCode.Karyei.Relics;

[Pool(typeof(EventRelicPool))]
public class DoublePendulum : BagelsModRelic
{
    private bool _isActivating;
    
    private int _turnsSeen;
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override bool ShowCounter => true;
    
    public override int DisplayAmount => !IsActivating ? TurnsSeen : DynamicVars["Turns"].IntValue;

    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [new DynamicVar("Turns", 2), 
            new CardsVar(3),
            new DynamicVar("DrawDown", 1)
        ];

    private bool IsActivating
    {
        get => _isActivating;
        set
        {
            AssertMutable();
            _isActivating = value;
            UpdateDisplay();
        }
    }

    private int TurnsSeen
    {
        get => _turnsSeen;
        set
        {
            AssertMutable();
            _turnsSeen = value;
            UpdateDisplay();
        }
    }
    
    private void UpdateDisplay()
    {
        if (IsActivating)
            Status = RelicStatus.Normal;
        else
            Status = TurnsSeen == DynamicVars["Turns"].IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
        InvokeDisplayAmountChanged();
    }

    public override Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != this.Owner)
            return Task.CompletedTask;
        TurnsSeen=(TurnsSeen+1)%DynamicVars["Turns"].IntValue;
        Status = TurnsSeen == DynamicVars["Turns"].IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
        return Task.CompletedTask;
    }
    
    public override Task AfterCombatEnd(CombatRoom _)
    {
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }

    public override Decimal ModifyHandDraw(Player player, Decimal count)
    {
        return player != Owner || TurnsSeen != 0 ? count-DynamicVars["DrawDown"].IntValue : count + DynamicVars.Cards.BaseValue;
    }
    
    public override Task AfterModifyingHandDraw()
    {
        if (TurnsSeen == 0)
        {
            TaskHelper.RunSafely(DoActivateVisuals());
        }
        return Task.CompletedTask;
    }
    
    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }
}