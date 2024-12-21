namespace BossMod.Components;

abstract class ThinIce(BossModule module, uint statusID = 911, float distance = 11, bool stopAtWall = false, bool stopAfterWall = false) : Knockback(module, stopAtWall: stopAtWall, stopAfterWall: stopAfterWall)
{
    public readonly uint StatusID = statusID;
    public readonly float Distance = distance;

    public override IEnumerable<Source> Sources(int slot, Actor actor)
    {
        if (actor.FindStatus(StatusID) != null)
            yield return new(actor.Position, Distance, default, default, actor.Rotation, Kind.DirForward);
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        if (CalculateMovements(slot, actor).Any(e => DestinationUnsafe(slot, actor, e.to)))
            hints.Add("You are risking to slide into danger!");
    }

    public override void DrawArenaForeground(int pcSlot, Actor pc)
    {
        base.DrawArenaForeground(pcSlot, pc);
        if (pc.FindStatus(StatusID) != null)
            Arena.AddCircle(pc.Position, Distance, Colors.Vulnerable);
    }
}
