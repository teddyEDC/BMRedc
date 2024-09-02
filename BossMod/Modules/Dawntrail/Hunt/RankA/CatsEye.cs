namespace BossMod.Dawntrail.Hunt.RankA.CatsEye;

public enum OID : uint
{
    Boss = 0x4396, // R6.020, x1
}

public enum AID : uint
{
    AutoAttack = 38517, // Boss->player, no cast, single-target
    CatsEye1 = 38510, // Boss->location, 7.0s cast, range 40 circle, non-inverted.
    KillerCuriosity = 38514, // Boss->self, 4.0s cast, single-target
    CatsEye2 = 38511, // Boss->location, 7.0s cast, range 40 circle, inverted gaze on end.
    Unknown = 38516, // Boss->self, no cast, single-target
    GravitationalWave = 39887, // Boss->self, 5.0s cast, range 40 circle
    BloodshotGaze = 38515, // Boss->players, 5.0s cast, range 8 circle
}

class CatsEye1(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.CatsEye1), 40);

class CatsEye2(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.CatsEye2), 40);

class CatsEye1Gaze : Components.CastGaze
{
    public CatsEye1Gaze(BossModule module) : base(module, ActionID.MakeSpell(AID.CatsEye1), false) { }
    public override IEnumerable<Eye> ActiveEyes(int slot, Actor actor) => _casters.Select(c => new Eye(c.CastInfo!.LocXZ, Module.CastFinishAt(c.CastInfo)));
}

class CatsEye2Gaze : Components.CastGaze
{
    public CatsEye2Gaze(BossModule module) : base(module, ActionID.MakeSpell(AID.CatsEye2), true) { }
    public override IEnumerable<Eye> ActiveEyes(int slot, Actor actor) => _casters.Select(c => new Eye(c.CastInfo!.LocXZ, Module.CastFinishAt(c.CastInfo)));
}

class GravitationalWave(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.GravitationalWave), "Raidwide!");

class BloodshotGaze : Components.CastGaze
{
    public BloodshotGaze(BossModule module)
        : base(module, ActionID.MakeSpell(AID.BloodshotGaze), false) { }

    public override IEnumerable<Eye> ActiveEyes(int slot, Actor actor)
    {
        var stackComponent = Module.FindComponent<BloodshotStack>();

        var targetPosition = stackComponent?.ActiveStackTargets.FirstOrDefault()?.Position;

        return _casters.Select(c => new Eye(targetPosition ?? c.Position, Module.CastFinishAt(c.CastInfo)));
    }
}

class BloodshotStack(BossModule module) : Components.StackWithCastTargets(module, ActionID.MakeSpell(AID.BloodshotGaze), 8);

class CatsEyeStates : StateMachineBuilder
{
    public CatsEyeStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<CatsEye1>()
            .ActivateOnEnter<CatsEye2>()
            .ActivateOnEnter<CatsEye1Gaze>()
            .ActivateOnEnter<CatsEye2Gaze>()
            .ActivateOnEnter<GravitationalWave>()
            .ActivateOnEnter<BloodshotGaze>()
            .ActivateOnEnter<BloodshotStack>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Contributed, Contributors = "Shinryin", GroupType = BossModuleInfo.GroupType.Hunt, GroupID = (uint)BossModuleInfo.HuntRank.A, NameID = 13436)]
public class CatsEye(WorldState ws, Actor primary) : SimpleBossModule(ws, primary);