using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.PotionPools;

namespace bagelsMod.bagelsModCode.Karyei.Potions;

[Pool(typeof(EventPotionPool))]
public class PandorasBrew : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Token;
    
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    
    public override TargetType TargetType => TargetType.AnyPlayer;
    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        var cardsInHand = PileType.Hand.GetPile(Owner).Cards.ToList();
        foreach (var c in cardsInHand) 
            await CardCmd.TransformToRandom(c, Owner.RunState.Rng.CombatCardSelection);
        cardsInHand = PileType.Hand.GetPile(Owner).Cards.ToList();
        foreach (var c in cardsInHand) 
            if(c.IsUpgradable) CardCmd.Upgrade(c);
    }
}