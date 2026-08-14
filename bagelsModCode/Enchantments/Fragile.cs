using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace bagelsMod.bagelsModCode.Enchantments;

public class Fragile() : EnchantmentModel
{
    public override bool CanEnchantCardType(CardType cardType) => cardType is CardType.Skill or CardType.Attack;

    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) && !card.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Exhaust) && card.EnergyCost.Canonical > 0;
    }
    
    protected override void OnEnchant()
    {
        this.Card.AddKeyword(CardKeyword.Exhaust);
        Card.EnergyCost.UpgradeBy(-1);
    }
}