using bagelsMod.bagelsModCode.Karyei.Cards;
using bagelsMod.bagelsModCode.Relics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Karyei.Relics;

[Pool(typeof(EventRelicPool))]
public class ImbalancedScales : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<AllOut>();

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        var card = Owner.RunState.CreateCard<AllOut>(Owner);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(card, PileType.Deck)], 2f);
    }
}