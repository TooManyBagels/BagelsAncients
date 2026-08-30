using BaseLib.Abstracts;

namespace bagelsMod.bagelsModCode.Mnemi.Enchantments;

public class Copied : CustomEnchantmentModel
{
    public override bool ShouldStartAtBottomOfDrawPile => true;

    protected override void OnEnchant()
    {
        Card.BaseReplayCount++;
    }
}