using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class AxolotlTail : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    private bool _wasUsed;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(25)
    ];

    private bool WasUsed
    {
        get => _wasUsed;
        set
        {
            AssertMutable();
            _wasUsed = value;
            if (!_wasUsed)
                return;
            Status = RelicStatus.Disabled;
        }
    }

    public override Task BeforeCombatStart()
    {
        WasUsed = false;
        Status = RelicStatus.Active;

        return base.BeforeCombatStart();
    }
    

    public override bool ShouldDieLate(Creature creature)
    {
        return creature != Owner.Creature || WasUsed;
    }
    
    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        WasUsed = true;
        await CreatureCmd.Heal(creature, Math.Max(1, creature.MaxHp * (DynamicVars.Heal.BaseValue / 100)));
    }
}