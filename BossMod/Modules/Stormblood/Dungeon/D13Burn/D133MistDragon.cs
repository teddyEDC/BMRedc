namespace BossMod.Stormblood.Dungeon.D13TheBurn.D133MistDragon;

public enum OID : uint
{

    Boss = 0x2431, // R3.0-7.0
    IceVoidzone = 0x1E8713,
    DraconicRegard = 0x2432, // R3.0
    Mist = 0x2433, // R2.16
    Helper2 = 0x2434, // R7.0
    Helper = 0x233C
}

public enum AID : uint
{
    AutoAttack = 870, // Boss->player, no cast, single-target

    RimeWreath = 12619, // Boss->self, 5.0s cast, range 40 circle
    FrostBreath = 12620, // Boss->self, no cast, range 20 90-degree cone
    FogPlumeVisual = 12612, // Boss->self, 3.0s cast, single-target
    FogPlumeCircle = 12613, // Helper->self, 3.0s cast, range 6 circle
    FogPlumeCross = 12614, // Helper->self, 1.5s cast, range 40 width 5 cross
    Vaporize = 12608, // Boss->self, 4.0s cast, single-target
    ColdFogVisual = 12609, // Boss->self, 24.0s cast, range 4 circle
    ColdFog = 12610, // Helper->self, no cast, range 4 circle
    WhiteDeath = 12611, // Helper->player, no cast, single-target
    ChillingAspiration = 12621, // Boss->self, no cast, range 40 width 6 rect
    DeepFog = 12615, // Boss->self, 4.0s cast, range 40 circle
    CauterizeVisual = 12616, // Helper2->self, no cast, range 40 width 16 rect
    Cauterize = 12617, // Helper->self, 0.7s cast, range 40 width 16 rect
    Touchdown = 12618 // Boss->self, 6.0s cast, range 40 circle
}

public enum SID : uint
{
    AreaOfInfluenceUp = 618 // Boss->Boss, extra=0x1/0x2/0x3/0x4/0x5/0x6/0x7/0x8/0x9/0xA/0xB/0xC/0xD/0xE/0xF/0x10
}

public enum IconID : uint
{
    BaitawayRect = 14,
    BaitawayCone = 26
}

class FogPlumeCross(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];
    private static readonly AOEShapeCross cross = new(40, 2.5f);
    private static readonly Angle[] angles = [-0.003f.Degrees(), 44.998f.Degrees()];

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _aoes;

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.FogPlumeCircle)
        {
            foreach (var a in angles)
                _aoes.Add(new(cross, caster.Position, a, Module.CastFinishAt(spell, 3.6f)));
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.FogPlumeCross)
            _aoes.Clear();
    }
}

class FogPlumeCircle(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.FogPlumeCircle), new AOEShapeCircle(6));

class ColdFogGrowth(BossModule module) : Components.GenericAOEs(module)
{
    private AOEInstance? _aoe;

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => Utils.ZeroOrOne(_aoe);

    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID == SID.AreaOfInfluenceUp)
            _aoe = new(new AOEShapeCircle(4 + status.Extra), Arena.Center);
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID == AID.ColdFog)
            _aoe = null;
    }
}

abstract class GenericBaitAwayModule(BossModule module) : Components.GenericBaitAway(module)
{
    protected static readonly AOEShapeCircle circle = new(6);
    protected static readonly AOEShapeCone cone = new(20, 45.Degrees());
    protected static readonly AOEShapeRect rect = new(40, 3);

    protected void DrawPositionsInBounds(WPos[] positions)
    {
        foreach (var position in positions)
            if (Module.InBounds(position))
                circle.Outline(Arena, position);
    }

    protected WPos[] GenerateStandardPositions(Actor boss, Actor target, float baseDistance, int count = 5, float increment = 9)
    {
        var positions = new WPos[count];
        for (var i = 0; i < count; i++)
            positions[i] = CalculatePosition(boss.Position, target, baseDistance + i * increment);
        return positions;
    }

    protected WPos CalculatePosition(WPos bossPos, Actor target, float distance) => bossPos + distance * Module.PrimaryActor.DirectionTo(target);

    protected WPos[] GenerateRotatedPositions(Actor boss, Actor target, float[] angles)
    {
        return
        [
            CalculatePosition(boss.Position, target, 7),
            CalculatePosition(boss.Position, target, 15),
            CalculateRotatedPosition(boss.Position, target, 15, angles[0]),
            CalculateRotatedPosition(boss.Position, target, 15, angles[1])
        ];
    }

    private WPos CalculateRotatedPosition(WPos bossPos, Actor target, float distance, float angleDegrees)
    {
        return bossPos + (distance * Module.PrimaryActor.DirectionTo(target)).Rotate(angleDegrees.Degrees());
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        if (CurrentBaits.Any(x => x.Target == actor))
            hints.Add("Bait away!");
    }

    protected void DrawBaitsOnActor(int pcSlot, Actor pc, Action<Actor> drawAction)
    {
        if (!ActiveBaits.Any(x => x.Target == pc))
            return;

        base.DrawArenaForeground(pcSlot, pc);
        drawAction(pc);
    }

    protected void DrawBaitsOnOther(int pcSlot, Actor pc, Action<Actor> drawAction)
    {
        if (ActiveBaits.Any(x => x.Target == pc))
            return;

        base.DrawArenaBackground(pcSlot, pc);
        foreach (var bait in ActiveBaitsNotOn(pc))
            drawAction(bait.Target);
    }
}

class ChillingAspiration(BossModule module) : GenericBaitAwayModule(module)
{
    public override void OnEventIcon(Actor actor, uint iconID)
    {
        if ((IconID)iconID == IconID.BaitawayRect)
            CurrentBaits.Add(new(Module.PrimaryActor, actor, rect, Module.WorldState.FutureTime(6.1f)));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID == AID.ChillingAspiration)
            CurrentBaits.Clear();
    }

    public override void DrawArenaForeground(int pcSlot, Actor pc)
    {
        DrawBaitsOnActor(pcSlot, pc, target =>
        {
            var boss = Module.PrimaryActor;
            circle.Outline(Arena, boss.Position + boss.HitboxRadius * boss.DirectionTo(target));
            DrawPositionsInBounds(GenerateStandardPositions(boss, target, boss.HitboxRadius));
        });
    }

    public override void DrawArenaBackground(int pcSlot, Actor pc)
    {
        DrawBaitsOnOther(pcSlot, pc, target =>
        {
            var boss = Module.PrimaryActor;
            circle.Draw(Arena, boss.Position + boss.HitboxRadius * boss.DirectionTo(target));
            DrawPositionsInBounds(GenerateStandardPositions(boss, target, boss.HitboxRadius));
        });
    }
}

class FrostBreath(BossModule module) : GenericBaitAwayModule(module)
{
    public override void OnEventIcon(Actor actor, uint iconID)
    {
        if ((IconID)iconID == IconID.BaitawayCone)
            CurrentBaits.Add(new(Module.PrimaryActor, actor, cone, Module.WorldState.FutureTime(4.1f)));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID == AID.FrostBreath)
            CurrentBaits.Clear();
    }

    public override void DrawArenaForeground(int pcSlot, Actor pc)
    {
        DrawBaitsOnActor(pcSlot, pc, target =>
        {
            var positions = GenerateRotatedPositions(Module.PrimaryActor, target, [30, -30]);
            DrawPositionsInBounds(positions);
        });
    }

    public override void DrawArenaBackground(int pcSlot, Actor pc)
    {
        DrawBaitsOnOther(pcSlot, pc, target =>
        {
            var positions = GenerateRotatedPositions(Module.PrimaryActor, target, [30, -30]);
            DrawPositionsInBounds(positions);
        });
    }
}

class RimeWreath(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.RimeWreath));
class TouchDown(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.Touchdown), new AOEShapeCircle(15));
class IceVoidzone(BossModule module) : Components.PersistentVoidzone(module, 6, m => m.Enemies(OID.IceVoidzone).Where(z => z.EventState != 7));

class Cauterize(BossModule module) : Components.GenericAOEs(module)
{
    private AOEInstance? _aoe;
    private static readonly AOEShapeRect rect = new(40, 8);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => Utils.ZeroOrOne(_aoe);

    public override void OnActorCreated(Actor actor)
    {
        if ((OID)actor.OID == OID.Helper2)
            _aoe = new(rect, actor.Position, actor.Rotation, WorldState.FutureTime(7.3f));
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.Cauterize)
            _aoe = null;
    }
}

class D133MistDragonStates : StateMachineBuilder
{
    public D133MistDragonStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<FogPlumeCircle>()
            .ActivateOnEnter<FogPlumeCross>()
            .ActivateOnEnter<ColdFogGrowth>()
            .ActivateOnEnter<ChillingAspiration>()
            .ActivateOnEnter<RimeWreath>()
            .ActivateOnEnter<TouchDown>()
            .ActivateOnEnter<IceVoidzone>()
            .ActivateOnEnter<FrostBreath>()
            .ActivateOnEnter<Cauterize>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "The Combat Reborn Team (Malediktus)", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 585, NameID = 7672)]
public class D133MistDragon(WorldState ws, Actor primary) : BossModule(ws, primary, new(-300, -400), new ArenaBoundsCircle(19.5f))
{
    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actors(Enemies(OID.DraconicRegard).Concat([PrimaryActor]).Concat(Enemies(OID.Mist)));
    }

    protected override void CalculateModuleAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        foreach (var e in hints.PotentialTargets)
        {
            e.Priority = (OID)e.Actor.OID switch
            {
                OID.Mist => 3,
                OID.DraconicRegard => 2,
                OID.Boss => 1,
                _ => 0
            };
        }
    }
}
