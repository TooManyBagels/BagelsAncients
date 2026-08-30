using bagelsMod.bagelsModCode.Menitas.Enchantments;
using bagelsMod.bagelsModCode.Mnemi.Enchantments;
using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Mnemi.Relics;

[Pool(typeof(EventRelicPool))]
public class Photograph : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        ..HoverTipFactory.FromEnchantment<Copied>(),
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic),
    ];
    
    public override bool HasUponPickupEffect => true;
    
    public override async Task AfterObtained()
    {
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, DynamicVars.Cards.IntValue)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        var canonicalEnchantment = ModelDb.Enchantment<Copied>();
        foreach (var card in await CardSelectCmd.FromDeckForEnchantment(Owner, canonicalEnchantment, 1, prefs))
        {
            CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, 1);
            CardCmd.Preview(card);
        }
    }
    
}