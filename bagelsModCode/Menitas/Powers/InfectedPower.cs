using bagelsMod.bagelsModCode.Menitas.Relics;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Powers;

namespace bagelsMod.bagelsModCode.Menitas.Powers;

public class InfectedPower : CustomTemporaryPowerModelWrapper<InfectedScythe, StrengthPower>
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
}