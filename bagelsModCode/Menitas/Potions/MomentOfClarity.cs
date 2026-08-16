using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace bagelsMod.bagelsModCode.Menitas.Potions;

[Pool(typeof(EventPotionPool))]
public class MomentOfClarity : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Token;
    
    public override PotionUsage Usage => PotionUsage.AnyTime;
    
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("HealPercent", 10)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        NCombatRoom.Instance?.PlaySplashVfx(target, Colors.Red);
        await CreatureCmd.Heal(target, target.MaxHp * DynamicVars["HealPercent"].BaseValue / 100);
        if (!CombatManager.Instance.IsInProgress)
            return;
        foreach (var allCard in Owner.PlayerCombatState.AllCards) 
        {
            if (allCard.IsUpgradable)
                CardCmd.Upgrade(allCard);
        }
    }
}