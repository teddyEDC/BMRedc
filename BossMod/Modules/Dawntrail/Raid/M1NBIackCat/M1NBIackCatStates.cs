namespace BossMod.Dawntrail.Raid.M1NBlackCat;

class M1NBlackCatStates : StateMachineBuilder
{
    public M1NBlackCatStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<BlackCatCrossing3>()
            .ActivateOnEnter<BlackCatCrossing4>()
            .ActivateOnEnter<BloodyScratch>()
            .ActivateOnEnter<OneTwoPaw>()
            .ActivateOnEnter<BlackCatCrossing>()
            .ActivateOnEnter<BiscuitMaker>()
            .ActivateOnEnter<Clawful2>()
            .ActivateOnEnter<Shockwave2>()
            .ActivateOnEnter<PredaceousPounce2>()
            .ActivateOnEnter<PredaceousPounce3>()
            .ActivateOnEnter<PredaceousPounce5>()
            .ActivateOnEnter<PredaceousPounce6>()
            .ActivateOnEnter<GrimalkinGale2>()
            .ActivateOnEnter<LeapingOneTwoPaw>()
            .ActivateOnEnter<LeapingBlackCatCrossing>()
            .ActivateOnEnter<Overshadow>();
    }
}
