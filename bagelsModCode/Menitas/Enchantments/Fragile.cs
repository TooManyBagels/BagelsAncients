using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace bagelsMod.bagelsModCode.Menitas.Enchantments;

public class Fragile : CustomEnchantmentModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override bool CanEnchantCardType(CardType cardType) => cardType is CardType.Skill or CardType.Attack;

    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) && !card.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Exhaust) && card.EnergyCost.Canonical > 0;
    }
    
    protected override void OnEnchant()
    {
        Card.AddKeyword(CardKeyword.Exhaust);
        Card.EnergyCost.UpgradeBy(-1);
    }
}