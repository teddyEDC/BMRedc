namespace BossMod.Dawntrail.Alliance.A11Prishe;

class Explosion(BossModule module) : Components.GenericAOEs(module, ActionID.MakeSpell(AID.Explosion))
{
    private static readonly AOEShapeCircle circle = new(8f);
    private readonly List<AOEInstance> _aoes = new(28);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        var count = _aoes.Count;
        if (count == 0)
            return [];
        var firstactivation = _aoes[0].Activation;
        var aoes = new AOEInstance[count];
        var color = Colors.Danger;
        for (var i = 0; i < count; ++i)
        {
            var aoe = _aoes[i];
            aoes[i] = (aoe.Activation - firstactivation).TotalSeconds < 1d ? aoe with { Color = color } : aoe with { Risky = false };
        }
        return aoes;
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if (spell.Action.ID == (uint)AID.Explosion)
            _aoes.Add(new(circle, spell.LocXZ, default, Module.CastFinishAt(spell)));
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (_aoes.Count != 0 && spell.Action.ID == (uint)AID.Explosion)
            _aoes.RemoveAt(0);
    }
}
