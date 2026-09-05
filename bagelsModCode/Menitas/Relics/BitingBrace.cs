using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace bagelsMod.bagelsModCode.Menitas.Relics;

[Pool(typeof(EventRelicPool))]
public class BitingBrace : BagelsModRelic
{
    private int _hits;

    private bool _isActivating;
        
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override bool ShowCounter => true;

    public override int DisplayAmount => !_isActivating ? Hits : DynamicVars["Hits"].IntValue;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(1),
        new MaxHpVar(3),
        new ("Hits", 2)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Energy)
    ];
    
    [SavedProperty]
    private int Hits
    {
        get => _hits;
        set
        {
            AssertMutable();
            _hits = value;
            InvokeDisplayAmountChanged();
        }
    }

    public bool IsActivating
    {
        get => _isActivating;
        set
        {
            AssertMutable();
            _isActivating = value;
            InvokeDisplayAmountChanged();
        }
    }
    
    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player != Owner ? amount : amount + DynamicVars.Energy.IntValue;
    }
    
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress || target != Owner.Creature || result.UnblockedDamage <= 0)
            return;
        Hits++;
        if (Hits == DynamicVars["Hits"].BaseValue)
        {
            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.IntValue, false);
            await TaskHelper.RunSafely(DoActivateVisuals());
            Hits = 0;
        }
    }
    
    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.CustomScaledWait(0.2f, 0.4f);
        IsActivating = false;
    }
}