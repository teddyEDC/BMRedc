namespace BossMod.Heavensward.DeepDungeon.PalaceOfTheDead.DD30Ningishzida;

public enum OID : uint
{
    Boss = 0x16AC, // R4.8
    FireVoidzone = 0x1E8D9B, // R0.5
    IceVoidzone = 0x1E8D9C // R0.5
}

public enum AID : uint
{
    AutoAttack = 6501, // Boss->player, no cast, range 6+R ?-degree cone

    Dissever = 6426, // Boss->self, no cast, range 6+R 90-degree cone
    BallOfFire = 6427, // Boss->location, no cast, range 6 circle
    BallOfIce = 6428, // Boss->location, no cast, range 6 circle
    FearItself = 6429 // Boss->self, 6.0s cast, range 54+R circle
}

class Dissever(BossModule module) : Components.Cleave(module, ActionID.MakeSpell(AID.Dissever), new AOEShapeCone(10.8f, 45f.Degrees()), activeWhileCasting: false);

abstract class Voidzones(BossModule module, AID aid, uint oid) : Components.PersistentVoidzoneAtCastTarget(module, 6, ActionID.MakeSpell(aid), m => GetVoidzones(m, oid), 2.1f)
{
    private static Actor[] GetVoidzones(BossModule module, uint oid)
    {
        var enemies = module.Enemies(oid);
        var count = enemies.Count;
        if (count == 0)
            return [];

        var voidzones = new Actor[count];
        var index = 0;
        for (var i = 0; i < count; ++i)
        {
            var z = enemies[i];
            if (z.EventState != 7)
                voidzones[index++] = z;
        }
        return voidzones[..index];
    }
}
class BallofFire(BossModule module) : Voidzones(module, AID.BallOfFire, (uint)OID.FireVoidzone);
class BallofIce(BossModule module) : Voidzones(module, AID.BallOfIce, (uint)OID.IceVoidzone);

class FearItself(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.FearItself), new AOEShapeDonut(5f, 50f));

class Hints(BossModule module) : BossComponent(module)
{
    // arena is like a weird octagon and the boss also doesn't cast from the center
    private static readonly WPos FearCastSource = new(-300f, -236f);
    public int NumCasts;

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (spell.Action.ID is (uint)AID.BallOfFire or (uint)AID.BallOfIce or (uint)AID.FearItself)
            ++NumCasts;

        if (NumCasts >= 5)
        {
            NumCasts = 0;
        }
    }

    public override void AddGlobalHints(GlobalHints hints)
    {
        if (NumCasts < 4)
            hints.Add($"Bait the boss away from the middle of the arena. \n{Module.PrimaryActor.Name} will cast x2 Fire Puddles & x2 Ice Puddles. \nAfter the 4th puddle is dropped, run to the middle.");
        if (NumCasts >= 4)
            hints.Add($"Run to the middle of the arena! \n{Module.PrimaryActor.Name} is about to cast a donut AOE!");
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (NumCasts < 4)
            hints.AddForbiddenZone(ShapeDistance.Circle(FearCastSource, 11), WorldState.FutureTime(10d));
        else
            hints.AddForbiddenZone(ShapeDistance.InvertedCircle(FearCastSource, 5), WorldState.FutureTime(10d));
    }
}

class DD30NingishzidaStates : StateMachineBuilder
{
    public DD30NingishzidaStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<Dissever>()
            .ActivateOnEnter<BallofFire>()
            .ActivateOnEnter<BallofIce>()
            .ActivateOnEnter<FearItself>()
            .ActivateOnEnter<Hints>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Contributed, Contributors = "LegendofIceman", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 176, NameID = 5012)]
public class DD30Ningishzida(WorldState ws, Actor primary) : BossModule(ws, primary, new(-300f, -237f), new ArenaBoundsCircle(24f));
