using Dalamud.Logging.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace BossMod.Dawntrail.Dungeon.D06Alexandria.D061AntivirusX;

public enum OID : uint
{
    Boss = 0x4173, // R8.000, x1
    Helper = 0x233C, // R0.500, x20, 523 type
    ElectricCharge = 0x18D6, // R0.500, x6
    InterferonC = 0x4175, // R1.000, x0 (spawn during fight)
    InterferonR = 0x4174, // R1.000, x0 (spawn during fight)
}

public enum AID : uint
{
    AutoAttack = 36388, // Boss->player, no cast, single-target

    ImmuneResponse1 = 36378, // Boss->self, 5.0s cast, single-target
    ImmuneResponse2 = 36379, // Helper->self, 6.0s cast, range 40 ?-degree cone // Frontal AOE cone
    ImmuneResponse3 = 36380, // Boss->self, 5.0s cast, single-target
    ImmuneResponse4 = 36381, // Helper->self, 6.0s cast, range 40 ?-degree cone // Side and back AOE cone

    PathocrossPurge = 36383, // InterferonC->self, 1.0s cast, range 40 width 6 cross
    PathocircuitPurge = 36382, // InterferonR->self, 1.0s cast, range ?-40 donut

    Quarantine1 = 36384, // Boss->self, 3.0s cast, single-target
    Quarantine2 = 36386, // Helper->players, no cast, range 6 circle

    Disinfection = 36385, // Helper->player, no cast, range 6 circle
    Cytolysis = 36387, // Boss->self, 5.0s cast, range 40 circle
}

public enum SID : uint
{
    VulnerabilityUp = 1789, // Helper/InterferonC/InterferonR->player, extra=0x1/0x2/0x3
    Electrocution1 = 3073, // none->player, extra=0x0
    Electrocution2 = 3074, // none->player, extra=0x0
}

public enum IconID : uint
{
    Spreadmarker = 344, // player
    Stackmarker = 62, // player
}
class ImmuneResponse2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.ImmuneResponse2), new AOEShapeCone(40, 60.Degrees()));
class ImmuneResponse4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.ImmuneResponse4), new AOEShapeCone(40, 120.Degrees()));
class PathocrossPurge(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.PathocrossPurge), new AOEShapeCross(40, 3));
class PathocircuitPurge(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.PathocircuitPurge), new AOEShapeDonut(6, 40));

class PathoPurge(BossModule module) : Components.GenericAOEs(module)
{
    private List<AOEInstance> aoes = [];

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => aoes;

    public void AddAoE(bool cross, WPos pos, float delay)
    {
        var instance = new AOEInstance(cross ?
                                              new AOEShapeCross(40, 3.5f) :
                                              new AOEShapeDonut(4, 40),
                                          pos, Activation: Module.WorldState.FutureTime(delay));
        aoes.Add(instance);

        if (!cross)
        {
            donutActive = true;
            donutActiveInstance = instance;
        }
    }

    public List<Actor> actors = [];
    public int actorCount;

    private bool donutActive;
    private AOEInstance donutActiveInstance;
    private int donutActorIndex;

    public override void OnActorCreated(Actor actor)
    {
        base.OnActorCreated(actor);
        var actorOid = (OID)actor.OID;
        if (actorOid is OID.InterferonC or OID.InterferonR)
        {
            actors.Add(actor);
            //Service.Log("added actor");

            if(actors.Count == 1)
            {
                Module.FindComponent<ImmuneResponse2>()!.Risky = false;
                Module.FindComponent<ImmuneResponse4>()!.Risky = false;

                bool cross = actorOid == OID.InterferonC;

                AddAoE(cross, actor.Position, 13f);

                if (!cross)
                {
                    donutActorIndex = 0;
                }

                //Service.Log($"Added first {(cross ? "cross" : "donut")} AoE");
            } else if (!donutActive && actorOid == OID.InterferonR)
            {
                donutActive = true;
                AddAoE(false, actor.Position, 12f + (actors.Count - 1) * 4);
            }
        }
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        base.OnCastStarted(caster, spell);
        var actionId = (AID)spell.Action.ID;
        
        if (actors.Contains(caster) && (actionId is AID.PathocircuitPurge or AID.PathocrossPurge))
        {
            actors.Remove(caster);
            actorCount += 1;

            Actor? next = actors.FirstOrDefault();

            if (next != null)
            {
                var nextCross = (OID)next.OID == OID.InterferonC;
                if (nextCross)
                {
                    AddAoE(true, next.Position, 3);


                    Module.FindComponent<ImmuneResponse2>()!.Risky = true;
                    Module.FindComponent<ImmuneResponse4>()!.Risky = true;
                }

                switch (actorCount)
                {
                    case 1:
                        if (donutActive && donutActorIndex == 2)
                        {
                            donutActiveInstance.Risky = false;
                        }
                        goto case 3;
                    case 3:
                        if (!nextCross)
                        {
                            Module.FindComponent<ImmuneResponse2>()!.Risky = true;
                            Module.FindComponent<ImmuneResponse4>()!.Risky = true;
                        }
                        break;
                    default:
                        if (!nextCross)
                        {
                            Module.FindComponent<ImmuneResponse2>()!.Risky = false;
                            Module.FindComponent<ImmuneResponse4>()!.Risky = false;
                        }
                        break;
                }
            }
            else
            {
                donutActive = false;
                donutActorIndex = 0;
            }
        }
    }
    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        var actionId = (AID) spell.Action.ID;
        if (aoes.Count > 0 && actionId is AID.PathocircuitPurge or AID.PathocrossPurge)
        {
            var cross = actionId == AID.PathocrossPurge;
            aoes.Remove(aoes.FirstOrDefault(aoe => cross  && aoe.Shape is AOEShapeCross ||
                                                   !cross && aoe.Shape is AOEShapeDonut));
            //Service.Log($"removed {(cross ? "cross" : "donut")} AoE");

            bool reactivate = false;

            if (!cross)
            {
                int nextDonut = actors.FindIndex(0, a => (OID)a.OID == OID.InterferonR);
                if (nextDonut >= 0)
                {
                    Actor next = actors[nextDonut];
                    AddAoE(false, next.Position, 1 + nextDonut * 5);
                    //Service.Log("added Donut AoE");
                }
                else
                    reactivate = true;
            }

            if(actorCount == 1 && donutActive && donutActorIndex == 2)
            {
                donutActiveInstance.Risky = true;
            }

            if (reactivate)
            {
                //Service.Log("reactivate modules");
                actorCount = 0;
                Module.FindComponent<ImmuneResponse2>()!.Risky = true;
                Module.FindComponent<ImmuneResponse4>()!.Risky = true;
            }
        }
    }
}

class Quarantine2(BossModule module) : Components.StackWithIcon(module, (uint)IconID.Stackmarker, ActionID.MakeSpell(AID.Quarantine2), 6, 5, 4);

class QuarantineTest(BossModule module) : Components.StackWithIcon(module, (uint)IconID.Stackmarker, ActionID.MakeSpell(AID.Quarantine2), 6, 5, 4)
{
    public override void OnEventIcon(Actor actor, uint iconID)
    {
        //Service.Log($"OnEventIcon: {module.PrimaryActor.OID} {actor.OID} {actor.Type} | {iconID}");
        if(actor.OID != 0)
            base.OnEventIcon(actor, iconID);
    }
}

class Disinfection(BossModule module) : Components.SpreadFromIcon(module, (uint)IconID.Spreadmarker, ActionID.MakeSpell(AID.Disinfection), 7, 6);

class Cytolysis(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.Cytolysis));

class D061AntivirusXStates : StateMachineBuilder
{
    public D061AntivirusXStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<ImmuneResponse2>()
            .ActivateOnEnter<ImmuneResponse4>()
            .ActivateOnEnter<PathoPurge>()
            //.ActivateOnEnter<PathocrossPurge>()
            //.ActivateOnEnter<PathocircuitPurge>()
            .ActivateOnEnter<QuarantineTest>()
            .ActivateOnEnter<Disinfection>()
            .ActivateOnEnter<Cytolysis>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "The Combat Reborn Team", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 827, NameID = 12844)]
public class D061AntivirusX(WorldState ws, Actor primary) : BossModule(ws, primary, new WPos(852, 822f), new ArenaBoundsRect(20, 14));
