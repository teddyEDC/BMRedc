namespace BossMod.Dawntrail.Raid.M1NBlackCat;

class BlackCatCrossing3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlackCatCrossing3), new AOEShapeCone(60, 22.5f.Degrees()));
class BlackCatCrossing4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlackCatCrossing4), new AOEShapeCone(60, 22.5f.Degrees()));
class BloodyScratch(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.BloodyScratch));

class OneTwoPaw(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];

    private static readonly AOEShapeCone cone = new(60, 90.Degrees());

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_aoes.Count > 0)
            yield return _aoes[0] with { Color = ArenaColor.Danger };
        if (_aoes.Count > 1)
            yield return _aoes[1] with { Risky = false };
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.OneTwoPaw2 or AID.OneTwoPaw3 or AID.OneTwoPaw5 or AID.OneTwoPaw6)
        {
            _aoes.Add(new(cone, caster.Position, spell.Rotation, spell.NPCFinishAt));
            _aoes.SortBy(x => x.Activation);
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (_aoes.Count > 0 && (AID)spell.Action.ID is AID.OneTwoPaw2 or AID.OneTwoPaw3 or AID.OneTwoPaw5 or AID.OneTwoPaw6)
            _aoes.RemoveAt(0);
    }
}

class BlackCatCrossing(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];

    private static readonly AOEShapeCone cone = new(60, 22.5f.Degrees());

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_aoes.Count > 0)
            yield return _aoes[0] with { Color = ArenaColor.Danger };
        if (_aoes.Count > 1)
            yield return _aoes[1] with { Risky = false };
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.BlackCatCrossing3 or AID.BlackCatCrossing4)
        {
            _aoes.Add(new(cone, caster.Position, spell.Rotation, spell.NPCFinishAt));
            _aoes.SortBy(x => x.Activation);
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (_aoes.Count > 0 && (AID)spell.Action.ID is AID.BlackCatCrossing3 or AID.BlackCatCrossing4)
            _aoes.RemoveAt(0);
    }
}

class BiscuitMaker(BossModule module) : Components.SingleTargetCast(module, ActionID.MakeSpell(AID.BiscuitMaker));
class Clawful2(BossModule module) : Components.StackWithCastTargets(module, ActionID.MakeSpell(AID.Clawful2), 5, 8);
class Shockwave2(BossModule module) : Components.KnockbackFromCastTarget(module, ActionID.MakeSpell(AID.Shockwave2), 18, stopAtWall: true, kind: Kind.AwayFromOrigin);

class PredaceousPounce2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.PredaceousPounce2), new AOEShapeCircle(11));
class PredaceousPounce3(BossModule module) : Components.ChargeAOEs(module, ActionID.MakeSpell(AID.PredaceousPounce3), 3);
class PredaceousPounce5(BossModule module) : Components.ChargeAOEs(module, ActionID.MakeSpell(AID.PredaceousPounce5), 3);
class PredaceousPounce6(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.PredaceousPounce6), new AOEShapeCircle(11));

class GrimalkinGale2(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.GrimalkinGale2), 5);

class LeapingOneTwoPaw(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];

    private static readonly AOEShapeCone cone = new(60, 90.Degrees());

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_aoes.Count > 0)
            yield return _aoes[0] with { Color = ArenaColor.Danger };
        if (_aoes.Count > 1)
            yield return _aoes[1] with { Risky = false };
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.LeapingOneTwoPaw6 or AID.LeapingOneTwoPaw7 or AID.LeapingOneTwoPaw9 or AID.LeapingOneTwoPaw10)
        {
            _aoes.Add(new(cone, caster.Position, spell.Rotation, spell.NPCFinishAt));
            _aoes.SortBy(x => x.Activation);
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (_aoes.Count > 0 && (AID)spell.Action.ID is AID.LeapingOneTwoPaw6 or AID.LeapingOneTwoPaw7 or AID.LeapingOneTwoPaw9 or AID.LeapingOneTwoPaw10)
            _aoes.RemoveAt(0);
    }
}

class LeapingBlackCatCrossing(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];

    private static readonly AOEShapeCone cone = new(60, 22.5f.Degrees());

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_aoes.Count > 0)
            yield return _aoes[0] with { Color = ArenaColor.Danger };
        if (_aoes.Count > 1)
            yield return _aoes[1] with { Risky = false };
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.LeapingBlackCatCrossing4 or AID.LeapingBlackCatCrossing5)
        {
            _aoes.Add(new(cone, caster.Position, spell.Rotation, spell.NPCFinishAt));
            _aoes.SortBy(x => x.Activation);
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (_aoes.Count > 0 && (AID)spell.Action.ID is AID.LeapingBlackCatCrossing4 or AID.LeapingBlackCatCrossing5)
            _aoes.RemoveAt(0);
    }
}

class Overshadow(BossModule module) : Components.LineStack(module, ActionID.MakeSpell(AID.Overshadow1), ActionID.MakeSpell(AID.Overshadow2), 5.2f);

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "The Combat Reborn Team", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 985, NameID = 12686)]
public class M1NBlackCat(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), new ArenaBoundsSquare(20));
