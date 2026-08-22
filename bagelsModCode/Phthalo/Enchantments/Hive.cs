using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace bagelsMod.bagelsModCode.Phthalo.Enchantments;

public class Hive : CustomEnchantmentModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if(card.Enchantment is not Hive)
            return;
        IList<CardModel> hiveCards = PileType.Draw.GetPile(card.Owner).Cards.Where(c => c.Enchantment is Hive).ToList();
        foreach (var c in hiveCards)
        {
            await CardPileCmd.Add(c, PileType.Hand);
        }
    }
}