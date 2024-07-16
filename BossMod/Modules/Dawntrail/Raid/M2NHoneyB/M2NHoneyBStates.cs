namespace BossMod.Dawntrail.Raid.M2NHoneyB;

class M2NHoneyBStates : StateMachineBuilder
{
    public M2NHoneyBStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<CallMeHoney>()
            .ActivateOnEnter<TemptingTwist3>()
            .ActivateOnEnter<TemptingTwist4>()
            .ActivateOnEnter<HoneyBeeline3>()
            .ActivateOnEnter<HoneyBeeline4>()
            //.ActivateOnEnter<HoneyedBreeze>()
            .ActivateOnEnter<HoneyBLive1>()
            .ActivateOnEnter<Heartsore>()
            .ActivateOnEnter<Fracture>()
            .ActivateOnEnter<Loveseeker2>()
            .ActivateOnEnter<BlowKiss>()
            .ActivateOnEnter<HoneyBFinale>()
            .ActivateOnEnter<DropOfVenom2>()
            .ActivateOnEnter<BlindingLove3>()
            .ActivateOnEnter<BlindingLove4>()
            .ActivateOnEnter<HeartStruck1>()
            .ActivateOnEnter<HeartStruck2>()
            .ActivateOnEnter<HeartStruck3>();
    }
}

