using bagelsMod.bagelsModCode.Phthalo.Cards;
using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs.History;

namespace bagelsMod.bagelsModCode.Phthalo.Relics;

[Pool(typeof(EventRelicPool))]
public class EncasingEmber : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        List<CardModel> questList =
        [
            ModelDb.Card<MysteriousSeed>(),
            ModelDb.Card<PreservedVines>(),
            ModelDb.Card<EndlessTrove>()
        ];
        
        var reward = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), questList, Owner);
        switch (reward)
        {
            case MysteriousSeed: 
                CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<MysteriousSeed>(Owner), Owner.Deck));
                break;
            case PreservedVines:
                CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<PreservedVines>(Owner), Owner.Deck));
                break;
            case EndlessTrove:
                CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner.RunState.CreateCard<EndlessTrove>(Owner), Owner.Deck));
                break;
        }
        
        foreach(var c in questList)
        {
            if (c != reward)
                Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(Owner.NetId).CardChoices.Add(new CardChoiceHistoryEntry(c, false));
        }
    }
}