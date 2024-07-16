namespace BossMod.Dawntrail.Raid.M4NWickedThunder;

class M4NWickedThunderStates : StateMachineBuilder
{
    public M4NWickedThunderStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<WrathOfZeus>()
            .ActivateOnEnter<SidewiseSpark1>()
            .ActivateOnEnter<SidewiseSpark2>()
            .ActivateOnEnter<SidewiseSpark3>()
            .ActivateOnEnter<SidewiseSpark4>()
            .ActivateOnEnter<SidewiseSpark5>()
            .ActivateOnEnter<SidewiseSpark6>()
            .ActivateOnEnter<StampedingThunder3>()
            .ActivateOnEnter<Burst>()
            .ActivateOnEnter<BewitchingFlight3>()
            .ActivateOnEnter<Thunderslam>()
            .ActivateOnEnter<UnknownWeaponskill7>();
    }
}
