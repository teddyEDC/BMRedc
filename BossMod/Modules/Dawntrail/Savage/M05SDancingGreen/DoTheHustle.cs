namespace BossMod.Dawntrail.Savage.M05SDancingGreen;

class DoTheHustle(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeCone cone = new(50f, 90f.Degrees());
    private readonly List<AOEInstance> _aoes = new(4);

    public override ReadOnlySpan<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        var count = _aoes.Count;
        if (count == 0)
            return [];
        var aoes = CollectionsMarshal.AsSpan(_aoes);
        var deadline = aoes[0].Activation.AddSeconds(1d);

        var index = 0;
        while (index < count && aoes[index].Activation < deadline)
            ++index;

        return aoes[..index];
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        switch (spell.Action.ID)
        {
            case (uint)AID.DoTheHustle1:
            case (uint)AID.DoTheHustle2:
            case (uint)AID.DoTheHustle3:
            case (uint)AID.DoTheHustle4:
                _aoes.Add(new(cone, spell.LocXZ, spell.Rotation, Module.CastFinishAt(spell)));
                break;
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        var count = _aoes.Count;
        if (count != 0)
            switch (spell.Action.ID)
            {
                case (uint)AID.DoTheHustle1:
                case (uint)AID.DoTheHustle2:
                case (uint)AID.DoTheHustle3:
                case (uint)AID.DoTheHustle4:
                    _aoes.RemoveAt(0);
                    ++NumCasts;
                    break;
            }
    }
}
