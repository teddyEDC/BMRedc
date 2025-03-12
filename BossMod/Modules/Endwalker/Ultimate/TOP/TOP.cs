namespace BossMod.Endwalker.Ultimate.TOP;

class SolarRayM(BossModule module) : Components.BaitAwayCast(module, ActionID.MakeSpell(AID.SolarRayM), new AOEShapeCircle(5f), true);
class SolarRayF(BossModule module) : Components.BaitAwayCast(module, ActionID.MakeSpell(AID.SolarRayF), new AOEShapeCircle(5f), true);
class P4BlueScreen(BossModule module) : Components.CastCounter(module, ActionID.MakeSpell(AID.BlueScreenAOE));
class P5BlindFaith(BossModule module) : Components.CastHint(module, ActionID.MakeSpell(AID.BlindFaithSuccess), "Intermission");

[ModuleInfo(BossModuleInfo.Maturity.Verified, GroupType = BossModuleInfo.GroupType.CFC, GroupID = 908, PlanLevel = 90)]
public class TOP(WorldState ws, Actor primary) : BossModule(ws, primary, new(100f, 100f), new ArenaBoundsCircle(20f))
{
    private Actor? _opticalUnit;
    private Actor? _omegaM;
    private Actor? _omegaF;
    private Actor? _bossP3;
    private Actor? _bossP5;
    private Actor? _bossP6;
    public Actor? BossP1() => PrimaryActor;
    public Actor? OpticalUnit() => _opticalUnit; // we use this to distinguish P1 wipe vs P1 kill - primary actor can be destroyed before P2 bosses spawn
    public Actor? BossP2M() => _omegaM;
    public Actor? BossP2F() => _omegaF;
    public Actor? BossP3() => _bossP3;
    public Actor? BossP5() => _bossP5;
    public Actor? BossP6() => _bossP6;

    protected override void UpdateModule()
    {
        // TODO: this is an ugly hack, think how multi-actor fights can be implemented without it...
        // the problem is that on wipe, any actor can be deleted and recreated in the same frame
        var enemyMappings = new (int phaseIndex, uint oid, Actor? field, bool allowLaterPhases)[]
        {
            (0, (uint)OID.OpticalUnit, _opticalUnit, false),
            (1, (uint)OID.OmegaM, _omegaM, false),
            (1, (uint)OID.OmegaF, _omegaF, false),
            (2, (uint)OID.BossP3, _bossP3, false),
            (4, (uint)OID.BossP5, _bossP5, false),
            (4, (uint)OID.BossP6, _bossP6, true),
        };

        for (var i = 0; i < 6; ++i)
        {
            var (phaseIndex, oid, field, allowLaterPhases) = enemyMappings[i];

            if (field == null && (allowLaterPhases ? StateMachine.ActivePhaseIndex >= phaseIndex : StateMachine.ActivePhaseIndex == phaseIndex))
            {
                var enemies = Enemies(oid);
                enemyMappings[i].field = enemies.Count != 0 ? enemies[0] : null;
            }
        }

        _opticalUnit = enemyMappings[0].field;
        _omegaM = enemyMappings[1].field;
        _omegaF = enemyMappings[2].field;
        _bossP3 = enemyMappings[3].field;
        _bossP5 = enemyMappings[4].field;
        _bossP6 = enemyMappings[5].field;
    }

    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actor(PrimaryActor);
        Arena.Actor(_omegaM);
        Arena.Actor(_omegaF);
        Arena.Actor(_bossP3);
        Arena.Actor(_bossP5);
        Arena.Actor(_bossP6);
    }
}
