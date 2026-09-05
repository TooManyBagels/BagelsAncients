using bagelsMod.bagelsModCode.Phthalo.RestSiteOptions;
using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class BoilingKettle : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    private int _amount = 1;

    private int _turns;

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

    private int Turns
    {
        get => _turns;
        set
        {
            AssertMutable();
            _turns = value;
            InvokeDisplayAmountChanged();
        }
    }

    public override bool ShowCounter => Amount - Turns > 0;

    public override int DisplayAmount => Amount - Turns > 0 ? Amount - Turns : 0;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ("BoilLevel", Amount),
        new CardsVar(1),
        new EnergyVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Energy)
    ];

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Turns++;
        if (Turns == Amount) Status = RelicStatus.Disabled;
        return base.AfterPlayerTurnStart(choiceContext, player);
    }

    public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
    {
        return player != Owner || Turns > Amount - 1 ? amount : amount + DynamicVars.Energy.IntValue;
    }

    public override Decimal ModifyHandDraw(Player player, Decimal count)
    {
        return player != Owner || Turns > Amount - 1 ? count : count + DynamicVars.Cards.IntValue;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        Turns = 0;
        Status = RelicStatus.Normal;
        return base.AfterCombatEnd(room);
    }

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != Owner)
            return false;
        options.Add(new BoilRestSiteAction(player));
        return true;
    }
    
    public void Boil()
    {
        Amount++;
        Flash();
    }
}