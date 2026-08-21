using bagelsMod.bagelsModCode.Phthalo.Cards;
using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class ShamblingVines : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;

    private int _thorns;

    private int _plating;

    private int Thorns
    {
        get => _thorns;
        set
        {
            AssertMutable();
            _thorns = value;
            DynamicVars["ThornsPower"].BaseValue = _thorns;
        }
    }

    private int Plating
    {
        get => _plating;
        set
        {
            AssertMutable();
            _plating = value;
            DynamicVars["PlatingPower"].BaseValue = _plating;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ThornsPower>(Thorns),
        new PowerVar<PlatingPower>(Plating),
    ];

    public override Task AfterObtained()
    {
        var list = PileType.Deck.GetPile(Owner).Cards.Where(c => c is PreservedVines).ToList();
        foreach (var card in list)
        {
            Thorns += card.DynamicVars["ThornsPower"].IntValue;
            Plating += card.DynamicVars["PlatingPower"].IntValue;
            CardPileCmd.RemoveFromDeck(card);
        }
        return base.AfterObtained();
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;
        Flash();
        await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["ThornsPower"].IntValue, Owner.Creature, null);
        await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["PlatingPower"].IntValue, Owner.Creature, null);
    } 
}