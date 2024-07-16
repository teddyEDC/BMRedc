namespace BossMod.Dawntrail.Raid.M4NWickedThunder;

class WrathOfZeus(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.WrathOfZeus));
class SidewiseSpark1(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SidewiseSpark1), new AOEShapeCone(60, 90.Degrees()));
class SidewiseSpark2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SidewiseSpark2), new AOEShapeCone(60, 90.Degrees()));
class SidewiseSpark3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SidewiseSpark3), new AOEShapeCone(60, 90.Degrees()));
class SidewiseSpark4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SidewiseSpark4), new AOEShapeCone(60, 90.Degrees()));
class SidewiseSpark5(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SidewiseSpark5), new AOEShapeCone(60, 90.Degrees()));
class SidewiseSpark6(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.SidewiseSpark6), new AOEShapeCone(60, 90.Degrees()));
class StampedingThunder3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.StampedingThunder3), new AOEShapeRect(40, 15));
class Burst(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.Burst), new AOEShapeRect(40, 8));
class BewitchingFlight3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BewitchingFlight3), new AOEShapeRect(40, 2.5f));
class Thunderslam(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.Thunderslam), 5);
class UnknownWeaponskill7(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.UnknownWeaponskill7), 6);
class Thunderstorm(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.Thunderstorm), 6);

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "The Combat Reborn Team", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 991, NameID = 13057)]
public class M4NWickedThunder(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), new ArenaBoundsSquare(20));
