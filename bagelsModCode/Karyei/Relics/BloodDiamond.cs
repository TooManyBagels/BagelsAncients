using bagelsMod.bagelsModCode.Karyei.Enchantments;
using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Karyei.Relics;

[Pool(typeof(EventRelicPool))]
public class BloodDiamond : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        ..HoverTipFactory.FromEnchantment<BloodPact>(),
        HoverTipFactory.Static(StaticHoverTip.Energy),
    ];
    
    public override async Task AfterObtained()
    {
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0,DynamicVars.Cards.IntValue)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        var canonicalEnchantment = ModelDb.Enchantment<BloodPact>();
        foreach (var card in await CardSelectCmd.FromDeckForEnchantment(Owner, canonicalEnchantment, 1, prefs))
        {
            CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, 1);
            CardCmd.Preview(card);
        }
    }
}