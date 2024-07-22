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
            .ActivateOnEnter<FireSpin>()
            .ActivateOnEnter<LitFuse>()
            .ActivateOnEnter<LariatCombo>()
            .ActivateOnEnter<BrutalBurn>()
            .ActivateOnEnter<MurderousMist>();
    }
}
