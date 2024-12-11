namespace BossMod.Dawntrail.Alliance.A11Prishe;

class AsuranFists(BossModule module) : Components.GenericTowers(module)
{
    public override void OnActorEAnim(Actor actor, uint state)
    {
        if ((OID)actor.OID == OID.Tower && state == 0x00100020)
        {
            Towers.Add(new(actor.Position, 6, PartyState.MaxAllianceSize, PartyState.MaxAllianceSize, activation: WorldState.FutureTime(6)));
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID is AID.AsuranFists1 or AID.AsuranFists2 or AID.AsuranFists3)
        {
            ++NumCasts;
            if ((AID)spell.Action.ID == AID.AsuranFists3)
                Towers.Clear();
        }
    }
}
