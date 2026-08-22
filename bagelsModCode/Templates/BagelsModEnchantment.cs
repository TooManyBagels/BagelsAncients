using bagelsMod.bagelsModCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;

namespace bagelsMod.bagelsModCode.Templates;

public abstract class BagelsModEnchantment : CustomEnchantmentModel
{
    protected override string CustomIconPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentImagePath();
}