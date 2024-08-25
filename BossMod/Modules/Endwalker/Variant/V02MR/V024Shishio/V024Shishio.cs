namespace BossMod.Endwalker.Variant.V02MR.V024Shishio;

class NoblePursuit(BossModule module) : Components.ChargeAOEs(module, ActionID.MakeSpell(AID.NoblePursuit), 6);
class Enkyo(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.Enkyo));

class CloudToCloud1(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.CloudToCloud1), new AOEShapeRect(100, 1))
{
    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        var aoes = ActiveCasters.Select((c, index) =>
            new AOEInstance(Shape, c.Position, c.CastInfo!.Rotation, Module.CastFinishAt(c.CastInfo),
            index < 6 ? Colors.Danger : Colors.AOE));

        return aoes;
    }
}

class CloudToCloud2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.CloudToCloud2), new AOEShapeRect(100, 3))
{
    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        var aoes = ActiveCasters.Select((c, index) =>
            new AOEInstance(Shape, c.Position, c.CastInfo!.Rotation, Module.CastFinishAt(c.CastInfo),
            index < 4 ? Colors.Danger : Colors.AOE));

        return aoes;
    }
}

class CloudToCloud3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.CloudToCloud3), new AOEShapeRect(100, 6))
{
    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        var aoes = ActiveCasters.Select((c, index) =>
            new AOEInstance(Shape, c.Position, c.CastInfo!.Rotation, Module.CastFinishAt(c.CastInfo),
            index < 2 ? Colors.Danger : Colors.AOE));

        return aoes;
    }
}

class ThunderOnefold(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.ThunderOnefold), 6);
class ThunderTwofold(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.ThunderTwofold), 6);
class ThunderThreefold(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.ThunderThreefold), 6);

class SplittingCry(BossModule module) : Components.BaitAwayIcon(module, new AOEShapeRect(60, 7), (uint)IconID.Tankbuster, ActionID.MakeSpell(AID.SplittingCry), 5)
{
    public override void AddGlobalHints(GlobalHints hints)
    {
        if (CurrentBaits.Count > 0)
            hints.Add("Tankbuster cleave");
    }
}

class ThunderVortex(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.ThunderVortex), new AOEShapeDonut(8, 30));
class UnsagelySpin(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.UnsagelySpin), new AOEShapeCircle(6));
class Rush(BossModule module) : Components.ChargeAOEs(module, ActionID.MakeSpell(AID.Rush), 4);
class Vasoconstrictor(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.Vasoconstrictor), 5);
class Yoki(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.Yoki), new AOEShapeCircle(6));
class RightSwipe(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.RightSwipe), new AOEShapeCone(40, 90.Degrees()));
class LeftSwipe(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LeftSwipe), new AOEShapeCone(40, 90.Degrees()));

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "The Combat Reborn Team (Malediktus, LTS)", GroupType = BossModuleInfo.GroupType.CFC, Category = BossModuleInfo.Category.Criterion, GroupID = 945, NameID = 12428, SortOrder = 5)]
public class V024Shishio(WorldState ws, Actor primary) : BossModule(ws, primary, ArenaCenter, NormalBounds)
{
    public static readonly WPos ArenaCenter = new(-40, -300);
    public static readonly ArenaBoundsSquare NormalBounds = new(19.5f);
    public static readonly ArenaBoundsComplex CircleBounds = new([new Circle(ArenaCenter, 20)], [new Rectangle(ArenaCenter + new WDir(-20, 0), 20, 0.5f, 90.Degrees()),
    new Rectangle(ArenaCenter + new WDir(20, 0), 20, 0.5f, 90.Degrees()), new Rectangle(ArenaCenter + new WDir(0, 20), 20, 0.5f), new Rectangle(ArenaCenter + new WDir(0, -20), 20, 0.5f)]);
}
