namespace BossMod.Dawntrail.Alliance.A12Fafnir;

class DarkMatterBlast(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.DarkMatterBlast));
class HorridRoar2(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.HorridRoarAOE), 4);
class HorridRoar3(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.HorridRoarSpread), 8);
class SpikeFlail(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SpikeFlail), new AOEShapeCone(80, 135.Degrees()));
class Touchdown(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.Touchdown), new AOEShapeCircle(24));

class HurricaneWing1(BossModule module) : Components.ConcentricAOEs(module, _shapes)
{
    private static readonly AOEShape[] _shapes = [new AOEShapeCircle(9), new AOEShapeDonut(9, 16), new AOEShapeDonut(16, 23), new AOEShapeDonut(23, 30)];

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.HurricaneWingConcentricA1)
            AddSequence(Arena.Center, Module.CastFinishAt(spell));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (Sequences.Count > 0)
        {
            var order = (AID)spell.Action.ID switch
            {
                AID.HurricaneWingConcentricA1 => 0,
                AID.HurricaneWingConcentricA2 => 1,
                AID.HurricaneWingConcentricA3 => 2,
                AID.HurricaneWingConcentricA4 => 3,
                _ => -1
            };
            AdvanceSequence(order, Arena.Center, WorldState.FutureTime(3));
        }
    }
}

class HurricaneWing2(BossModule module) : Components.ConcentricAOEs(module, _shapes)
{
    private static readonly AOEShape[] _shapes = [new AOEShapeCircle(9), new AOEShapeDonut(9, 16), new AOEShapeDonut(16, 23), new AOEShapeDonut(23, 30)];

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.HurricaneWingConcentricB1)
            AddSequence(Arena.Center, Module.CastFinishAt(spell));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (Sequences.Count > 0)
        {
            var order = (AID)spell.Action.ID switch
            {
                AID.HurricaneWingConcentricB1 => 0,
                AID.HurricaneWingConcentricB2 => 1,
                AID.HurricaneWingConcentricB3 => 2,
                AID.HurricaneWingConcentricB4 => 3,
                _ => -1
            };
            AdvanceSequence(order, Arena.Center, WorldState.FutureTime(3));
        }
    }
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "The Combat Reborn Team (LTS)", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1015, NameID = 13662)]
public class A12Fafnir(WorldState ws, Actor primary) : BossModule(ws, primary, ArenaCenter, new ArenaBoundsCircle(34.5f))
{
    public static readonly WPos ArenaCenter = new(-500, 600);
    public static readonly ArenaBoundsCircle DefaultBounds = new(30);
    public static readonly ArenaBoundsCircle FireArena = new(16);
}
