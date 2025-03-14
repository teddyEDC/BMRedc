namespace BossMod.Stormblood.Extreme.Ex7Suzaku;

class Ex7SuzakuStates : StateMachineBuilder
{
    public Ex7SuzakuStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<Cremate>()
            .ActivateOnEnter<ScreamsOfTheDamned>()
            .ActivateOnEnter<AshesToAshes>()
            .ActivateOnEnter<ScarletFever>()
            .ActivateOnEnter<ScarletFeverArenaChange>()
            .ActivateOnEnter<SouthronStar>()
            .ActivateOnEnter<Rout>()
            .ActivateOnEnter<RekindleSpread>()
            .ActivateOnEnter<FleetingSummer>()
            .ActivateOnEnter<RapturousEcho>()
            .ActivateOnEnter<RapturousEchoTowers>()
            .ActivateOnEnter<ScarletMelody>()
            // .ActivateOnEnter<WingAndAPrayerTailFeather>()
            // .ActivateOnEnter<WingAndAPrayerPlume>()
            .ActivateOnEnter<MesmerizingMelody>()
            .ActivateOnEnter<RuthlessRefrain>()
            .ActivateOnEnter<PayThePiper>()
            .ActivateOnEnter<WellOfFlame>()
            .ActivateOnEnter<ScathingNetStack>()
            .ActivateOnEnter<PhantomFlurryCombo>()
            .ActivateOnEnter<PhantomFlurryKnockback>()
            .ActivateOnEnter<Hotspot>();
    }
}