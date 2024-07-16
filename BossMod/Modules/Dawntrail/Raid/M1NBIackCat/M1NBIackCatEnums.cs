namespace BossMod.Dawntrail.Raid.M1NBlackCat;

public enum OID : uint
{
    Boss = 0x429C, // R3.993, x1
    Helper = 0x233C, // R0.500, x40, 523 type
    CopyCat = 0x429D, // R3.993, x1
    Actor1ea1a1 = 0x1EA1A1, // R2.000, x1, EventObj type
    UnknownActor = 0x429E, // R1.000, x0 (spawn during fight)
}

public enum AID : uint
{
    AutoAttack = 39151, // Boss->player, no cast, single-target
    Teleport = 37640, // Boss->location, no cast, single-target

    BiscuitMaker = 37706, // Boss->player, 5.0s cast, single-target

    BlackCatCrossing1 = 37647, // Boss->self, 6.0s cast, single-target
    BlackCatCrossing2 = 37648, // Boss->self, 1.0s cast, single-target
    BlackCatCrossing3 = 37649, // Helper->self, 7.0s cast, range 60 45.000-degree cone
    BlackCatCrossing4 = 37650, // Helper->self, 7.0s cast, range 60 45.000-degree cone

    BloodyScratch = 37696, // Boss->self, 5.0s cast, range 60 circle

    Clawful1 = 37692, // Boss->self, 4.0+1.0s cast, single-target
    Clawful2 = 37693, // Helper->players, 5.0s cast, range 5 circle

    Copycat = 37656, // Boss->self, 3.0s cast, single-target
    ElevateAndEviscerate = 37655, // Boss/CopyCat->player, 8.0s cast, single-target

    GrimalkinGale1 = 37694, // Boss->self, no cast, single-target
    GrimalkinGale2 = 37695, // Helper->player, 5.0s cast, range 5 circle

    Impact = 39250, // Helper->self, no cast, range 10 width 10 rect

    LeapingBlackCatCrossing1 = 37673, // Boss->self, 7.0s cast, single-target
    LeapingBlackCatCrossing2 = 37674, // Boss->self, no cast, single-target
    LeapingBlackCatCrossing3 = 37675, // Boss->self, no cast, single-target
    LeapingBlackCatCrossing4 = 37676, // Helper->self, 1.0s cast, range 60 45.000-degree cone
    LeapingBlackCatCrossing5 = 37677, // Helper->self, 3.0s cast, range 60 45.000-degree cone
    LeapingBlackCatCrossing6 = 38928, // Boss->self, 7.0s cast, single-target

    LeapingOneTwoPaw1 = 37663, // Boss->self, 7.0s cast, single-target
    LeapingOneTwoPaw2 = 37664, // Boss->self, 7.0s cast, single-target
    LeapingOneTwoPaw3 = 37665, // Boss->self, 7.0s cast, single-target
    LeapingOneTwoPaw4 = 37666, // Boss->self, 7.0s cast, single-target
    LeapingOneTwoPaw5 = 37667, // Boss->self, no cast, single-target
    LeapingOneTwoPaw6 = 37668, // Helper->self, 0.8s cast, range 60 180.000-degree cone
    LeapingOneTwoPaw7 = 37669, // Helper->self, 2.8s cast, range 60 180.000-degree cone
    LeapingOneTwoPaw8 = 37670, // Boss->self, no cast, single-target
    LeapingOneTwoPaw9 = 37671, // Helper->self, 2.8s cast, range 60 180.000-degree cone
    LeapingOneTwoPaw10 = 37672, // Helper->self, 0.8s cast, range 60 180.000-degree cone

    Mouser1 = 37651, // Boss->self, 9.0s cast, single-target
    Mouser2 = 37654, // Helper->location, no cast, single-target
    Mouser3 = 38053, // Helper->self, no cast, range 10 width 10 rect

    OneTwoPaw1 = 37641, // Boss->self, 6.0s cast, single-target
    OneTwoPaw2 = 37642, // Helper->self, 6.8s cast, range 60 180.000-degree cone
    OneTwoPaw3 = 37643, // Helper->self, 9.8s cast, range 60 180.000-degree cone
    OneTwoPaw4 = 37644, // Boss->self, 6.0s cast, single-target
    OneTwoPaw5 = 37645, // Helper->self, 9.8s cast, range 60 180.000-degree cone
    OneTwoPaw6 = 37646, // Helper->self, 6.8s cast, range 60 180.000-degree cone

    Overshadow1 = 37657, // Boss->player, 5.0s cast, single-target
    Overshadow2 = 37658, // Boss->players, no cast, range 60 width 5 rect

    PredaceousPounce1 = 37680, // Boss/CopyCat->location, no cast, single-target
    PredaceousPounce2 = 37681, // Helper->self, 1.5s cast, range 11 circle
    PredaceousPounce3 = 39268, // Helper->location, 1.0s cast, width 6 rect charge
    PredaceousPounce4 = 39634, // Boss/CopyCat->location, 13.0s cast, single-target
    PredaceousPounce5 = 39702, // Helper->location, 13.5s cast, width 6 rect charge
    PredaceousPounce6 = 39703, // Helper->self, 14.0s cast, range 11 circle

    Shockwave1 = 37661, // Boss->self, 6.0+1.0s cast, single-target
    Shockwave2 = 37662, // Helper->self, 7.0s cast, range 30 circle

    UnknownAbility = 26708, // Helper->player, no cast, single-target

    UnknownWeaponskill1 = 37652, // Boss->self, no cast, single-target
    UnknownWeaponskill2 = 37653, // Helper->self, 1.0s cast, range 10 width 10 rect
    UnknownWeaponskill3 = 37682, // Helper->location, 2.0s cast, width 6 rect charge
    UnknownWeaponskill4 = 37683, // Helper->self, 3.0s cast, range 11 circle
    UnknownWeaponskill5 = 37684, // Helper->location, 4.0s cast, width 6 rect charge
    UnknownWeaponskill6 = 37685, // Helper->self, 5.0s cast, range 11 circle
    UnknownWeaponskill7 = 37686, // Helper->location, 6.0s cast, width 6 rect charge
    UnknownWeaponskill8 = 37687, // Helper->self, 7.0s cast, range 11 circle
    UnknownWeaponskill9 = 37688, // Helper->location, 8.0s cast, width 6 rect charge
    UnknownWeaponskill10 = 37689, // Helper->self, 9.0s cast, range 11 circle
    UnknownWeaponskill11 = 37690, // Helper->location, 10.0s cast, width 6 rect charge
    UnknownWeaponskill12 = 37691, // Helper->self, 11.0s cast, range 11 circle
    UnknownWeaponskill13 = 39275, // Helper->self, 1.0s cast, range 10 width 10 rect
    UnknownWeaponskill14 = 39630, // Helper->location, 12.0s cast, width 6 rect charge
    UnknownWeaponskill15 = 39631, // Helper->self, 13.0s cast, range 11 circle
}

public enum SID : uint
{
    SustainedDamage = 2935, // Helper->player, extra=0x0
    UnknownStatus1 = 2056, // none->Boss, extra=0x2CB/0x307
    UnknownStatus2 = 2193, // none->Boss, extra=0x2CC
    VulnerabilityUp = 1789, // Helper->player, extra=0x1
    Stun = 2656, // Boss/CopyCat->player, extra=0x0
}

public enum IconID : uint
{
    Icon218 = 218, // player
    Icon93 = 93, // player
    Icon376 = 376, // player
}

public enum TetherID : uint
{
    Tether267 = 267, // Boss/CopyCat->player
    Tether268 = 268, // Boss/CopyCat->player
    Tether12 = 12, // Boss->UnknownActor
}
