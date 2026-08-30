using bagelsMod.bagelsModCode.Templates;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace bagelsMod.bagelsModCode.Mnemi.Relics;

//! Currently does not work, crashes on the shuffle (Bug: Instantiating the new cards wrong?)

[Pool(typeof(EventRelicPool))]
public class ReflectingPool : BagelsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    
    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner || Owner.PlayerCombatState.TurnNumber != 1)
            return;
        Flash();
        var list = PileType.Deck.GetPile(Owner).Cards.Where(c => c.Rarity is not (CardRarity.Basic or CardRarity.Curse)).ToList();
        foreach (var c in list)
            await CardPileCmd.AddGeneratedCardToCombat(combatState.CloneCard(c), PileType.Draw, Owner, CardPilePosition.Random);
    }
}