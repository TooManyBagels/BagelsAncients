using bagelsMod.bagelsModCode.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace bagelsMod.bagelsModCode.Karyei.Cards;

[Pool(typeof(CurseCardPool))]
public class Obsession() : bagelsModCard(-1,
    CardType.Curse, CardRarity.Curse,
    TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override int MaxUpgradeLevel => 0;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Eternal, CardKeyword.Unplayable];
    
    public override bool HasTurnEndInHandEffect => true;
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card != this)
            return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CreateClone(), PileType.Draw, Owner));
    }
}