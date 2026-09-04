using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;

namespace bagelsMod.bagelsModCode.Mnemi.Relics;

[Pool(typeof(EventRelicPool))]
public class FriendshipBracelet : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(8)
    ];
    
    public override async Task AfterObtained()
    {
        var cards = await CardSelectCmd.FromDeckGeneric(Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 0, DynamicVars.Cards.IntValue), c => !c.CanonicalKeywords.Contains(CardKeyword.Eternal));
        NRun.Instance?.GlobalUi.GridCardPreviewContainer.ForceMaxColumnsUntilEmpty(4);
        foreach (var original in cards)
        {
            IEnumerable<CardModel> options;
            switch (original.Rarity)
            {
                case CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare or CardRarity.Curse:
                    options = CardCreationOptions.ForNonCombatWithUniformOdds([original.Pool], c => c.Rarity == original.Rarity && c.Id != original.Id).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications).GetPossibleCards(Owner);
                    break;
                case CardRarity.Basic:
                    options = Owner.Character.CardPool.AllCards
                        .Where(c => c.Rarity is CardRarity.Basic && c.Id != original.Id)
                        .TakeRandom(1, Owner.RunState.Rng.Niche);
                    break;
                default:
                    options = CardCreationOptions.ForNonCombatWithUniformOdds([ModelDb.CardPool<ColorlessCardPool>()], c => c.Id != original.Id).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications).GetPossibleCards(Owner);
                    break;
            }
            var cardForTransform = original.Rarity is CardRarity.Basic ? Owner.RunState.CreateCard(options.First(), Owner) : CardFactory.CreateRandomCardForTransform(original, options, false, Owner.RunState.Rng.Niche);
            CardCmd.Upgrade(cardForTransform);
            await CardCmd.Transform(original, cardForTransform, CardPreviewStyle.GridLayout);
        }
    }
}