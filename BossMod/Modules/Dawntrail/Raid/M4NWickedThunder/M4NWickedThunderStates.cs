namespace BossMod.Dawntrail.Raid.M4NWickedThunder;

class M4NWickedThunderStates : StateMachineBuilder
{
    public M4NWickedThunderStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<ArenaChanges>()
            .ActivateOnEnter<WickedJolt>()
            .ActivateOnEnter<WickedBolt>()
            .ActivateOnEnter<WickedCannon>()
            .ActivateOnEnter<WrathOfZeus>()
            .ActivateOnEnter<SidewiseSpark>()
            .ActivateOnEnter<SoaringSoulpress>()
            .ActivateOnEnter<StampedingThunder>()
            .ActivateOnEnter<Burst>()
            .ActivateOnEnter<BewitchingFlight>()
            .ActivateOnEnter<Thunderslam>()
            .ActivateOnEnter<Thunderstorm>()
            .ActivateOnEnter<WickedHypercannon>()
            .ActivateOnEnter<WitchHunt>();
    }
}
