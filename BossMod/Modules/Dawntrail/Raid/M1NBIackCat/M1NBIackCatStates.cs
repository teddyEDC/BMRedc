namespace BossMod.Dawntrail.Raid.M1NBlackCat;

class M1NBlackCatStates : StateMachineBuilder
{
    public M1NBlackCatStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<ArenaChanges>()
            .ActivateOnEnter<ElevateAndEviscerate>()
            .ActivateOnEnter<ElevateAndEviscerateHint>()
            .ActivateOnEnter<BloodyScratch>()
            .ActivateOnEnter<Mouser>()
            .ActivateOnEnter<OneTwoPaw>()
            .ActivateOnEnter<BlackCatCrossing>()
            .ActivateOnEnter<BiscuitMaker>()
            .ActivateOnEnter<Clawful>()
            .ActivateOnEnter<Shockwave>()
            .ActivateOnEnter<PredaceousPounce>()
            .ActivateOnEnter<GrimalkinGale2>()
            .ActivateOnEnter<Overshadow>();
    }
}
