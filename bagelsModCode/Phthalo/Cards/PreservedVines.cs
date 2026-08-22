using bagelsMod.bagelsModCode.Templates;
using bagelsMod.bagelsModCode.Phthalo.RestSiteOptions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace bagelsMod.bagelsModCode.Phthalo.Cards;

[Pool(typeof(EventCardPool))]
public class PreservedVines() : BagelsModCard(1,
    CardType.Quest, CardRarity.Quest,
    TargetType.Self)
{
    private int _thorns;
    private int _plating;

    [SavedProperty]
    public int Thorns
    {
        get => _thorns;
        private set
        {
            AssertMutable();
            _thorns = value;
            DynamicVars["ThornsPower"].BaseValue = _thorns;
        }
    }
    
    [SavedProperty]
    public int Plating
    {
        get => _plating;
        private set
        {
            AssertMutable();
            _plating = value;
            DynamicVars["PlatingPower"].BaseValue = _plating;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ThornsPower>("ThornsPower", Thorns),
        new PowerVar<PlatingPower>("PlatingPower", Plating),
        new ("ThornsIncrement", 2),
        new ("PlatingIncrement", 1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, CardKeyword.Exhaust];
    
    public override int MaxUpgradeLevel => 0;

    protected override Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var thornsIncrement = DynamicVars["ThornsIncrement"].IntValue; 
        var platingIncrement = DynamicVars["PlatingIncrement"].IntValue;
        BuffThorns(thornsIncrement);
        BuffPlating(platingIncrement);
        if (DeckVersion is not PreservedVines deckVersion)
            return Task.CompletedTask;
        deckVersion.BuffThorns(thornsIncrement);
        deckVersion.BuffPlating(platingIncrement);
        return Task.CompletedTask;
    }
    
    private void BuffPlating(int buff)
    {
        Plating += buff;
    }
    
    private void BuffThorns(int buff)
    {
        Thorns += buff;
    }
    
    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != Owner)
            return false;
        options.Add(new DissolveRestSiteAction(player));
        return true;
    }
}