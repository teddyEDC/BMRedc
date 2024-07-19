namespace BossMod.Dawntrail.Raid.M3NBruteBomber;

class BrutalImpact1(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.BrutalImpact1));

class BrutalLariat3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BrutalLariat3), new AOEShapeRect(50, 17));
class BrutalLariat4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BrutalLariat4), new AOEShapeRect(50, 17));
class ExplosiveRain4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.ExplosiveRain4), new AOEShapeCircle(6));

class ExplosiveRain(BossModule module) : Components.ConcentricAOEs(module, _shapes)
{
    private static readonly AOEShape[] _shapes = [new AOEShapeCircle(8), new AOEShapeDonut(8, 16), new AOEShapeDonut(16, 24)];

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.ExplosiveRain1)
            AddSequence(Module.Center, Module.CastFinishAt(spell));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (Sequences.Count > 0)
        {
            var order = (AID)spell.Action.ID switch
            {
                AID.ExplosiveRain1 => 0,
                AID.ExplosiveRain2 => 1,
                AID.ExplosiveRain3 => 2,
                _ => -1
            };
            AdvanceSequence(order, Module.Center, WorldState.FutureTime(2));
        }
    }
}
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

class LariatCombo9(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LariatCombo9), new AOEShapeRect(70, 17));
class LariatCombo10(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LariatCombo10), new AOEShapeRect(70, 17));
class LariatCombo11(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LariatCombo11), new AOEShapeRect(50, 17));
class LariatCombo12(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LariatCombo12), new AOEShapeRect(50, 17));

class BrutalBurn2(BossModule module) : Components.StackWithCastTargets(module, ActionID.MakeSpell(AID.BrutalBurn2), 6, 8);

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "The Combat Reborn Team", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 989, NameID = 13356)]
public class M3NBruteBomber(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), new ArenaBoundsSquare(15));
