using BossMod.Shadowbringers.Quest.SorrowOfWerlyt.SleepNowInSapphire.P1GuidanceSystem;

namespace BossMod.Shadowbringers.Quest.SorrowOfWerlyt.SleepNowInSapphire.P2SapphireWeapon;

public enum OID : uint
{
    Boss = 0x2DFA,
    Helper = 0x233C,
}

public enum AID : uint
{
    TailSwing = 20326, // Boss->self, 4.0s cast, range 46 circle
    OptimizedJudgment = 20325, // Boss->self, 4.0s cast, range 21-60 donut
    MagitekSpread = 20336, // RegulasImage->self, 5.0s cast, range 43 240-degree cone
    SideraysRight = 20329, // Helper->self, 8.0s cast, range 128 90-degree cone
    SideraysLeft = 21021, // Helper->self, 8.0s cast, range 128 90-degree cone
    SapphireRay = 20327, // Boss->self, 8.0s cast, range 120 width 40 rect
    MagitekRay = 20332, // 2DFC->self, 3.0s cast, range 100 width 6 rect
    ServantRoar = 20339, // 2DFD->self, 2.5s cast, range 100 width 8 rect
}

public enum SID : uint
{
    Invincibility = 775, // none->Boss, extra=0x0
}

class MagitekRay(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.MagitekRay), new AOEShapeRect(100, 3));
class ServantRoar(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.ServantRoar), new AOEShapeRect(100, 4));
class TailSwing(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.TailSwing), new AOEShapeCircle(46));
class OptimizedJudgment(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.OptimizedJudgment), new AOEShapeDonut(21, 60));
class MagitekSpread(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.MagitekSpread), new AOEShapeCone(43, 120.Degrees()));
class SapphireRay(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.SapphireRay), new AOEShapeRect(120, 20));

abstract class Siderays(BossModule module, AID aid) : Components.SimpleAOEs(module, ActionID.MakeSpell(aid), new AOEShapeCone(128, 45.Degrees()));
class SideraysLeft(BossModule module) : Siderays(module, AID.SideraysLeft);
class SideraysRight(BossModule module) : Siderays(module, AID.SideraysRight);

class TheSapphireWeaponStates : StateMachineBuilder
{
    public TheSapphireWeaponStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<TailSwing>()
            .ActivateOnEnter<OptimizedJudgment>()
            .ActivateOnEnter<MagitekSpread>()
            .ActivateOnEnter<SideraysLeft>()
            .ActivateOnEnter<SideraysRight>()
            .ActivateOnEnter<SapphireRay>()
            .ActivateOnEnter<MagitekRay>()
            .ActivateOnEnter<ServantRoar>()
            .ActivateOnEnter<GWarrior>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Contributed, GroupType = BossModuleInfo.GroupType.Quest, GroupID = 69431, NameID = 9458)]
public class TheSapphireWeapon(WorldState ws, Actor primary) : BossModule(ws, primary, new(-15, 610), new ArenaBoundsSquare(60))
{
    protected override void DrawEnemies(int pcSlot, Actor pc) => Arena.Actors(WorldState.Actors.Where(x => !x.IsAlly));

    protected override void CalculateModuleAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        for (var i = 0; i < hints.PotentialTargets.Count; ++i)
        {
            var h = hints.PotentialTargets[i];
            h.Priority = h.Actor.FindStatus(SID.Invincibility) == null ? 1 : 0;
        }
    }
}

