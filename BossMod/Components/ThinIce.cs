namespace BossMod.Components;

// component for ThinIce mechanic
// observation: for SID 911 the distance is 0.1 * status extra
public abstract class ThinIce(BossModule module, bool createforbiddenzones, uint statusID = 911, float distance = 11, bool stopAtWall = false, bool stopAfterWall = false) : Knockback(module, stopAtWall: stopAtWall, stopAfterWall: stopAfterWall)
{
    public readonly uint StatusID = statusID;
    public readonly float Distance = distance;
    private static readonly WDir offset = new(0, 1);

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

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (createforbiddenzones && actor.FindStatus(StatusID) != null)
        {
            var pos = actor.Position;
            var ddistance = 2 * Distance;
            var forbidden = new List<Func<WPos, float>>
            {
                ShapeDistance.InvertedDonut(pos, Distance, Distance + 0.5f),
                ShapeDistance.InvertedDonut(pos, ddistance, ddistance + 0.5f),
                ShapeDistance.InvertedRect(pos, offset, 0.5f, 0.5f, 0.5f)
            };
            hints.AddForbiddenZone(p => forbidden.Max(f => f(p)), WorldState.FutureTime(1.1f));
        }
    }
}
