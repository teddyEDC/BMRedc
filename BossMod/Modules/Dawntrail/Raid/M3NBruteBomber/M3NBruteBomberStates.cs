namespace BossMod.Dawntrail.Raid.M3NBruteBomber;

class M3NBruteBomberStates : StateMachineBuilder
{
    public M3NBruteBomberStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<KnuckleSandwich>()
            .ActivateOnEnter<BrutalImpact>()
            .ActivateOnEnter<BarbarousBarrageTower>()
            .ActivateOnEnter<BarbarousBarrageKnockback>()
            .ActivateOnEnter<BrutalLariat1>()
            .ActivateOnEnter<BrutalLariat2>()
            .ActivateOnEnter<ExplosiveRainCircle>()
            .ActivateOnEnter<ExplosiveRainConcentric>()
            .ActivateOnEnter<InfernalSpin4>()
            .ActivateOnEnter<SelfDestruct>()
            .ActivateOnEnter<FireSpin4>()
            .ActivateOnEnter<FireSpin5>()
            .ActivateOnEnter<InfernalSpin4>()
            .ActivateOnEnter<InfernalSpin5>()
            .ActivateOnEnter<LariatCombo>()
            .ActivateOnEnter<MurderousMist>();
    }
}
