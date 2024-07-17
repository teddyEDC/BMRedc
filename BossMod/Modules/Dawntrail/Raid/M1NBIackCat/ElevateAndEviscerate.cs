namespace BossMod.Dawntrail.Raid.M1NBlackCat;

class ElevateAndEvisverate(BossModule module) : Components.Knockback(module)
{
    private DateTime activation;
    private (Actor source, Actor target) _tether;

    public override IEnumerable<Source> Sources(int slot, Actor actor)
    {
        if (_tether != default && actor == _tether.target)
            yield return new(_tether.source.Position, 10, activation);
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.ElevateAndEviscerate)
            activation = spell.NPCFinishAt;
    }

    public override void OnTethered(Actor source, ActorTetherInfo tether)
    {
        if (tether.ID is (uint)TetherID.ElevateAndEviscerateGood or (uint)TetherID.ElevateAndEviscerateBad)
            _tether = (source, WorldState.Actors.Find(tether.Target)!);
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.ElevateAndEviscerate)
        {
            _tether = default;
            ++NumCasts;
        }
    }
}
