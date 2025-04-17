namespace BossMod.Dawntrail.Extreme.Ex4Zelenia;

class SpecterOfTheLost(BossModule module) : Components.TankbusterTether(module, ActionID.MakeSpell(AID.SpecterOfTheLost), (uint)TetherID.SpecterOfTheLost, new AOEShapeCone(48f, 22.5f.Degrees()), 7.8d);
class AlexandrianThunderIIISpread(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.AlexandrianThunderIIISpread), 4f);
class AlexandrianBanishII(BossModule module) : Components.StackWithIcon(module, (uint)IconID.AlexandrianBanishII, ActionID.MakeSpell(AID.AlexandrianBanishII), 4f, 5.8f, 4, 4);
class AlexandrianThunderIIIAOE(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.AlexandrianThunderIIIAOE), 4f);
class HolyHazard(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.HolyHazard), new AOEShapeCone(24f, 60f.Degrees()), 2);

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "The Combat Reborn Team (Malediktus)", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1031, NameID = 13861, PlanLevel = 100)]
public class Ex4Zelenia(WorldState ws, Actor primary) : BossModule(ws, primary, arenaCenter, DefaultArena)
{
    private static readonly WPos arenaCenter = new(100f, 100f);
    public static readonly ArenaBoundsComplex DefaultArena = new([new Polygon(arenaCenter, 16f, 64)]);
    public static readonly ArenaBoundsComplex DonutArena = new([new DonutV(arenaCenter, 2f, 16f, 64)]);
}