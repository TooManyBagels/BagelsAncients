using bagelsMod.bagelsModCode.Cards;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace bagelsMod.bagelsModCode.Menitas.Cards;

[Pool(typeof(EventCardPool))]
public class Slip() : bagelsModCard(1,
    CardType.Power, CardRarity.Ancient,
    TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<SlipperyPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature,null);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}