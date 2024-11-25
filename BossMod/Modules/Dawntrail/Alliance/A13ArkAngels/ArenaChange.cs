namespace BossMod.Dawntrail.Alliance.A13ArkAngels;

class ArenaChange(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeDonut donut = new(25, 30);
    private AOEInstance? _aoe;

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => Utils.ZeroOrOne(_aoe);
    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.DecisiveBattleMR)
            _aoe = new(donut, Arena.Center, default, Module.CastFinishAt(spell, 3.4f));
    }

    public override void OnEventEnvControl(byte index, uint state)
    {
        if (state == 0x00200001 && index == 0x1F)
        {
            Arena.Bounds = A13ArkAngels.DefaultBounds;
            _aoe = null;
        }
    }
}
