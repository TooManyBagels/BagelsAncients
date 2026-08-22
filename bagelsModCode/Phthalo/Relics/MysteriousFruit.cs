using bagelsMod.bagelsModCode.Phthalo.Cards;
using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class MysteriousFruit : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;
    
    private int _gold;
    private int _maxHp;

    [SavedProperty]
    private int Gold
    {
        get => _gold;
        set
        {
            AssertMutable();
            _gold = value;
            DynamicVars.Gold.BaseValue = _gold;
        }
    }
    
    [SavedProperty]
    private int MaxHp
    {
        get => _maxHp;
        set
        {
            AssertMutable();
            _maxHp = value;
            DynamicVars.MaxHp.BaseValue = _maxHp;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new GoldVar(0),
        new MaxHpVar(0)
    ];

    public override async Task AfterObtained()
    {
        var list = PileType.Deck.GetPile(Owner).Cards.Where(c => c is MysteriousSeed).ToList();
        foreach (var card in list)
        {
            Gold += card.DynamicVars.Gold.IntValue;
            MaxHp += card.DynamicVars.MaxHp.IntValue;
            await CardPileCmd.RemoveFromDeck(card);
        }
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
    }
}