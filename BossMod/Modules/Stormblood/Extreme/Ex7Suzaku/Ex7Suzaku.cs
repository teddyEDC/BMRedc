namespace BossMod.Stormblood.Extreme.Ex7Suzaku;

class Cremate(BossModule module) : Components.SingleTargetCast(module, ActionID.MakeSpell(AID.Cremate));
class ScreamsOfTheDamned(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.ScreamsOfTheDamned));
class AshesToAshes(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.AshesToAshes));
class ScarletFever(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.ScarletFever));
class SouthronStar(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.SouthronStar));
class Rout(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.Rout), new AOEShapeRect(55, 3));
class RekindleSpread(BossModule module) : Components.SpreadFromIcon(module, (uint)IconID.Spreadmarker, ActionID.MakeSpell(AID.RekindleSpread), 6, 5.1f)
{
    private bool _firstSpread = true;

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        base.OnEventCast(caster, spell);
        if ((AID)spell.Action.ID == AID.RekindleSpread)
            _firstSpread = false;
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        base.AddAIHints(slot, actor, assignment, hints);
        foreach (var scarletLady in Module.Enemies(OID.ScarletLady))
        {
            if (_firstSpread && ActiveStacks.Count != 0 && scarletLady.IsDead)
                hints.AddForbiddenZone(ShapeDistance.Circle(scarletLady.Position, 6));
        }
    }

    public override void DrawArenaBackground(int pcSlot, Actor pc)
    {
        base.DrawArenaBackground(pcSlot, pc);
        foreach (var scarletLady in Module.Enemies(OID.ScarletLady))
        {
            if (_firstSpread && ActiveStacks.Count != 0 && scarletLady.IsDead)
                Arena.AddCircle(scarletLady.Position, 6, Colors.Vulnerable);
        }
    }
}

class FleetingSummer(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.FleetingSummer), new AOEShapeCone(40, 45.Degrees()));

// class WingAndAPrayerTailFeather(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.WingAndAPrayerTailFeather), 9);
// class WingAndAPrayerPlume(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.WingAndAPrayerPlume), 9);

class MesmerizingMelody(BossModule module) : Components.SimpleKnockbacks(module, ActionID.MakeSpell(AID.MesmerizingMelody), KnockbackDistance, kind: Kind.TowardsOrigin)
{
    public const int KnockbackDistance = 11;
    public const float SafeDistance = Ex7Suzaku.OuterRadius - Ex7Suzaku.InnerRadius - KnockbackDistance - 0.5f;

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        var source = Casters.Count != 0 ? Casters[0] : null;
        if (source != null)
            hints.AddForbiddenZone(ShapeDistance.InvertedDonutSector(source.Position, Ex7Suzaku.OuterRadius - SafeDistance, Ex7Suzaku.OuterRadius, default, 180f.Degrees()), Module.CastFinishAt(source.CastInfo));
    }
}

class RuthlessRefrain(BossModule module) : Components.SimpleKnockbacks(module, ActionID.MakeSpell(AID.RuthlessRefrain), MesmerizingMelody.KnockbackDistance, kind: Kind.AwayFromOrigin)
{
    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        var source = Casters.Count != 0 ? Casters[0] : null;
        if (source != null)
            hints.AddForbiddenZone(ShapeDistance.InvertedDonutSector(source.Position, Ex7Suzaku.InnerRadius, Ex7Suzaku.InnerRadius + MesmerizingMelody.SafeDistance, default, 180f.Degrees()), Module.CastFinishAt(source.CastInfo));
    }
}

class PayThePiper(BossModule module) : BossComponent(module)
{
    private bool _isLoomingCrescendo;
    private Direction _marchDirection = Direction.Unset;

    public override void OnTethered(Actor source, ActorTetherInfo tether)
    {
        var target = WorldState.Actors.Find(tether.Target)!;
        if (target != Module.Raid.Player() || (TetherID)tether.ID != TetherID.PayThePiper)
            return;
        switch (source.OID)
        {
            case (uint)OID.NorthernPyre:
                _marchDirection = Direction.North;
                break;
            case (uint)OID.EasternPyre:
                _marchDirection = Direction.East;
                break;
            case (uint)OID.SouthernPyre:
                _marchDirection = Direction.East;
                break;
            case (uint)OID.WesternPyre:
                _marchDirection = Direction.West;
                break;
        }
    }

    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if (actor != Module.Raid.Player())
            return;
        switch (status.ID)
        {
            case (uint)SID.LoomingCrescendo:
                _isLoomingCrescendo = true;
                break;
            case (uint)SID.PayingThePiper:
                _isLoomingCrescendo = false;
                break;
        }
    }

    public override void OnStatusLose(Actor actor, ActorStatus status)
    {
        if (actor != Module.Raid.Player())
            return;
        if (status.ID == (uint)SID.LoomingCrescendo)
            _isLoomingCrescendo = false;
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (_isLoomingCrescendo)
        {
            var safeCenter = new WPos();
            var safeOffset = Ex7Suzaku.OuterRadius + Ex7Suzaku.InnerRadius + 0.5f;
            var rotation = 0.Degrees();

            switch (_marchDirection)
            {
                case Direction.Unset:
                    return;
                case Direction.North:
                    safeCenter = new(Ex7Suzaku.ArenaCenter.X, Ex7Suzaku.ArenaCenter.Z + safeOffset);
                    rotation = 90.Degrees();
                    break;
                case Direction.East:
                    safeCenter = new(Ex7Suzaku.ArenaCenter.X - safeOffset, Ex7Suzaku.ArenaCenter.Z);
                    rotation = default;
                    break;
                case Direction.South:
                    safeCenter = new(Ex7Suzaku.ArenaCenter.X, Ex7Suzaku.ArenaCenter.Z - safeOffset);
                    rotation = 90.Degrees();
                    break;
                case Direction.West:
                    safeCenter = new(Ex7Suzaku.ArenaCenter.X + safeOffset, Ex7Suzaku.ArenaCenter.Z);
                    rotation = default;
                    break;
            }

            hints.AddForbiddenZone(ShapeDistance.Rect(Ex7Suzaku.ArenaCenter, rotation, Ex7Suzaku.InnerRadius + 0.5f, Ex7Suzaku.InnerRadius + 0.5f, Ex7Suzaku.OuterRadius));
            hints.AddForbiddenZone(ShapeDistance.InvertedCircle(safeCenter, Ex7Suzaku.OuterRadius));
        }
    }
}
class WellOfFlame(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.WellOfFlame), new AOEShapeRect(41f, 10f));
class ScathingNetStack(BossModule module) : Components.StackWithIcon(module, (uint)IconID.Stackmarker, ActionID.MakeSpell(AID.ScathingNetStack), 6f, 5.1f, 8);
class PhantomFlurryCombo(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.PhantomFlurryCombo), new AOEShapeCone(41f, 90f.Degrees()));
class PhantomFlurryKnockback(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.PhantomFlurryKnockback), new AOEShapeCone(41f, 90f.Degrees()));
class Hotspot(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.Hotspot), new AOEShapeCone(21f, 45f.Degrees()));

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "Kismet", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 597, NameID = 7702)]
public class Ex7Suzaku(WorldState ws, Actor primary) : BossModule(ws, primary, ArenaCenter, Phase1Bounds)
{
    public const int InnerRadius = 4;
    public const int OuterRadius = 20;
    public static readonly WPos ArenaCenter = new(100, 100);
    public static readonly ArenaBoundsComplex Phase1Bounds = new([new Polygon(ArenaCenter, 19.5f, 80)]);
    public static readonly ArenaBoundsComplex Phase2Bounds = new([new DonutV(ArenaCenter, 3.5f, 20f, 80)]);

    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actor(PrimaryActor);
        Arena.Actors(Enemies((uint)OID.ScarletLady), Colors.Vulnerable);
    }
}
