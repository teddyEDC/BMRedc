namespace BossMod.Dawntrail.Hunt.RankS.ArchAethereater;

public enum OID : uint
{
    Boss = 0x4544, // R9.6
}

public enum AID : uint
{
    AutoAttack = 39517, // Boss->player, no cast, single-target
    Aethermodynamics1 = 39840, // Boss->self, 5.0s cast, range 40 circle
    Aethermodynamics2 = 39512, // Boss->self, 5.0s cast, range 40 circle
    Aethermodynamics3 = 39511, // Boss->self, 5.0s cast, range 40 circle
    Aethermodynamics4 = 39839, // Boss->self, 5.0s cast, range 40 circle
    FireIV1 = 39829, // Boss->self, 5.0s cast, range 15 circle
    FireIV2 = 39800, // Boss->self, 1.0s cast, range 15 circle
    FireIV3 = 39513, // Boss->self, 1.0s cast, range 15 circle
    FireIV4 = 39802, // Boss->self, 1.0s cast, range 15 circle
    FireIV5 = 39837, // Boss->self, 5.0s cast, range 15 circle
    BlizzardIV1 = 39838, // Boss->self, 5.0s cast, range 6-40 donut
    BlizzardIV2 = 39514, // Boss->self, 1.0s cast, range 6-40 donut
    BlizzardIV3 = 39801, // Boss->self, 1.0s cast, range 6-40 donut
    BlizzardIV4 = 39803, // Boss->self, 1.0s cast, range 6-40 donut
    BlizzardIV5 = 39830, // Boss->self, 5.0s cast, range 6-40 donut
    SoullessStream1 = 39509, // Boss->self, 5.0s cast, range 40 180-degree cone, FireIV2 combo or FireIV4 combo
    SoullessStream2 = 39510, // Boss->self, 5.0s cast, range 40 180-degree cone, BlizzardIV2 or BlizzardIV4 combo
    SoullessStream3 = 39507, // Boss->self, 5.0s cast, range 40 180-degree cone, BlizzardIV3 combo
    SoullessStream4 = 39508, // Boss->self, 5.0s cast, range 40 180-degree cone, FireIV3 combo
    Obliterate = 39515, // Boss->players, 5.0s cast, range 6 circle, stack
    Meltdown = 39516, // Boss->self, 3.0s cast, range 40 width 10 rect
    unknown = 19277 // Boss->self, no cast, single-target
}

public enum SID : uint
{
    Heatstroke = 4141, // Boss->player, extra=0x0
    ColdSweats = 4142, // Boss->player, extra=0x0
    Pyretic = 960, // none->player, extra=0x0
    FreezingUp = 2540, // none->player, extra=0x0
    DeepFreeze = 3519 // none->player, extra=0x0
}

class Heatstroke(BossModule module) : Components.StayMove(module)
{
    private readonly DateTime[] _expire = new DateTime[PartyState.MaxAllianceSize];
    private BitMask _pyretic;
    private BitMask _heatstroke;

    public override void Update()
    {
        base.Update();
        var deadline = WorldState.FutureTime(3);
        for (var i = 0; i < _expire.Length; ++i)
            Requirements[i] = _expire[i] != default && _expire[i] < deadline ? Requirement.Stay : Requirement.None;
    }

    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if (Raid.FindSlot(actor.InstanceID) is var slot && slot >= 0)
        {
            if ((SID)status.ID == SID.Heatstroke)
            {
                _heatstroke.Set(Raid.FindSlot(actor.InstanceID));
                _expire[slot] = status.ExpireAt;
            }
            else if ((SID)status.ID == SID.Pyretic)
                _pyretic.Set(Raid.FindSlot(actor.InstanceID));
        }
    }

    public override void OnStatusLose(Actor actor, ActorStatus status)
    {
        if (Raid.FindSlot(actor.InstanceID) is var slot && slot >= 0)
        {
            if ((SID)status.ID == SID.Heatstroke)
                _heatstroke.Clear(Raid.FindSlot(actor.InstanceID));
            else if ((SID)status.ID == SID.Pyretic)
            {
                _expire[slot] = default;
                _pyretic.Clear(Raid.FindSlot(actor.InstanceID));
            }
        }
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        if (_heatstroke[slot])
            hints.Add($"Heatstroke on you in {(actor.FindStatus(SID.Heatstroke)!.Value.ExpireAt - WorldState.CurrentTime).TotalSeconds:f1}s. (Pyretic!)");
        else if (_pyretic[slot])
            hints.Add("Pyretic on you! STOP everything!");
        if (_expire[slot] != default && actor.IsDead)
        {
            _expire[slot] = default;
            Requirements[slot] = Requirement.None;
        }
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (Requirements[slot] == Requirement.Stay && _expire[slot] != default && _expire[slot] < WorldState.FutureTime(0.3f))
            hints.ForcedMovement = new();
    }
}

class ColdSweats(BossModule module) : Components.StayMove(module)
{
    public DateTime ExpiresAt;
    private BitMask _freezing;
    private BitMask _coldsweats;

    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if (Raid.FindSlot(actor.InstanceID) is var slot && slot >= 0 && slot < Requirements.Length)
        {
            if ((SID)status.ID == SID.ColdSweats)
            {
                Requirements[slot] = Requirement.Move;
                _coldsweats.Set(Raid.FindSlot(actor.InstanceID));
                if (actor == Module.Raid.Player()!)
                    ExpiresAt = status.ExpireAt;
            }
            else if ((SID)status.ID == SID.FreezingUp)
            {
                Requirements[slot] = Requirement.Move;
                _freezing.Set(Raid.FindSlot(actor.InstanceID));
            }
        }
    }

    public override void OnStatusLose(Actor actor, ActorStatus status)
    {
        if (Raid.FindSlot(actor.InstanceID) is var slot && slot >= 0 && slot < Requirements.Length)
        {
            if ((SID)status.ID == SID.ColdSweats)
            {
                Requirements[slot] = Requirement.None;
                _coldsweats.Clear(Raid.FindSlot(actor.InstanceID));
            }
            else if ((SID)status.ID == SID.FreezingUp)
            {
                Requirements[slot] = Requirement.None;
                ExpiresAt = default;
                _freezing.Clear(Raid.FindSlot(actor.InstanceID));
            }
        }
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        if (_coldsweats[slot])
            hints.Add($"Cold Sweats on you in {(actor.FindStatus(SID.ColdSweats)!.Value.ExpireAt - WorldState.CurrentTime).TotalSeconds:f1}s. (Freezing!)");
        else if (_freezing[slot])
            hints.Add("Freezing on you! MOVE!");
    }
}

class Aethermodynamics1(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.Aethermodynamics1));
class Aethermodynamics2(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.Aethermodynamics2));
class Aethermodynamics3(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.Aethermodynamics3));
class Aethermodynamics4(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.Aethermodynamics4));

class Obliterate(BossModule module) : Components.StackWithCastTargets(module, ActionID.MakeSpell(AID.Obliterate), 6, 8);
class Meltdown(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.Meltdown), new AOEShapeRect(40, 5));

class Blizzard(BossModule module, AID aid) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(aid), new AOEShapeDonut(6, 40));
class BlizzardIV1(BossModule module) : Blizzard(module, AID.BlizzardIV1);
class BlizzardIV5(BossModule module) : Blizzard(module, AID.BlizzardIV5);

class Fire(BossModule module, AID aid) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(aid), new AOEShapeCircle(15));
class FireIV1(BossModule module) : Fire(module, AID.FireIV1);
class FireIV5(BossModule module) : Fire(module, AID.FireIV5);

class SoullessStreamFireBlizzardCombo(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeCone cone = new(40, 90.Degrees());
    private static readonly AOEShapeDonut donut = new(6, 40);
    private static readonly AOEShapeCircle circle = new(15);
    private readonly List<AOEInstance> _aoes = [];
    private static readonly HashSet<AID> castEnd = [AID.SoullessStream1, AID.SoullessStream2, AID.SoullessStream3, AID.SoullessStream4,
    AID.BlizzardIV2, AID.BlizzardIV3, AID.BlizzardIV4, AID.FireIV2, AID.FireIV3, AID.FireIV4];

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_aoes.Count > 0)
            yield return _aoes[0] with { Color = Colors.Danger };
        if (_aoes.Count > 1)
            yield return _aoes[1];
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        switch ((AID)spell.Action.ID)
        {
            case AID.SoullessStream1:
            case AID.SoullessStream4:
                AddAOEs(cone, circle, spell);
                break;
            case AID.SoullessStream2:
            case AID.SoullessStream3:
                AddAOEs(cone, donut, spell);
                break;
        }
    }

    private void AddAOEs(AOEShape primaryShape, AOEShape secondaryShape, ActorCastInfo spell)
    {
        var position = Module.PrimaryActor.Position;
        _aoes.Add(new(primaryShape, position, spell.Rotation, Module.CastFinishAt(spell)));
        _aoes.Add(new(secondaryShape, position, default, Module.CastFinishAt(spell, 2.5f)));
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (_aoes.Count > 0 && castEnd.Contains((AID)spell.Action.ID))
            _aoes.RemoveAt(0);
    }
}

class ArchAethereaterStates : StateMachineBuilder
{
    public ArchAethereaterStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<Heatstroke>()
            .ActivateOnEnter<ColdSweats>()
            .ActivateOnEnter<Meltdown>()
            .ActivateOnEnter<Aethermodynamics1>()
            .ActivateOnEnter<Aethermodynamics2>()
            .ActivateOnEnter<Aethermodynamics3>()
            .ActivateOnEnter<Aethermodynamics4>()
            .ActivateOnEnter<Obliterate>()
            .ActivateOnEnter<BlizzardIV1>()
            .ActivateOnEnter<FireIV1>()
            .ActivateOnEnter<BlizzardIV5>()
            .ActivateOnEnter<FireIV5>()
            .ActivateOnEnter<SoullessStreamFireBlizzardCombo>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "The Combat Reborn Team (Malediktus)", GroupType = BossModuleInfo.GroupType.Hunt, GroupID = (uint)BossModuleInfo.HuntRank.SS, NameID = 13406)]
public class ArchAethereater(WorldState ws, Actor primary) : SimpleBossModule(ws, primary)
{
    public override bool NeedToJump(WPos from, WDir dir)
    {
        var component = FindComponent<ColdSweats>()!;
        return component.ExpiresAt != default && component.ExpiresAt.AddSeconds(-0.1f) <= WorldState.CurrentTime;
    }
}
