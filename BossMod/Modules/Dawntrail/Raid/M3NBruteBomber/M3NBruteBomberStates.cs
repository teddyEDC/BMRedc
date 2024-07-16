namespace BossMod.Dawntrail.Raid.M3NBruteBomber;

class M3NBruteBomberStates : StateMachineBuilder
{
    public M3NBruteBomberStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<BrutalImpact1>()
            //.ActivateOnEnter<BrutalLariat3>()
            //.ActivateOnEnter<BrutalLariat4>()
            .ActivateOnEnter<ExplosiveRain4>()
            .ActivateOnEnter<ExplosiveRain>()
            .ActivateOnEnter<InfernalSpin4>()
            .ActivateOnEnter<SelfDestruct>()
            .ActivateOnEnter<FireSpin4>()
            .ActivateOnEnter<FireSpin5>()
            .ActivateOnEnter<InfernalSpin4>()
            .ActivateOnEnter<InfernalSpin5>()
            //.ActivateOnEnter<LariatCombo9>()
            //.ActivateOnEnter<LariatCombo10>()
            //.ActivateOnEnter<LariatCombo11>()
            //.ActivateOnEnter<LariatCombo12>()
            .ActivateOnEnter<MurderousMist>();
    }
}
