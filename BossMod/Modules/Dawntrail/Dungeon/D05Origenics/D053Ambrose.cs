namespace BossMod.Dawntrail.Dungeon.D05Origenics.D053Ambrose;

public enum OID : uint
{
    Boss = 0x417D, // R4.998
    Electrolance = 0x4180, // R1.38
    Superfluity = 0x417F, // R1.8
    OrigenicsEyeborg = 0x417E, // R4.0
    Helper = 0x233C
}

public enum AID : uint
{
    AutoAttack = 870, // Boss->player, no cast, single-target
    AutoAttack2 = 872, // Superfluity/OrigenicsEyeborg->player, no cast, single-target
    Teleport = 36439, // Boss->location, no cast, single-target

    PsychicWave = 36436, // Boss->self, 5.0s cast, range 80 circle

    OverwhelmingChargeVisual = 36435, // Boss->self, no cast, single-target
    OverwhelmingCharge1 = 39233, // Boss->self, 5.0s cast, range 26 180-degree cone
    OverwhelmingCharge2 = 39072, // Helper->self, 9.8s cast, range 26 180-degree cone

    PsychokinesisVisual1 = 36427, // Boss->self, 10.0s cast, single-target
    PsychokinesisVisual2 = 38929, // Boss->self, 8.0s cast, single-target
    Psychokinesis = 36428, // Helper->self, 10.0s cast, range 70 width 13 rect

    ExtrasensoryField = 36432, // Boss->self, 7.0s cast, single-target
    ExtrasensoryExpulsionWestEast = 36434, // Helper->self, 7.0s cast, range 15 width 20 rect
    ExtrasensoryExpulsionNorthSouth = 36433, // Helper->self, 7.0s cast, range 20 width 15 rect

    VoltaicSlash = 36437, // Boss->player, 5.0s cast, single-target
    PsychokineticCharge = 39055, // Boss->self, 7.0s cast, single-target

    Electrolance = 36429, // Boss->location, 6.0s cast, range 22 circle

    RushTelegraph = 38953, // Helper->location, 2.5s cast, width 10 rect charge
    Rush = 38954, // Electrolance->location, no cast, width 10 rect charge
    ElectrolanceAssimilationVisual = 36430, // Boss->self, 0.5s cast, single-target
    ElectrolanceAssimilation = 36431, // Helper->self, 1.0s cast, range 33 width 10 rect

    WhorlOfTheMind = 36438, // Helper->player, 5.0s cast, range 5 circle
}

class PsychicWaveArenaChange(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeCustom square = new([new Rectangle(D053Ambrose.ArenaCenter, 33, 24)], [new Rectangle(D053Ambrose.ArenaCenter, 15, 19.5f)]);
    private AOEInstance? _aoe;

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => Utils.ZeroOrOne(_aoe);
    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.PsychicWave && Module.Arena.Bounds == D053Ambrose.StartingBounds)
            _aoe = new(square, Module.Center, default, spell.NPCFinishAt.AddSeconds(0.7f));
    }

    public override void OnEventEnvControl(byte index, uint state)
    {
        if (state == 0x00020001 && index == 0x28)
        {
            Module.Arena.Bounds = D053Ambrose.DefaultBounds;
            _aoe = null;
        }
    }
}

class PsychicWave(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.PsychicWave));
class Psychokinesis(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.Psychokinesis), new AOEShapeRect(70, 6.5f));

class ExtrasensoryExpulsion(BossModule module) : Components.Knockback(module, maxCasts: 1)
{
    private const float QuarterWidth = 7.5f;
    private const float QuarterHeight = 9.75f;
    private const float HalfHeight = 19.5f;
    private readonly List<(WPos, Angle)> data = [];
    public DateTime Activation;
    private readonly List<Source> _sources = [];
    private static readonly AOEShapeRect rectNS = new(HalfHeight, QuarterWidth);
    private static readonly AOEShapeRect rectEW = new(15, QuarterHeight);
    private static readonly Angle[] angles = [-0.003f.Degrees(), 180.Degrees(), -89.982f.Degrees(), 89.977f.Degrees()];

    public override IEnumerable<Source> Sources(int slot, Actor actor) => _sources;

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.ExtrasensoryExpulsionNorthSouth)
        {
            Activation = spell.NPCFinishAt.AddSeconds(0.8f);
            HandleCastStarted(caster.Position);
        }
    }

    private void HandleCastStarted(WPos position)
    {
        if (position.AlmostEqual(new(182.7f, 8.75f), 0.1f))
        {
            AddSourceAndData(new WDir(QuarterWidth, -HalfHeight), rectNS, angles[0]);
            AddSourceAndData(new WDir(-QuarterWidth, HalfHeight), rectNS, angles[1]);
            AddSource(new WDir(0, -QuarterHeight), rectEW, angles[2]);
            AddSource(new WDir(0, QuarterHeight), rectEW, angles[3]);
        }
        else if (position.AlmostEqual(new(182.5f, -8.75f), 0.1f))
        {
            AddSourceAndData(new WDir(-QuarterWidth, -HalfHeight), rectNS, angles[0]);
            AddSourceAndData(new WDir(QuarterWidth, HalfHeight), rectNS, angles[1]);
            AddSource(new WDir(0, -QuarterHeight), rectEW, angles[3]);
            AddSource(new WDir(0, QuarterHeight), rectEW, angles[2]);
        }
    }

    private void AddSource(WDir direction, AOEShapeRect shape, Angle angle)
    {
        _sources.Add(new Source(Module.Center + direction, 20, Activation, shape, angle, Kind.DirForward));
    }

    private void AddSourceAndData(WDir direction, AOEShapeRect shape, Angle angle)
    {
        AddSource(direction, shape, angle);
        data.Add((_sources.Last().Origin, angle));
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.ExtrasensoryExpulsionNorthSouth or AID.ExtrasensoryExpulsionWestEast)
        {
            _sources.Clear();
            ++NumCasts;
        }
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (Sources(slot, actor).Any() || Activation > Module.WorldState.CurrentTime) // 0.8s delay to wait for action effect
        {
            var forbiddenZones = data.Select(w => ShapeDistance.InvertedRect(w.Item1, w.Item2, HalfHeight, 0, QuarterWidth)).ToList();
            hints.AddForbiddenZone(p => forbiddenZones.Max(f => f(p)), Activation);
        }
    }
}

class VoltaicSlash(BossModule module) : Components.SingleTargetCast(module, ActionID.MakeSpell(AID.VoltaicSlash));
class OverwhelmingCharge1(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.OverwhelmingCharge1), new AOEShapeCone(26, 90.Degrees()));

class OverwhelmingCharge2(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeCone cone = new(26, 90.Degrees());
    private AOEInstance _aoe;

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        var component = Module.FindComponent<ExtrasensoryExpulsion>()!.Sources(slot, actor).Any() || Module.FindComponent<ExtrasensoryExpulsion>()!.Activation > Module.WorldState.CurrentTime;
        if (_aoe != default)
            yield return _aoe with { Risky = !component };
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.OverwhelmingCharge2)
            _aoe = new(cone, caster.Position, spell.Rotation, spell.NPCFinishAt);
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.OverwhelmingCharge2)
            _aoe = default;
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        var component = Module.FindComponent<ExtrasensoryExpulsion>()!.Sources(slot, actor).Any() || Module.FindComponent<ExtrasensoryExpulsion>()!.Activation > Module.WorldState.CurrentTime;
        var aoe = ActiveAOEs(slot, actor).FirstOrDefault();
        if (component && ActiveAOEs(slot, actor).Any())
            hints.AddForbiddenZone(aoe.Shape, aoe.Origin, aoe.Rotation + 180.Degrees(), aoe.Activation);
        else
            base.AddAIHints(slot, actor, assignment, hints);
    }
}

class Electrolance(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.Electrolance), 22);
class WhorlOfTheMind(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.WhorlOfTheMind), 5);

class Rush(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeRect rect = new(33, 5);
    private readonly List<AOEInstance> _aoes = [];

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_aoes.Count > 0)
            yield return _aoes[0] with { Color = ArenaColor.Danger };
        for (var i = 1; i < _aoes.Count; ++i)
            yield return _aoes[i];
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.RushTelegraph)
        {
            var activation = spell.NPCFinishAt.AddSeconds(6.8f);
            var dir = spell.LocXZ - caster.Position;
            if (_aoes.Count < 7)
                _aoes.Add(new(new AOEShapeRect(dir.Length(), 5), caster.Position, Angle.FromDirection(dir), activation));
            else if (_aoes.Count == 7)
                _aoes.Add(new(rect, new(190, 19.5f), -180.Degrees(), activation));
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (_aoes.Count > 0 && (AID)spell.Action.ID is AID.Rush or AID.ElectrolanceAssimilation)
            _aoes.RemoveAt(0);
    }
}

class D053AmbroseStates : StateMachineBuilder
{
    public D053AmbroseStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<PsychicWaveArenaChange>()
            .ActivateOnEnter<PsychicWave>()
            .ActivateOnEnter<OverwhelmingCharge1>()
            .ActivateOnEnter<OverwhelmingCharge2>()
            .ActivateOnEnter<Psychokinesis>()
            .ActivateOnEnter<ExtrasensoryExpulsion>()
            .ActivateOnEnter<VoltaicSlash>()
            .ActivateOnEnter<Electrolance>()
            .ActivateOnEnter<Rush>()
            .ActivateOnEnter<WhorlOfTheMind>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "The Combat Reborn Team (Malediktus, LTS)", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 825, NameID = 12695, SortOrder = 4)]
public class D053Ambrose(WorldState ws, Actor primary) : BossModule(ws, primary, ArenaCenter, StartingBounds)
{
    public static readonly WPos ArenaCenter = new(190, 0);
    public static readonly ArenaBoundsRect StartingBounds = new(32.5f, 24);
    public static readonly ArenaBoundsRect DefaultBounds = new(15, 19.5f);

    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actor(PrimaryActor, ArenaColor.Enemy);
        foreach (var s in Enemies(OID.Superfluity))
            Arena.Actor(s, ArenaColor.Enemy);
        foreach (var s in Enemies(OID.OrigenicsEyeborg))
            Arena.Actor(s, ArenaColor.Enemy);
    }
}
