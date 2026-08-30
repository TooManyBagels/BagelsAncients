using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace bagelsMod.bagelsModCode.Mnemi.Relics;

[Pool(typeof(EventRelicPool))]
public class Gramophone : BagelsModRelic
{
    private int _cardsDowngrade;

    private int CardsDowngrade
    {
        get => _cardsDowngrade;
        set
        {
            AssertMutable();
            _cardsDowngrade = value;
            InvokeDisplayAmountChanged();
            if (_cardsDowngrade != 0) return;
            Status = RelicStatus.Disabled;
        }
    }
        
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override bool ShowCounter
    {
        get
        {
            if (!CombatManager.Instance.IsInProgress)
                return false;
            return CardsDowngrade > 0;
        }
    }

    public override int DisplayAmount => CardsDowngrade;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
        new CardsVar(3)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Energy)
    ];
    
    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player != Owner ? amount : amount + DynamicVars.Energy.IntValue;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        CardsDowngrade = DynamicVars.Cards.IntValue;
        Status = RelicStatus.Normal;
        return base.AfterPlayerTurnStart(choiceContext, player);
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CardsDowngrade <= 0) return base.AfterCardPlayed(choiceContext, cardPlay);
        cardPlay.Card.EnergyCost.AddThisCombat(1);
        CardsDowngrade--;
        return base.AfterCardPlayed(choiceContext, cardPlay);
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        InvokeDisplayAmountChanged();
        return base.AfterCombatEnd(room);
    }
}