using bagelsMod.bagelsModCode.Phthalo.RestSiteOptions;
using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class RoastedChestnuts : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    private int _amount = 1;

    [SavedProperty]
    private int Amount
    {
        get => _amount;
        set {
            AssertMutable();
            _amount = value;
            InvokeDisplayAmountChanged();
        }
    }
    
    public override bool ShowCounter => true;

    public override int DisplayAmount => Amount;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ("RoastLevel", Amount),
        new CardsVar(1),
        new EnergyVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.ForEnergy(this)
    ];
    
    public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
    {
        if(CombatManager.Instance.IsInProgress && Owner.PlayerCombatState.TurnNumber <= Amount)
            return player != Owner ? amount : amount + DynamicVars.Energy.IntValue;
        return amount;
    }

    public override Decimal ModifyHandDraw(Player player, Decimal count)
    {
        if(CombatManager.Instance.IsInProgress && Owner.PlayerCombatState.TurnNumber <= Amount)
            return player != Owner ? count : count + DynamicVars.Energy.IntValue;
        return count;
    }

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != Owner)
            return false;
        options.Add(new RoastRestSiteAction(player));
        return true;
    }
    
    public void Roast()
    {
        Amount++;
        Flash();
    }
}