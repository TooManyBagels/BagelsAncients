public class MENITAS : CustomAncientModel
{
    protected override OptionPools MakeOptionPools => new OptionPools(
        [
            AncientOption<Nunchaku>(),
            AncientOption<Lantern>(),
            AncientOption<ArtOfWar>()
            //more relic options
        ]
    );
    
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
}