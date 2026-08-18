using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace bagelsMod.bagelsModCode.Karyei.Enchantments;

public class BloodPact() : CustomEnchantmentModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1), new HpLossVar(0)];

    public override bool HasExtraCardText => true;

    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) && card.EnergyCost.Canonical > 0;
    }
    
    protected override void OnEnchant()
    {
        DynamicVars.HpLoss.BaseValue = Card.EnergyCost.Canonical;
        Card.EnergyCost.UpgradeBy(-Card.EnergyCost.Canonical);
    }
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await CreatureCmd.Damage(choiceContext, Card.Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Card, cardPlay);
    }
}