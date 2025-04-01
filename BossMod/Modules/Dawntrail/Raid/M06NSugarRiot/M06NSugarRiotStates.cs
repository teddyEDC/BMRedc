namespace BossMod.Dawntrail.Raid.M06SugarRiot;

class M06SugarRiotStates : StateMachineBuilder
{
    public M06SugarRiotStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<ArenaChanges>()
            .ActivateOnEnter<SingleDoubleStyle>()
            .ActivateOnEnter<MousseMural>()
            .ActivateOnEnter<SprayPain>()
            .ActivateOnEnter<WarmBomb>()
            .ActivateOnEnter<CoolBomb>()
            .ActivateOnEnter<PuddingParty>()
            .ActivateOnEnter<MousseTouchUp>()
            .ActivateOnEnter<LightningBolt>()
            .ActivateOnEnter<TasteOfFire>()
            .ActivateOnEnter<TasteOfThunder>()
            .ActivateOnEnter<Quicksand>()
            .ActivateOnEnter<Highlightning>();
    }
}
