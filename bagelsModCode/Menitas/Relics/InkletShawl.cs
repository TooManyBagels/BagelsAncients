using bagelsMod.bagelsModCode.Menitas.Cards;
using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Menitas.Relics;

[Pool(typeof(EventRelicPool))]
public class InkletShawl : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        var card = Owner.RunState.CreateCard<Slip>(Owner);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(card, PileType.Deck)], 2f);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        ..HoverTipFactory.FromCardWithCardHoverTips<Slip>(),
        HoverTipFactory.FromPower<SlipperyPower>(),
        HoverTipFactory.Static(StaticHoverTip.Energy)
    ];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if(!this.Owner.Creature.HasPower<SlipperyPower>())
        {
            return;
        }
        this.Flash();
        await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
    }
}