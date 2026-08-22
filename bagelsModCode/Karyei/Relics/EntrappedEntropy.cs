using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Karyei.Relics;

[Pool(typeof(EventRelicPool))]
public class EntrappedEntropy : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPotion<EntropicBrew>()];
    
    public override Task AfterObtained()
    {
        var potionSlots = Owner.PotionSlots.Count;
        for(var i = 0; i < potionSlots; i++) PotionCmd.TryToProcure<EntropicBrew>(Owner);
        return Task.CompletedTask;
    }

    public override async Task AfterPotionUsed(PotionModel potion, Creature? target)
    {
        if (potion.Owner != Owner || !CombatManager.Instance.IsInProgress)
            return;
        Flash();
        var cardsInHand = PileType.Hand.GetPile(Owner).Cards.ToList();
        foreach (var c in cardsInHand) await CardCmd.TransformToRandom(c, Owner.RunState.Rng.CombatCardSelection);
    }
}