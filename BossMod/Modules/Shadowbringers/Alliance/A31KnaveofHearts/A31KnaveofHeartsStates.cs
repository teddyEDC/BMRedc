namespace BossMod.Shadowbringers.Alliance.A31KnaveofHearts;

class A31KnaveofHeartsStates : StateMachineBuilder
{
    public A31KnaveofHeartsStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<Roar>()
            .ActivateOnEnter<ColossalImpactLeft>()
            .ActivateOnEnter<ColossalImpactRight>()
            .ActivateOnEnter<ColossalImpactMiddle>()
            .ActivateOnEnter<ColossalImpact6>()
            .ActivateOnEnter<ColossalImpact7>()
            .ActivateOnEnter<ColossalImpact8>()
            .ActivateOnEnter<MagicArtilleryBeta2>()
            .ActivateOnEnter<MagicArtilleryAlpha2>()
            .ActivateOnEnter<Energy>()
            .ActivateOnEnter<LightLeap2>()
            .ActivateOnEnter<BoxSpawn>()
            .ActivateOnEnter<MagicBarrage>()
            .ActivateOnEnter<Lunge>();
    }
}
