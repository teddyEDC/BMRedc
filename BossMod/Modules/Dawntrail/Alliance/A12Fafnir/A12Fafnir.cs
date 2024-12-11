namespace BossMod.Dawntrail.Alliance.A12Fafnir;

class ShudderingEarth(BossModule module) : Components.CastCounter(module, ActionID.MakeSpell(AID.ShudderingEarth));
class Darter(BossModule module) : Components.Adds(module, (uint)OID.Darter);
class Venom(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.Venom), new AOEShapeCone(30, 60.Degrees()));
class AbsoluteTerror(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.AbsoluteTerrorAOE), new AOEShapeRect(70, 10));
class BalefulBreath(BossModule module) : Components.LineStack(module, (uint)IconID.BalefulBreath, ActionID.MakeSpell(AID.BalefulBreathAOERest), 8.2f, 70, 3, PartyState.MaxAllianceSize, PartyState.MaxAllianceSize, 3, false);
class SharpSpike(BossModule module) : Components.BaitAwayIcon(module, new AOEShapeCircle(4), (uint)IconID.SharpSpike, ActionID.MakeSpell(AID.SharpSpikeAOE), 6.2f, true);

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "The Combat Reborn Team (Malediktus, LTS)", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 1015, NameID = 13662, SortOrder = 4)]
public class A12Fafnir(WorldState ws, Actor primary) : BossModule(ws, primary, ArenaCenter, new ArenaBoundsCircle(34.5f))
{
    public static readonly WPos ArenaCenter = new(-500, 600);
    public static readonly ArenaBoundsCircle DefaultBounds = new(30);
    public static readonly ArenaBoundsCircle FireArena = new(16);
}
