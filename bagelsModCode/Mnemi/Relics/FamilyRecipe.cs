using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Mnemi.Relics;

[Pool(typeof(EventRelicPool))]
public class FamilyRecipe : BagelsModRelic
{
    private int _timesReplayed;
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ("Replay", 1)
    ];

    public override async Task AfterPotionUsed(PotionModel potion, Creature? target)
    {
        if (potion.Owner != Owner) return;
        if (_timesReplayed == DynamicVars["Replay"].IntValue)
        {
            _timesReplayed = 0;
            return;
        }
        _timesReplayed++;
        await PotionCmd.TryToProcure(potion, Owner);
        await potion.OnUseWrapper(new ThrowingPlayerChoiceContext(), target);
        Flash();
    }
}