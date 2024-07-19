namespace BossMod.Shadowbringers.Alliance.A32HanselGretel;

class A32HanselGretelStates : StateMachineBuilder
{
    public A32HanselGretelStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<Wail1>()
            .ActivateOnEnter<Wail2>()
            .ActivateOnEnter<CripplingBlow1>()
            .ActivateOnEnter<CripplingBlow2>()
            .ActivateOnEnter<BloodySweep3>()
            .ActivateOnEnter<BloodySweep4>()
            .ActivateOnEnter<BloodySweep7>()
            .ActivateOnEnter<BloodySweep8>()
            .ActivateOnEnter<SeedOfMagicAlpha2>()
            .ActivateOnEnter<RiotOfMagic2>()
            .ActivateOnEnter<PassingLance3>()
            .ActivateOnEnter<Explosion>()
            .ActivateOnEnter<UnevenFooting>()
            .ActivateOnEnter<HungryLance1>()
            .ActivateOnEnter<HungryLance2>()
            .ActivateOnEnter<Breakthrough1>()
            .ActivateOnEnter<SeedOfMagicBeta3>()
            .ActivateOnEnter<Lamentation>();
    }
}
