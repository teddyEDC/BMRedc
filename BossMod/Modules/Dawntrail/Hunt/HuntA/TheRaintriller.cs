namespace BossMod.Dawntrail.Hunt.RankA.TheRaintriller;

public enum OID : uint
{
    Boss = 0x457F, // R4.800, x1
}

public enum AID : uint
{
    // Do Re Misery has multiple AIDs with variant casts, 1 -> One Yell, 2 -> 2 Yells, 3 -> 3 Yells.
    // Croakdown/Ribbitygibbet/ChirpOfTheWisp are NOT what they correspond to in-game...
    AutoAttack = 872, // Boss->player, no cast, single-target
    Unknown = 39804, // Boss->location, no cast, single-target (I think this corresponds to yells??)
    DoReMisery1 = 39758, // Boss->self, 5.0s cast, single-target
    Croakdown = 39750, // Boss->self, 1.0s cast, range 12 circle "Chirp!"
    DoReMisery2 = 39751, // Boss->self, 6.2s cast, single-target
    Ribbitygibbet = 39752, // Boss->self, 1.0s cast, range 10-40 donut "Ribbit!"
    DoReMisery3 = 39749, // Boss->self, 7.5s cast, single-target
    ChirpOTheWisp = 39753, // Boss->self, 1.0s cast, range 40 135-degree cone "Croak!"
    DropOfVenom = 39754, // Boss->player, 5.0s cast, range 6 circle
}

class DoReMisery : Components.GenericAOEs
{
    private Actor? _caster;
    private List<AOEInstance> _activeAOEs = new();
    private Queue<AID> _doReMiseryActions = new();
    private static readonly AOEShapeCircle _shapeOut = new(12);
    private static readonly AOEShapeDonut _shapeIn = new(10, 40);
    private static readonly AOEShapeCone _shapeBehind = new(40, 135.Degrees());
    private int _actionCount = 0;

    private static readonly Dictionary<ushort, AID[]> DoReMiseryNpcYellMap = new()
        {
            { 17815, new[] { AID.Croakdown } },
            { 17719, new[] { AID.Croakdown, AID.Ribbitygibbet } },
            { 17720, new[] { AID.Ribbitygibbet, AID.Croakdown, AID.ChirpOTheWisp } },
            { 17721, new[] { AID.Croakdown, AID.Ribbitygibbet, AID.ChirpOTheWisp } },
            { 17722, new[] { AID.ChirpOTheWisp, AID.Ribbitygibbet, AID.Croakdown } },
            { 17723, new[] { AID.Ribbitygibbet, AID.ChirpOTheWisp, AID.Croakdown } },
            { 17724, new[] { AID.ChirpOTheWisp, AID.Croakdown, AID.Ribbitygibbet } },
            { 17725, new[] { AID.Croakdown, AID.ChirpOTheWisp, AID.Ribbitygibbet } },
            { 17734, new[] { AID.Croakdown, AID.ChirpOTheWisp, AID.Croakdown } },
            { 17793, new[] { AID.Ribbitygibbet, AID.Ribbitygibbet, AID.Croakdown } }
        };

    public DoReMisery(BossModule module) : base(module) { }

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        foreach (var aoe in _activeAOEs)
        {
            if (aoe.Shape == _shapeBehind)
            {
                yield return new AOEInstance(_shapeBehind, _caster.Position, _caster.Rotation, aoe.Activation, aoe.Color, aoe.Risky);
            }
            else
            {
                yield return aoe;
            }
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (spell.Action.ID is not (uint)AID.Croakdown and not (uint)AID.Ribbitygibbet and not (uint)AID.ChirpOTheWisp || _activeAOEs.Count == 0)
            return;

        _activeAOEs.RemoveAt(0);
        _actionCount++;

        if (_actionCount == 1 && _activeAOEs.Count > 0 || _actionCount == 2 && _activeAOEs.Count > 0)
            _activeAOEs[0] = new AOEInstance(_activeAOEs[0].Shape, _activeAOEs[0].Origin, _activeAOEs[0].Rotation, _activeAOEs[0].Activation, Colors.Danger, true);

        if (_doReMiseryActions.Count > 0)
            _activeAOEs.Add(CreateAOEInstance(_doReMiseryActions.Dequeue(), WorldState.CurrentTime.AddSeconds(10), _actionCount >= 2 ? Colors.Danger : Colors.AOE, false));

        if (_doReMiseryActions.Count == 0 && _activeAOEs.Count == 0)
            _activeAOEs.Clear(); _doReMiseryActions.Clear(); _actionCount = 0;
    }

    public override void OnActorNpcYell(Actor actor, ushort id)
    {
        if (_caster == null) _caster = actor;
        if (!DoReMiseryNpcYellMap.TryGetValue(id, out var actions)) return;

        _doReMiseryActions.Clear(); _activeAOEs.Clear(); _actionCount = 0;
        foreach (var action in actions) _doReMiseryActions.Enqueue(action);

        if (_doReMiseryActions.Count > 0)
            _activeAOEs.Add(CreateAOEInstance(_doReMiseryActions.Dequeue(), WorldState.CurrentTime.AddSeconds(10), Colors.Danger, true));

        if (_doReMiseryActions.Count > 0)
            _activeAOEs.Add(CreateAOEInstance(_doReMiseryActions.Dequeue(), WorldState.CurrentTime.AddSeconds(10), Colors.AOE, false));
    }

    private AOEInstance CreateAOEInstance(AID aid, DateTime activation, uint color, bool risky) => aid switch
    {
        AID.Ribbitygibbet => new AOEInstance(_shapeIn, _caster.Position, default, activation, color, risky),
        AID.Croakdown => new AOEInstance(_shapeOut, _caster.Position, default, activation, color, risky),
        AID.ChirpOTheWisp => new AOEInstance(_shapeBehind, _caster.Position, _caster.Rotation, activation, color, risky),
        _ => new AOEInstance(_shapeIn, _caster.Position, default, activation, color, risky)
    };
}

class DropOfVenom(BossModule module) : Components.StackWithCastTargets(module, ActionID.MakeSpell(AID.DropOfVenom), 6);

class TheRaintrillerStates : StateMachineBuilder
{
    public TheRaintrillerStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<DoReMisery>()
            .ActivateOnEnter<DropOfVenom>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Contributed, Contributors = "Shinryin", GroupType = BossModuleInfo.GroupType.Hunt, GroupID = (uint)BossModuleInfo.HuntRank.A, NameID = 13442)]
public class TheRaintriller(WorldState ws, Actor primary) : SimpleBossModule(ws, primary);
