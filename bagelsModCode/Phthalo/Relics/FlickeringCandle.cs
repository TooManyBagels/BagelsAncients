using bagelsMod.bagelsModCode.Phthalo.Powers;
using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class FlickeringCandle : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ("Turns", 3),
        new EnergyVar(12)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Energy)
    ];

    public override async Task BeforeCombatStart()
    {
        await PlayerCmd.SetEnergy(DynamicVars.Energy.IntValue, Owner);
        await PowerCmd.Apply<FlickeringCandlePower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
            DynamicVars["Turns"].IntValue, Owner.Creature, null);
    }
}