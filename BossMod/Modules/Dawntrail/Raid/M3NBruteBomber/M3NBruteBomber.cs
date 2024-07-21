namespace BossMod.Dawntrail.Raid.M3NBruteBomber;

class BrutalImpact(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.BrutalImpactFirst));
class KnuckleSandwich(BossModule module) : Components.CastSharedTankbuster(module, ActionID.MakeSpell(AID.KnuckleSandwich), 6);
class BrutalLariat1(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BrutalLariat1), new AOEShapeRect(20, 30, 5, -90.Degrees()));
class BrutalLariat2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BrutalLariat2), new AOEShapeRect(20, 30, 5, 90.Degrees()));

class MurderousMist(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.MurderousMist), new AOEShapeCone(40, 135.Degrees()));

class SelfDestruct(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];

    private static readonly AOEShapeCircle circle = new(8);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_aoes.Count > 0)
            yield return _aoes[0] with { Color = ArenaColor.Danger };
        if (_aoes.Count > 1)
            yield return _aoes[1] with { Risky = false };
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.SelfDestruct1 or AID.SelfDestruct2)
        {
            _aoes.Add(new(circle, caster.Position, spell.Rotation, Module.CastFinishAt(spell)));
            _aoes.Sort((x, y) => x.Activation.CompareTo(y.Activation));
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (_aoes.Count > 0 && (AID)spell.Action.ID is AID.SelfDestruct1 or AID.SelfDestruct2)
            _aoes.RemoveAt(0);
    }
}

class FireSpin4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.FireSpin4), new AOEShapeCone(40, 30.Degrees()));
class FireSpin5(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.FireSpin5), new AOEShapeCone(40, 30.Degrees()));
class InfernalSpin4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.InfernalSpin4), new AOEShapeCone(40, 30.Degrees()));
class InfernalSpin5(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.InfernalSpin5), new AOEShapeCone(40, 30.Degrees()));
class BrutalBurn(BossModule module) : Components.StackWithCastTargets(module, ActionID.MakeSpell(AID.BrutalBurn), 6, 8);

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "The Combat Reborn Team", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 989, NameID = 13356)]
public class M3NBruteBomber(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), new ArenaBoundsSquare(15));
