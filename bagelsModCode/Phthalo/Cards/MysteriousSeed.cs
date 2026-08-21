using bagelsMod.bagelsModCode.Cards;
using bagelsMod.bagelsModCode.Phthalo.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace bagelsMod.bagelsModCode.Phthalo.Cards;

[Pool(typeof(EventCardPool))]
public class MysteriousSeed() : bagelsModCard(1,
    CardType.Quest, CardRarity.Quest,
    TargetType.Self)
{
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
        new MaxHpVar(0),
        new ("GoldIncrement", 40),
        new ("MaxHpIncrement", 4)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, CardKeyword.Exhaust];
    
    public override int MaxUpgradeLevel => 0;

    protected override Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var goldIncrement = DynamicVars["GoldIncrement"].IntValue; 
        var maxHpIncrement = DynamicVars["MaxHpIncrement"].IntValue;
        BuffGold(goldIncrement);
        BuffMaxHp(maxHpIncrement);
        if (DeckVersion is not MysteriousSeed deckVersion)
            return Task.CompletedTask;
        deckVersion.BuffGold(goldIncrement);
        deckVersion.BuffMaxHp(maxHpIncrement);
        return Task.CompletedTask;
    }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        if (room.RoomType is not RoomType.Elite)
        {
            return;
        }
        await RelicCmd.Obtain<MysteriousFruit>(Owner);
    }

    private void BuffGold(int buff)
    {
        Gold += buff;
    }
    
    private void BuffMaxHp(int buff)
    {
        MaxHp += buff;
    }
}