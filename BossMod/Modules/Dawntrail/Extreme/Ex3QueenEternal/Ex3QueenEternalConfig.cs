namespace BossMod.Dawntrail.Extreme.Ex3QueenEternal;

[ConfigDisplay(Order = 0x030, Parent = typeof(DawntrailConfig))]
class Ex3QueenEternalConfig() : ConfigNode()
{
    [PropertyDisplay("Absolute Authority: ignore flares, stack together")]
    public bool AbsoluteAuthorityIgnoreFlares = true;

    [PropertyDisplay("Fixed bridge tether spots", tooltip: "Side tethers stretch without crossing is typically used on EU/NA partyfinder, the other option is usually used by JP")]
    [PropertyCombo("Side tethers stretch without crossing", "Side tethers stretch cross (JP)")]
    public bool SideTethersNoCrossing = true;
}
