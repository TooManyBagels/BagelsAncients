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
        new CardsVar(5)
    ];
    
    public override async Task AfterObtained()
    {
        foreach (var original in (await CardSelectCmd.FromDeckForTransformation(Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, DynamicVars.Cards.IntValue))).ToList())
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
            await CardCmd.Transform(original, cardForTransform);
        }
    }
}