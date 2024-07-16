namespace BossMod.Dawntrail.Raid.M4NWickedThunder;

public enum OID : uint
{
    Boss = 0x4263, // R4.900, x1
    Helper = 0x233C, // R0.500, x25, 523 type
    WickedThunder = 0x4569, // R1.000, x1
    WickedReplica = 0x4264, // R3.675-4.900, x16
    Actor1ea1a1 = 0x1EA1A1, // R2.000, x1, EventObj type
    Exit = 0x1E850B, // R0.500, x1, EventObj type
}

public enum AID : uint
{
    AutoAttack1 = 39146, // Boss->player, no cast, single-target
    AutoAttack2 = 36759, // Boss->player, no cast, single-target

    BewitchingFlight1 = 36324, // Boss/WickedReplica->self, 6.0+1.0s cast, single-target
    BewitchingFlight2 = 36326, // Boss/WickedReplica->self, 6.0+1.0s cast, single-target
    BewitchingFlight3 = 37560, // Helper->self, 7.0s cast, range 40 width 5 rect

    Burst = 37561, // Helper->self, 7.0s cast, range 40 width 16 rect
    FivefoldBlast = 39767, // Boss->self, 16.0s cast, single-target

    FourfoldBlast1 = 39759, // Boss->self, 13.5s cast, single-target
    FourfoldBlast2 = 39765, // Boss->self, 13.5s cast, single-target

    ShadowsSabbath1 = 38044, // Boss->self, 3.0s cast, single-target
    ShadowsSabbath2 = 39871, // Boss->self, 3.0s cast, single-target

    SidewiseSpark1 = 37564, // Boss->self, 7.0s cast, range 60 180.000-degree cone
    SidewiseSpark2 = 37565, // Boss->self, 7.0s cast, range 60 180.000-degree cone
    SidewiseSpark3 = 37566, // Boss->self, 7.0s cast, range 60 180.000-degree cone
    SidewiseSpark4 = 37567, // Boss->self, 7.0s cast, range 60 180.000-degree cone
    SidewiseSpark5 = 39429, // WickedReplica->self, 1.0s cast, range 60 180.000-degree cone
    SidewiseSpark6 = 39439, // WickedReplica->self, 1.0s cast, range 60 180.000-degree cone

    SoaringSoulpress1 = 37568, // Boss->self, 5.0s cast, single-target
    SoaringSoulpress2 = 37569, // Boss->players, no cast, range 6 circle

    StampedingThunder1 = 35334, // Boss->self, no cast, single-target
    StampedingThunder2 = 35335, // Helper->self, no cast, range 40 width 30 rect
    StampedingThunder3 = 36150, // Helper->self, 7.3s cast, range 40 width 30 rect

    ThreefoldBlast1 = 37549, // Boss->self, 11.0s cast, single-target
    ThreefoldBlast2 = 37552, // Boss->self, 11.0s cast, single-target

    Thunderslam = 37574, // Helper->location, 3.0s cast, range 5 circle
    Thunderstorm = 37573, // Helper->player, 8.0s cast, range 5 circle
    UnknownSpell = 37102, // WickedThunder->self, no cast, single-target

    UnknownWeaponskill1 = 37543, // Boss->self, no cast, single-target
    UnknownWeaponskill2 = 37544, // Boss->self, no cast, single-target
    UnknownWeaponskill3 = 37545, // Boss->self, no cast, single-target
    UnknownWeaponskill4 = 37546, // Boss->self, no cast, single-target
    UnknownWeaponskill5 = 37547, // Boss->self, no cast, single-target
    UnknownWeaponskill6 = 37548, // Boss->self, no cast, single-target
    UnknownWeaponskill7 = 37557, // Helper->location, 1.5s cast, range 6 circle
    UnknownWeaponskill8 = 37577, // Boss->location, no cast, single-target

    WickedBolt1 = 37570, // Boss->self, 4.0+1.0s cast, single-target
    WickedBolt2 = 37571, // Helper->players, no cast, range 5 circle

    WickedCannon1 = 20032, // Helper->self, no cast, range 40 width 10 rect
    WickedCannon2 = 37550, // Boss->self, no cast, range 40 width 10 rect
    WickedCannon3 = 37551, // Boss->self, no cast, range 40 width 10 rect
    WickedCannon4 = 39852, // Boss->self, no cast, range 40 width 10 rect
    WickedCannon5 = 39870, // Boss->self, no cast, range 40 width 10 rect

    WickedHypercannon1 = 37553, // Boss->self, 5.0s cast, single-target
    WickedHypercannon2 = 37554, // Boss->self, 5.0s cast, single-target
    WickedHypercannon3 = 37555, // Helper->self, no cast, range 40 width 10 rect

    WickedJolt = 37576, // Boss->self/player, 5.0s cast, range 60 width 5 rect

    WitchHunt1 = 37556, // Boss->self, 3.0s cast, single-target
    WitchHunt2 = 37558, // WickedReplica->location, no cast, range 6 circle

    WrathOfZeus = 37575, // Boss->self, 5.0s cast, range 60 circle
}

public enum SID : uint
{
    DirectionalDisregard = 3808, // none->Boss, extra=0x0
    LightningResistanceDownII = 2095, // Helper->player, extra=0x0
    SustainedDamage = 3288, // Helper/Boss->player, extra=0x0
    Unknown1 = 2056, // none->WickedReplica, extra=0x319/0x31A
    Unknown2 = 2970, // none->Boss, extra=0x2D4/0x2D3
    VulnerabilityUp = 1789, // Boss/Helper/WickedReplica->player, extra=0x1/0x2
}

public enum IconID : uint
{
    Icon161 = 161, // player
    Icon316 = 316, // player
    Icon345 = 345, // player
    Icon471 = 471, // player
}
