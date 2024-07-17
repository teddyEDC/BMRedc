namespace BossMod.Dawntrail.Raid.M1NBlackCat;

class Mouser(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];
    private static readonly AOEShapeRect rect = new(5, 5, 5);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_aoes.Count > 0)
        {
            var aoeCount = Math.Clamp(_aoes.Count, 0, NumCasts > 2 ? 2 : 3);
            for (var i = aoeCount; i < _aoes.Count; i++)
                yield return _aoes[i];
            for (var i = 0; i < aoeCount; i++)
                yield return _aoes[i] with { Color = ArenaColor.Danger };
        }
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.MouserTelegraphFirst or AID.MouserTelegraphSecond)
            _aoes.Add(new(rect, caster.Position, spell.Rotation, Module.WorldState.FutureTime(9.7f)));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (_aoes.Count > 0 && (AID)spell.Action.ID == AID.Mouser)
        {
            _aoes.RemoveAt(0);
            if (++NumCasts == 19)
                NumCasts = 0;
        }
    }
}
