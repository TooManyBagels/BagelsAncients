using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;

namespace bagelsMod.bagelsModCode.Karyei.Relics;

[Pool(typeof(EventRelicPool))]
public class VolatileCrystal : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    public override async Task AfterObtained()
    {
        var relicRewards = new List<Reward>();
        for (var i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            relicRewards.Add(new RelicReward(Owner));
        }
        
        await Cmd.CustomScaledWait(0.1f, 0.2f);

        await RewardsCmd.OfferCustom(Owner, relicRewards);
        var removeCards = PileType.Deck.GetPile(Owner).Cards.Where((c => c.IsRemovable)).ToList().StableShuffle(Owner.RunState.Rng.Niche).Take(DynamicVars.Cards.IntValue);
        foreach (var card in removeCards) await CardPileCmd.RemoveFromDeck(card);
        
        await Cmd.CustomScaledWait(1f, 2f);
        
        var upgradeCards = PileType.Deck.GetPile(Owner).Cards.Where((c => c.IsUpgradable)).ToList().StableShuffle(Owner.RunState.Rng.Niche).Take(DynamicVars.Cards.IntValue);
        foreach (var card in upgradeCards) CardCmd.Upgrade(card, CardPreviewStyle.GridLayout);
    }
}