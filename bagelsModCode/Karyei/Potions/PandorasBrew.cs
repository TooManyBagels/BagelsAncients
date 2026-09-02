using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.PotionPools;

namespace bagelsMod.bagelsModCode.Karyei.Potions;

[Pool(typeof(EventPotionPool))]
public class PandorasBrew : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Token;

    public override PotionUsage Usage => PotionUsage.CombatOnly;
    
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(2)
    ];
    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        var cardsInHand = PileType.Hand.GetPile(Owner).Cards.ToList();
        foreach (var c in cardsInHand) 
            await CardCmd.TransformToRandom(c, Owner.RunState.Rng.CombatCardSelection);
        cardsInHand = PileType.Hand.GetPile(Owner).Cards.ToList();
        foreach (var c in cardsInHand) 
            if(c.IsUpgradable) CardCmd.Upgrade(c);
    }
}