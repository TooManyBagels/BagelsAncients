using bagelsMod.bagelsModCode.Mnemi.Cards;
using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Mnemi.Relics;

[Pool(typeof(EventRelicPool))]
public class RoseColoredGlasses : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromCardWithCardHoverTips<TotalRecall>();

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        var card = Owner.RunState.CreateCard<TotalRecall>(Owner);
        CardCmd.PreviewCardPileAdd([await CardPileCmd.Add(card, PileType.Deck)], 2f);
    }
}