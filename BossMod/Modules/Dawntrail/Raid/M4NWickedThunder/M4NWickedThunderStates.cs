namespace BossMod.Dawntrail.Raid.M4NWickedThunder;

class M4NWickedThunderStates : StateMachineBuilder
{
    public M4NWickedThunderStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<ArenaChanges>()
            .ActivateOnEnter<WickedHypercannon>()
            .ActivateOnEnter<WickedJolt>()
            .ActivateOnEnter<WickedBolt>()
            .ActivateOnEnter<WickedCannon>()
            .ActivateOnEnter<WrathOfZeus>()
            .ActivateOnEnter<SidewiseSpark>()
            .ActivateOnEnter<SoaringSoulpress>()
            .ActivateOnEnter<StampedingThunder>()
            .ActivateOnEnter<BewitchingFlight>()
            .ActivateOnEnter<Thunderslam>()
            .ActivateOnEnter<Thunderstorm>()
            .ActivateOnEnter<WitchHunt>();
    }
}
