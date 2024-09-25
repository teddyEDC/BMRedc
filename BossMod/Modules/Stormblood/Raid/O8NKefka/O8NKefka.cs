namespace BossMod.Stormblood.Raid.O8NKefka;

public enum OID : uint
{
    Boss = 0x2153, // R2.700, x1
    GravenImage = 0x18D6, // R0.500, x39, mixed types
}

public enum AID : uint
{
    AutoAttack = 872, // Boss->player, no cast, single-target

    Hyperdrive = 10542, // Boss->player, 4.0s cast, single-target

    BlizzardBlitz1 = 10515, // GravenImage->self, 5.0s cast, range 10 circle // fake 
    BlizzardBlitz2 = 10516, // GravenImage->self, 5.0s cast, range -40 donut
    BlizzardBlitz3 = 10517, // GravenImage->self, 5.0s cast, range 40 circle // fake
    BlizzardBlitz4 = 10518, // GravenImage->self, 5.0s cast, range 10 circle
    BlizzardBlitz5 = 10519, // Boss->self, 5.0s cast, single-target // Cast indicator
    BlizzardBlitz6 = 10520, // GravenImage->self, 5.0s cast, range 10 circle // Real out
    BlizzardBlitz7 = 10521, // GravenImage->self, 5.0s cast, range -40 donut

    ThrummingThunder1 = 10522, // Boss->self, 5.0s cast, single-target // Cast indicator
    ThrummingThunder2 = 10523, // GravenImage->self, 5.0s cast, range 40+R width 10 rect // fake
    ThrummingThunder3 = 10524, // GravenImage->self, 5.0s cast, range 40+R width 10 rect
    ThrummingThunder4 = 10525, // GravenImage->self, 5.0s cast, range 40+R width 10 rect // Real line 2 and 4

    FlagrantFire1 = 10526, // Boss->self, 4.2s cast, single-target
    FlagrantFire2 = 10527, // GravenImage->players, 5.2s cast, range 5 circle
    FlagrantFire3 = 10528, // GravenImage->players, 5.2s cast, range 6 circle

    TimelyTeleport1 = 10529, // Boss->self, 4.0s cast, single-target
    TimelyTeleport2 = 10530, // GravenImage->self, 4.0s cast, range 6 circle
    RevoltingRuin = 10531, // Boss->self, no cast, range 100+R ?-degree cone
    AeroAssault = 10532, // Boss->self, 3.0s cast, range 100 circle
    GravenImage = 10533, // Boss->self, 5.0s cast, single-target
    Shockwave = 10535, // GravenImage->self, 13.0s cast, range 100+R width 40 rect
    WaveCannon = 10536, // GravenImage->self, 3.0s cast, range 70+R width 6 rect

    GravitationalWave = 10537, // GravenImage->self, 6.0s cast, range 100+R ?-degree cone west half cleave
    IntemperateWill = 10538, // GravenImage->self, 6.0s cast, range 100+R ?-degree cone east half cleave
    AveMaria = 10539, // GravenImage->self, 6.0s cast, range 100 circle // reverse gaze, must look at tower
    IndolentWill = 10540, // GravenImage->self, 6.0s cast, range 100 circle // gaze

    UltimaUpsurge = 10541, // Boss->self, 4.0s cast, range 100 circle
}

public enum SID : uint
{
    JestersTruths = 1487, // none->GravenImage/Boss, extra=0x0
    JestersAntics = 1486, // none->GravenImage/Boss, extra=0x0
    VulnerabilityUp = 202, // GravenImage/Boss->player, extra=0x1/0x2
}
public enum IconID : uint
{
    Spreadmarker = 23, // player
    Stackmarker = 62, // player
}

class Hyperdrive(BossModule module) : Components.SingleTargetCast(module, ActionID.MakeSpell(AID.Hyperdrive));
class BlizzardBlitz1(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlizzardBlitz1), new AOEShapeCircle(10));
class BlizzardBlitz2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlizzardBlitz2), new AOEShapeDonut(10, 40));
class BlizzardBlitz3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlizzardBlitz3), new AOEShapeDonut(10, 40));
class BlizzardBlitz4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlizzardBlitz4), new AOEShapeCircle(10));
class BlizzardBlitz6(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlizzardBlitz6), new AOEShapeCircle(10));
class BlizzardBlitz7(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlizzardBlitz7), new AOEShapeDonut(10, 40));

class FlagrantFire2(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.FlagrantFire2), 5);
class FlagrantFire3(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.FlagrantFire3), 6);

class ThrummingThunder2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.ThrummingThunder2), new AOEShapeRect(40, 5));
class ThrummingThunder3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.ThrummingThunder3), new AOEShapeRect(40, 5));
class ThrummingThunder4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.ThrummingThunder4), new AOEShapeRect(40, 5));

class UltimaUpsurge(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.UltimaUpsurge));

class TimelyTeleport2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.TimelyTeleport2), new AOEShapeCircle(6));
class AeroAssault(BossModule module) : Components.KnockbackFromCastTarget(module, ActionID.MakeSpell(AID.AeroAssault), 10, kind: Kind.AwayFromOrigin);
class Shockwave(BossModule module) : Components.KnockbackFromCastTarget(module, ActionID.MakeSpell(AID.Shockwave), 15, kind: Kind.DirForward);

class WaveCannon(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.WaveCannon), new AOEShapeRect(70, 3));

class GravitationalWave(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.GravitationalWave), new AOEShapeCone(100, 180.Degrees()));
class IntemperateWill(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.IntemperateWill), new AOEShapeCone(100, 180.Degrees()));

class AveMaria(BossModule module) : Components.CastGaze(module, ActionID.MakeSpell(AID.AveMaria), true);
class IndolentWill(BossModule module) : Components.CastGaze(module, ActionID.MakeSpell(AID.IndolentWill));

class O8NKefkaStates : StateMachineBuilder
{
    public O8NKefkaStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<Hyperdrive>()
            .ActivateOnEnter<BlizzardBlitz2>()
            .ActivateOnEnter<BlizzardBlitz4>()
            .ActivateOnEnter<BlizzardBlitz6>()
            .ActivateOnEnter<BlizzardBlitz7>()
            .ActivateOnEnter<FlagrantFire2>()
            .ActivateOnEnter<FlagrantFire3>()
            .ActivateOnEnter<ThrummingThunder3>()
            .ActivateOnEnter<ThrummingThunder4>()
            .ActivateOnEnter<UltimaUpsurge>()
            .ActivateOnEnter<TimelyTeleport2>()
            .ActivateOnEnter<AeroAssault>()
            .ActivateOnEnter<Shockwave>()
            .ActivateOnEnter<WaveCannon>()
            .ActivateOnEnter<GravitationalWave>()
            .ActivateOnEnter<IntemperateWill>()
            .ActivateOnEnter<AveMaria>()
            .ActivateOnEnter<IndolentWill>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Contributed, Contributors = "The Combat Reborn Team (LTS)", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 289, NameID = 7131)]
public class O8NKefka(WorldState ws, Actor primary) : BossModule(ws, primary, new(0, 0), new ArenaBoundsCircle(20));
