namespace BossMod.Dawntrail.Dungeon.D08TheStrayboroughDeadwalk.D082JackInThePot;

public enum OID : uint
{
    Boss = 0x41CA, // R4.16
    Actor1e8536 = 0x1E8536, // R2.0
    SpectralSamovar = 0x41CB, // R2.88
    StrayPhantagenitrix = 0x41D2, // R2.1
    StrayRascal = 0x4139, // R1.3
    TeacupHelper = 0x41D5, // R1.5
    Helper = 0x233C
}

public enum AID : uint
{
    AutoAttack = 37169, // Boss->player, no cast, single-target

    TroublingTeacups = 36716, // Boss->self, 3.0s cast, single-target // Spawns teacups
    TeaAwhirl = 36717, // Boss->self, 6.0s cast, single-target // Ghost(s) tether teacup and enters, teacups spin then possesed teacup explodes in AOE
    TricksomeTreat = 36720, // StrayPhantagenitrix->self, 3.0s cast, range 19 circle // TeaAwhirl AOE

    ToilingTeapots = 36722, // Boss->self, 3.0s cast, single-target // Spawns 13 teacups

    Puppet = 36721, // StrayPhantagenitrix->location, 4.0s cast, single-target
    PipingPour = 36723, // SpectralSamovar->location, 2.0s cast, single-target // Spreading AOE

    MadTeaParty = 36724, // Helper->self, no cast, range 0 circle // DOT applied to players in puddles

    LastDrop = 36726, // Boss->player, 5.0s cast, single-target // Tankbuster

    SordidSteam = 36725, // Boss->self, 5.0s cast, range 40 circle // Raidwide
}

public enum SID : uint
{
    AreaOfInfluenceUp = 1909, // none->Helper, extra=0x1/0x2/0x3/0x4
    Bleeding = 3078, // Helper->player, extra=0x0
    VulnerabilityUp = 1789, // StrayPhantagenitrix->player, extra=0x1/0x2
}

public enum IconID : uint
{
    Tankbuster = 218, // player
}

public enum TetherID : uint
{
    CupTether = 276, // UnknownActor->StrayPhantagenitrix
}

class TricksomeTreat(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.TricksomeTreat), new AOEShapeCircle(19));
class SordidSteam(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.SordidSteam));
class LastDrop(BossModule module) : Components.SingleTargetCast(module, ActionID.MakeSpell(AID.LastDrop));

class D082JackInThePotStates : StateMachineBuilder
{
    public D082JackInThePotStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<TricksomeTreat>()
            .ActivateOnEnter<SordidSteam>()
            .ActivateOnEnter<LastDrop>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "The Combat Reborn Team", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 981, NameID = 12760)]
public class D082JackInThePot(WorldState ws, Actor primary) : BossModule(ws, primary, new(17, -170), new ArenaBoundsCircle(20));
