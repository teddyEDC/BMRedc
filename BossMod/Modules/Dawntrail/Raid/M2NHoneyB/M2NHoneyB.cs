namespace BossMod.Dawntrail.Raid.M2NHoneyB;

class CallMeHoney(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.CallMeHoney));

class TemptingTwist3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.TemptingTwist3), new AOEShapeDonut(6, 30));
class TemptingTwist4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.TemptingTwist4), new AOEShapeDonut(6, 30));

class HoneyBeeline3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.HoneyBeeline3), new AOEShapeRect(60, 7, 60));
class HoneyBeeline4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.HoneyBeeline4), new AOEShapeRect(60, 7, 60));

class HoneyedBreeze(BossModule module) : Components.GenericBaitAway(module, ActionID.MakeSpell(AID.HoneyedBreeze1))
{
    private static readonly AOEShapeCone _shape = new(40, 10.Degrees()); // TODO: verify angle

    public override void OnEventIcon(Actor actor, uint iconID)
    {
        if (iconID == (uint)IconID.HoneyedBreezeTB)
            CurrentBaits.Add(new(Module.PrimaryActor, actor, _shape));
    }
}
class HoneyBLive1(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.HoneyBLive1));
class Heartsore(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.Heartsore), 6);
class Fracture(BossModule module) : Components.CastTowers(module, ActionID.MakeSpell(AID.Fracture), 4, 1, 1);
class Loveseeker2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.Loveseeker2), new AOEShapeCircle(10));
class BlowKiss(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlowKiss), new AOEShapeCone(40, 60.Degrees()));
class HoneyBFinale(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.HoneyBFinale));
class DropOfVenom2(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.DropOfVenom2), 6);
class BlindingLove3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlindingLove3), new AOEShapeRect(50, 4));
class BlindingLove4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BlindingLove4), new AOEShapeRect(50, 4));
class HeartStruck1(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.HeartStruck1), 4);
class HeartStruck2(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.HeartStruck2), 6);
class HeartStruck3(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.HeartStruck3), 10);

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "The Combat Reborn Team", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 987, NameID = 12685)]
public class M2NHoneyB(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), new ArenaBoundsSquare(20));
