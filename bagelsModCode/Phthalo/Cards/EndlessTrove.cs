using bagelsMod.bagelsModCode.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;

namespace bagelsMod.bagelsModCode.Phthalo.Cards;

[Pool(typeof(EventCardPool))]
public class EndlessTrove() : bagelsModCard(2,
    CardType.Quest, CardRarity.Quest,
    TargetType.Self)
{
    private int _relics;

    private int Relics
    {
        get => _relics;
        set
        {
            AssertMutable();
            _relics = value;
            DynamicVars["Relics"].BaseValue = _relics;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ("Relics", Relics)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, CardKeyword.Exhaust];

    public override int MaxUpgradeLevel => 0;

    protected override Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        AddRelic();
        if (DeckVersion is not EndlessTrove deckVersion)
            return Task.CompletedTask;
        deckVersion.AddRelic();
        return Task.CompletedTask;
    }

    private void AddRelic()
    {
        Relics++;
    }

    public override async Task BeforeCardRemoved(CardModel card)
    {
        if (card != this) return;
        var relicList = new List<Reward>();
        for (var i = 0; i < DynamicVars["Relics"].IntValue; i++)
        {       
            relicList.Add(new RelicReward(Owner));
        }
        await RewardsCmd.OfferCustom(Owner, relicList);
    }
}