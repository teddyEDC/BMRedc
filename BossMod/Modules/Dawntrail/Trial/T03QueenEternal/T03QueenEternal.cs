namespace BossMod.Dawntrail.Trial.T03QueenEternal;

class T03QueenEternalStates : StateMachineBuilder
{
    public T03QueenEternalStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<LegitimateForce1>()
            .ActivateOnEnter<LegitimateForce2>()
            .ActivateOnEnter<LegitimateForce3>()
            .ActivateOnEnter<LegitimateForce4>()
            .ActivateOnEnter<WaltzOfTheRegalia2>()
            .ActivateOnEnter<ProsecutionOfWar>()
            .ActivateOnEnter<VirtualShiftArenaChange>()
            .ActivateOnEnter<VirtualShift1>()
            .ActivateOnEnter<VirtualShift2>()
            .ActivateOnEnter<VirtualShift3>()
            .ActivateOnEnter<Downburst2>()
            .ActivateOnEnter<BrutalCrown>()
            .ActivateOnEnter<PowerfulGust2>()
            .ActivateOnEnter<RoyalDomain>()
            .ActivateOnEnter<AbsoluteAuthority4>()
            .ActivateOnEnter<AuthoritysGaze>()
            .ActivateOnEnter<DynasticDiadem2>()
            .ActivateOnEnter<RoyalBanishment2>();
    }
}
class LegitimateForce1(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LegitimateForce1), new AOEShapeRect(60, 60, DirectionOffset: -90.Degrees()));
class LegitimateForce2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LegitimateForce2), new AOEShapeRect(60, 60, DirectionOffset: -90.Degrees()));
class LegitimateForce3(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LegitimateForce3), new AOEShapeRect(60, 60, DirectionOffset: 90.Degrees()));
class LegitimateForce4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LegitimateForce4), new AOEShapeRect(60, 60, DirectionOffset: 90.Degrees()));
class WaltzOfTheRegalia2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.WaltzOfTheRegalia2), new AOEShapeRect(14, 2));
class ProsecutionOfWar(BossModule module) : Components.SingleTargetCast(module, ActionID.MakeSpell(AID.ProsecutionOfWar));
class VirtualShift1(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.VirtualShift1));
class VirtualShift2(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.VirtualShift2));
class VirtualShift3(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.VirtualShift3));
class Downburst2(BossModule module) : Components.KnockbackFromCastTarget(module, ActionID.MakeSpell(AID.Downburst2), 10);
class BrutalCrown(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.BrutalCrown), new AOEShapeDonut(6, 60));
class PowerfulGust2(BossModule module) : Components.KnockbackFromCastTarget(module, ActionID.MakeSpell(AID.PowerfulGust2), 20, kind: Kind.DirForward);
class RoyalDomain(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.RoyalDomain));
class AbsoluteAuthority4(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.AbsoluteAuthority4), new AOEShapeCircle(8));
class AuthoritysGaze(BossModule module) : Components.GenericGaze(module)
{
    private DateTime _activation;
    private readonly List<Actor> _affected = [];

    public override IEnumerable<Eye> ActiveEyes(int slot, Actor actor)
    {
        foreach (var a in _affected)
            if (_affected.Count > 0 && WorldState.CurrentTime > _activation.AddSeconds(-10))
                yield return new(a.Position, _activation);
    }

    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID == SID.AuthoritysGaze)
        {
            _activation = status.ExpireAt;
            _affected.Add(actor);
        }
    }

    public override void OnStatusLose(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID == SID.AuthoritysGaze)
            _affected.Remove(actor);
    }
}
class DynasticDiadem2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.DynasticDiadem2), new AOEShapeDonut(10, 70));
class RoyalBanishment2(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.RoyalBanishment2), new AOEShapeCone(100, 15.Degrees()));

class VirtualShiftArenaChange(BossModule module) : Components.GenericAOEs(module)
{
    private AOEInstance? _aoe;
    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => Utils.ZeroOrOne(_aoe);

    public override void OnEventEnvControl(byte index, uint state)
    {
        if (state == 0x00020001 && index == 0x00) // x arena
        {
            Arena.Center = T03QueenEternal.XArenaCenter;
            Arena.Bounds = T03QueenEternal.XArena;
            _aoe = null; //
        }

        if (state == 0x00040004 && index == 0x01) // change back to normal arena
        {
            Arena.Center = T03QueenEternal.StartingArenaCenter;
            Arena.Bounds = T03QueenEternal.StartingBounds;
            _aoe = null; //
        }

        if (state == 0x00020001 && index == 0x02) // double rect arena, unsure how this phase should be implemented going forward given mechancics so leaving as starting arena
        {
            Arena.Center = T03QueenEternal.StartingArenaCenter;
            Arena.Bounds = T03QueenEternal.StartingBounds;
            _aoe = null; // Players are able to move freely with this arena, but likely will need to add persistent voidzone aoes
        }
    }
}

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "The Combat Reborn Team", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 984, NameID = 13029)]
public class T03QueenEternal(WorldState ws, Actor primary) : BossModule(ws, primary, StartingArenaCenter, StartingBounds)
{
    public static readonly WPos StartingArenaCenter = new(100, 100);
    public static readonly WPos XArenaCenter = new(100, 92.5f);
    public static readonly WPos WestRectArenaCenter = new(100, 94);
    public static readonly WPos EastRectArenaCenter = new(92, 94);

    public static readonly ArenaBoundsSquare StartingBounds = new(20);
    public static readonly ArenaBoundsComplex XArena = new([new Rectangle(new(100, 82.5f), 12.5f, 2.5f), new Rectangle(new(100, 102.5f), 12.5f, 2.5f), new Rectangle(new(100, 92.5f), 15, 2.5f, Rotation: 45.Degrees()), new Rectangle(new(100, 92.5f), 15, 2.5f, Rotation: -45.Degrees())]);
    public static readonly ArenaBoundsRect WestRectArena = new(4, 8);
    public static readonly ArenaBoundsRect EastRectArena = new(4, 8);
}
