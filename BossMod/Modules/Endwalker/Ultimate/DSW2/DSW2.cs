namespace BossMod.Endwalker.Ultimate.DSW2;

class P2AscalonsMercyConcealed(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.AscalonsMercyConcealedAOE), new AOEShapeCone(50, 15.Degrees()));

abstract class AscalonMight(BossModule module, uint oid) : Components.Cleave(module, ActionID.MakeSpell(AID.AscalonsMight), new AOEShapeCone(50, 30.Degrees()), [oid]);
class P2AscalonMight(BossModule module) : AscalonMight(module, (uint)OID.BossP2);

class P2UltimateEnd(BossModule module) : Components.CastCounter(module, ActionID.MakeSpell(AID.UltimateEndAOE));
class P3Drachenlance(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.DrachenlanceAOE), new AOEShapeCone(13, 45.Degrees()));
class P3SoulTether(BossModule module) : Components.TankbusterTether(module, ActionID.MakeSpell(AID.SoulTether), (uint)TetherID.HolyShieldBash, 5);
class P4Resentment(BossModule module) : Components.CastCounter(module, ActionID.MakeSpell(AID.Resentment));
class P5TwistingDive(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.TwistingDive), new AOEShapeRect(60, 5));

abstract class Cauterize(BossModule module, AID aid) : Components.SimpleAOEs(module, ActionID.MakeSpell(aid), new AOEShapeRect(48, 10));
class P5Cauterize1(BossModule module) : Cauterize(module, AID.Cauterize1);
class P5Cauterize2(BossModule module) : Cauterize(module, AID.Cauterize2);

class P5SpearOfTheFury(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.SpearOfTheFuryP5), new AOEShapeRect(50, 5));
class P5AscalonMight(BossModule module) : AscalonMight(module, (uint)OID.BossP5);
class P5Surrender(BossModule module) : Components.CastCounter(module, ActionID.MakeSpell(AID.Surrender));
class P6SwirlingBlizzard(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.SwirlingBlizzard), new AOEShapeDonut(20, 35));
class P7Shockwave(BossModule module) : Components.CastCounter(module, ActionID.MakeSpell(AID.ShockwaveP7));
class P7AlternativeEnd(BossModule module) : Components.CastCounter(module, ActionID.MakeSpell(AID.AlternativeEnd));

[ModuleInfo(BossModuleInfo.Maturity.Verified, PrimaryActorOID = (uint)OID.BossP2, GroupType = BossModuleInfo.GroupType.CFC, GroupID = 788, PlanLevel = 90)]
public class DSW2(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), BoundsCircle)
{
    public static readonly ArenaBoundsCircle BoundsCircle = new(21); // p2, intermission
    public static readonly ArenaBoundsSquare BoundsSquare = new(21); // p3, p4

    private Actor? _bossP3;
    private Actor? _leftEyeP4;
    private Actor? _rightEyeP4;
    private Actor? _nidhoggP4;
    private Actor? _serCharibert;
    private Actor? _spear;
    private Actor? _bossP5;
    private Actor? _nidhoggP6;
    private Actor? _hraesvelgrP6;
    private Actor? _bossP7;
    public Actor? ArenaFeatures { get; private set; }
    public Actor? BossP2() => PrimaryActor;
    public Actor? BossP3() => _bossP3;
    public Actor? LeftEyeP4() => _leftEyeP4;
    public Actor? RightEyeP4() => _rightEyeP4;
    public Actor? NidhoggP4() => _nidhoggP4;
    public Actor? SerCharibert() => _serCharibert;
    public Actor? Spear() => _spear;
    public Actor? BossP5() => _bossP5;
    public Actor? NidhoggP6() => _nidhoggP6;
    public Actor? HraesvelgrP6() => _hraesvelgrP6;
    public Actor? BossP7() => _bossP7;

    protected override void UpdateModule()
    {
        // TODO: this is an ugly hack, think how multi-actor fights can be implemented without it...
        // the problem is that on wipe, any actor can be deleted and recreated in the same frame
        var enemyMappings = new (int phaseIndex, uint oid, Actor? field)[]
        {
            (0, (uint)OID.ArenaFeatures, ArenaFeatures),
            (1, (uint)OID.BossP3, _bossP3),
            (2, (uint)OID.LeftEye, _leftEyeP4),
            (2, (uint)OID.RightEye, _rightEyeP4),
            (2, (uint)OID.NidhoggP4, _nidhoggP4),
            (3, (uint)OID.SerCharibert, _serCharibert),
            (3, (uint)OID.SpearOfTheFury, _spear),
            (4, (uint)OID.BossP5, _bossP5),
            (5, (uint)OID.NidhoggP6, _nidhoggP6),
            (5, (uint)OID.HraesvelgrP6, _hraesvelgrP6),
            (6, (uint)OID.DragonKingThordan, _bossP7),
        };

        for (var i = 0; i < 11; ++i)
        {
            var (phaseIndex, oid, field) = enemyMappings[i];

            if (field == null && StateMachine.ActivePhaseIndex == phaseIndex)
            {
                var enemies = Enemies(oid);
                enemyMappings[i].field = enemies.Count != 0 ? enemies[0] : null;
            }
        }

        ArenaFeatures = enemyMappings[0].field;
        _bossP3 = enemyMappings[1].field;
        _leftEyeP4 = enemyMappings[2].field;
        _rightEyeP4 = enemyMappings[3].field;
        _nidhoggP4 = enemyMappings[4].field;
        _serCharibert = enemyMappings[5].field;
        _spear = enemyMappings[6].field;
        _bossP5 = enemyMappings[7].field;
        _nidhoggP6 = enemyMappings[8].field;
        _hraesvelgrP6 = enemyMappings[9].field;
        _bossP7 = enemyMappings[10].field;
    }

    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actor(PrimaryActor, Colors.Enemy, true);
        Arena.Actor(_bossP3);
        Arena.Actor(_leftEyeP4);
        Arena.Actor(_rightEyeP4);
        Arena.Actor(_nidhoggP4);
        Arena.Actor(_serCharibert);
        Arena.Actor(_spear);
        Arena.Actor(_bossP5);
        Arena.Actor(_nidhoggP6);
        Arena.Actor(_hraesvelgrP6);
        Arena.Actor(_bossP7);
    }
}
