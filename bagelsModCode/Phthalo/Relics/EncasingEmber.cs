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
            Owner.RunState.CreateCard<MysteriousSeed>(Owner),
            Owner.RunState.CreateCard<PreservedVines>(Owner),
            Owner.RunState.CreateCard<EndlessTrove>(Owner)
        ];
        
        var reward = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), questList, Owner);
        foreach (var c in questList)
        {
            if(c == reward) CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(c, Owner.Deck));
            else Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(Owner.NetId).CardChoices.Add(new CardChoiceHistoryEntry(c, false));
        }
    }
}