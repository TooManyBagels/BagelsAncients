using bagelsMod.bagelsModCode.Enchantments;
using bagelsMod.bagelsModCode.Karyei.Enchantments;
using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace bagelsMod.bagelsModCode.Karyei.Relics;

[Pool(typeof(EventRelicPool))]
public class BloodDiamond() : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        var prefs = new CardSelectorPrefs(new LocString("relics", Id.Entry + ".prompt"), 0, PileType.Deck.GetPile(Owner).Cards.Count)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        var canonicalEnchantment = ModelDb.Enchantment<BloodPact>();
        foreach (var card in await CardSelectCmd.FromDeckForEnchantment(Owner, canonicalEnchantment, 1, prefs))
        {
            CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, 1);
            CardCmd.Preview(card, 1f, CardPreviewStyle.MessyLayout);
        }
    }
}