namespace BossMod.Dawntrail.Raid.M2NHoneyB;

public enum OID : uint
{
    Boss = 0x422A, // R5.004, x1
    Helper = 0x233C, // R0.500, x28, 523 type
    Actor1ea1a1 = 0x1EA1A1, // R2.000, x1, EventObj type
    Actor1ebaa2 = 0x1EBAA2, // R0.500, x0 (spawn during fight), EventObj type
    Actor1ebaa3 = 0x1EBAA3, // R0.500, x0 (spawn during fight), EventObj type
    Groupbee = 0x422B, // R1.500, x0 (spawn during fight)
    PoisonCloud = 0x4230, // R1.000, x0 (spawn during fight)
    Sweetheart = 0x422C, // R1.000, x0 (spawn during fight)
}

public enum AID : uint
{
    AutoAttack = 37319, // Boss->player, no cast, single-target

    AlarmPheromones = 37245, // Boss->self, 3.0s cast, single-target

    BlindingLove1 = 37246, // Groupbee->self, 6.3+0.7s cast, single-target
    BlindingLove2 = 37247, // Groupbee->self, 6.3+0.7s cast, single-target
    BlindingLove3 = 39525, // Helper->self, 7.0s cast, range 50 width 8 rect
    BlindingLove4 = 39526, // Helper->self, 7.0s cast, range 50 width 8 rect

    BlowKiss = 37235, // Boss->self, 6.0s cast, range 40 120.000-degree cone
    CallMeHoney = 37220, // Boss->self, 5.0s cast, range 60 circle

    DropOfVenom1 = 37226, // Boss->self, 3.0s cast, single-target
    DropOfVenom2 = 37232, // Helper->players, 7.0s cast, range 6 circle

    Fracture = 37240, // Helper->self, 8.0s cast, range 4 circle

    Heartsick = 39821, // Helper->players, 7.0s cast, range 6 circle
    Heartsore = 37242, // Helper->player, 7.0s cast, range 6 circle

    HeartStruck1 = 37237, // Helper->location, 3.0s cast, range 4 circle
    HeartStruck2 = 37238, // Helper->location, 4.0s cast, range 6 circle
    HeartStruck3 = 37239, // Helper->location, 5.0s cast, range 10 circle

    HoneyBeeline1 = 37221, // Boss->self, 5.5+0.7s cast, single-target
    HoneyBeeline2 = 37227, // Boss->self, 5.5+0.7s cast, single-target
    HoneyBeeline3 = 39737, // Helper->self, 6.2s cast, range 60 width 14 rect
    HoneyBeeline4 = 39739, // Helper->self, 6.2s cast, range 60 width 14 rect

    HoneyBFinale = 37243, // Boss->self, 5.0s cast, range 60 circle

    HoneyBLive1 = 37234, // Boss->self, 2.0+5.0s cast, single-target
    HoneyBLive2 = 39550, // Helper->self, no cast, range 60 circle

    HoneyedBreeze1 = 37223, // Boss->self, 4.0+1.0s cast, single-target
    HoneyedBreeze2 = 37224, // Helper->self, no cast, range 40 ?-degree cone

    LoveMeTender = 37236, // Boss->self, 4.0s cast, single-target

    Loveseeker1 = 39530, // Boss->self, 3.0+1.0s cast, single-target
    Loveseeker2 = 39617, // Helper->self, 4.0s cast, range 10 circle

    SplashOfVenom1 = 37225, // Boss->self, 3.0s cast, single-target
    SplashOfVenom2 = 37231, // Helper->player, 7.0s cast, range 6 circle

    Splinter1 = 37230, // PoisonCloud->self, 4.0s cast, range 8 circle
    Splinter2 = 37244, // Sweetheart->player, no cast, single-target

    TemptingTwist1 = 37222, // Boss->self, 5.5+0.7s cast, single-target
    TemptingTwist2 = 37228, // Boss->self, 5.5+0.7s cast, single-target
    TemptingTwist3 = 39738, // Helper->self, 6.2s cast, range ?-30 donut
    TemptingTwist4 = 39740, // Helper->self, 6.2s cast, range ?-30 donut

    UnknownAbility1 = 37219, // Boss->location, no cast, single-target
    UnknownAbility2 = 37229, // PoisonCloud->location, no cast, single-target
}

public enum SID : uint
{
    Electrocution1 = 3073, // none->player, extra=0x0
    Electrocution2 = 3074, // none->player, extra=0x0
    FatalAttraction = 3920, // none->player, extra=0x2162
    HeadOverHeels = 3918, // none->player, extra=0x2D8
    HopelessDevotion = 3919, // none->player, extra=0x2D9
    Infatuated = 3917, // none->player, extra=0x2D7
    LovelyPoison = 3915, // Boss->Boss, extra=0x31B
    MagicVulnerabilityUp = 2941, // Helper->player, extra=0x0
    PoisonResistanceDownII = 3935, // Helper->player, extra=0x0
    SustainedDamage = 2935, // Helper->player, extra=0x0
    TopOfTheHive = 4143, // none->Boss, extra=0x0
    UnknownStatus1 = 2234, // none->Sweetheart, extra=0x19
    UnknownStatus2 = 3916, // none->player, extra=0x2D6
    VulnerabilityUp = 1789, // Sweetheart/Helper/PoisonCloud->player, extra=0x1/0x2/0x3/0x4/0x5
}

public enum IconID : uint
{
    HoneyedBreezeTB = 230, // player
    Icon317 = 317, // player
    Icon375 = 375, // player
    Icon515 = 515, // player
    Icon517 = 517, // player
}
