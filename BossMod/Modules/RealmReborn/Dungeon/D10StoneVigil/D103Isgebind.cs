namespace BossMod.RealmReborn.Dungeon.D10StoneVigil.D103Isgebind;

public enum OID : uint
{
    Boss = 0x5AF, // x1
    Helper = 0x233C
}

public enum AID : uint
{
    AutoAttack = 870, // Boss->player, no cast

    RimeWreath = 1025, // Boss->self, 3.0s cast, raidwide
    FrostBreath = 1022, // Boss->self, 1.0s cast, range 27 120-degree cone cleave
    SheetOfIce = 1023, // Boss->location, 2.5s cast, range 5 aoe
    SheetOfIce2 = 1024, // Helper->location, 3.0s cast, range 5 aoe
    Cauterize = 1026, // Boss->self, 4.0s cast, range 48 width 20 rect aoe
    Touchdown = 1027 // Boss->self, no cast, range 5 aoe around center
}

class RimeWreath(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.RimeWreath));
class FrostBreath(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.FrostBreath), new AOEShapeCone(27f, 60f.Degrees()));
class FrostBreathCleave(BossModule module) : Components.Cleave(module, ActionID.MakeSpell(AID.FrostBreath), new AOEShapeCone(27f, 60f.Degrees()), activeWhileCasting: false);
class SheetOfIce(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.SheetOfIce), 5f);
class SheetOfIce2(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.SheetOfIce2), 5f);
class Cauterize(BossModule module) : Components.SimpleAOEs(module, ActionID.MakeSpell(AID.Cauterize), new AOEShapeRect(48f, 10f));

class Touchdown(BossModule module) : Components.GenericAOEs(module, ActionID.MakeSpell(AID.Touchdown))
{
    private readonly AOEShapeCircle _shape = new(5f);
    private bool _activation = false;  // Using a bool to track activation

    private const uint HelperOID = (uint)OID.Helper;

    // Counter to track how many times Cauterize has been cast
    private int _cauterizeCounter = 0;
    private bool _seenSheetOfIce2 = false;

    // Reset variables when the boss becomes targetable again
    public override void OnTargetable(Actor actor)
    {
        if (actor == Module.PrimaryActor)
        {
            // Reset activation and phase flags
            _activation = false;  // Reset to false when targetable
            _cauterizeCounter = 0;
            _seenSheetOfIce2 = false;
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        // Only track casts from the boss or helper
        if (caster.OID != Module.PrimaryActor.OID && caster.OID != HelperOID)
            return;

        var aid = (AID)spell.Action.ID;

        // Handle Cauterize casts and predict the Touchdown
        if (aid == AID.Cauterize)
        {
            bool phase2 = _seenSheetOfIce2;

            // Predict Touchdown if conditions for phase transitions and Cauterize casts are met
            if ((phase2 && _cauterizeCounter == 1) || (!phase2 && _cauterizeCounter == 0))
            {
                _activation = true;  // Set to true when prediction is made
            }

            // Increment the Cauterize counter after each cast
            _cauterizeCounter++;
        }

        // Track when the SheetOfIce2 cast happens to determine phase 2
        if (aid == AID.SheetOfIce2)
            _seenSheetOfIce2 = true;

        // Confirm Touchdown cast and reset activation time
        if (aid == AID.Touchdown)
        {
            _activation = true;  // Set to true when Touchdown is cast
        }
    }

    // Show touchdown AOE
    public override ReadOnlySpan<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_activation)
            return new AOEInstance[] { new(_shape, WPos.ClampToGrid(D103Isgebind.ArenaCenter)) };
        return [];
    }
}

class D103IsgebindStates : StateMachineBuilder
{
    public D103IsgebindStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<RimeWreath>()
            .ActivateOnEnter<FrostBreath>()
            .ActivateOnEnter<FrostBreathCleave>()
            .ActivateOnEnter<SheetOfIce>()
            .ActivateOnEnter<SheetOfIce2>()
            .ActivateOnEnter<Cauterize>()
            .ActivateOnEnter<Touchdown>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Contributed, Contributors = "Malediktus, Chuggalo", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 11, NameID = 1680)]
public class D103Isgebind(WorldState ws, Actor primary) : BossModule(ws, primary, arena.Center, arena)
{
    public static readonly WPos ArenaCenter = new(default, -248f);
    private static readonly Angle a45 = 45f.Degrees();
    private static readonly Square[] union = [new(ArenaCenter, 23.7f), new(new(-23, -259.4f), 1), new(new(3.4f, -271), 1), new(new(12.8f, -225), 1)];
    private static readonly Shape[] difference = [new Square(new(-24, -224), 5.75f, a45), new Square(new(24, -224), 5.75f, a45),
    new Square(new(-24, -272), 5.75f, a45), new Square(new(24, -272), 5.75f, a45), new Circle(new(-23.9f, -248), 4.5f),
    new Circle(new(23.9f, -248), 4.5f), new Square(new(-16.2f, -271.9f), 0.5f, a45), new Square(new(16.2f, -271.9f), 0.5f, a45), new Square(new(-16.2f, -224.1f), 0.5f, a45),
    new Square(new(16.2f, -224.1f), 0.5f, a45), new Square(new(-23.9f, -264.2f), 0.45f, a45), new Square(new(23.9f, -264.2f), 0.45f, a45), new Square(new(-23.9f, -231.8f), 0.45f, a45),
    new Square(new(23.9f, -231.8f), 0.45f, a45), new Rectangle(new(0, -224), 3.8f, 0.4f), new Rectangle(new(2.1f, -224.3f), 1.5f, 0.4f), new Rectangle(new(-2.1f, -224.3f), 1.5f, 0.4f),
    new Square(new(-8, -224), 0.45f), new Square(new(7.9f, -224), 0.45f), new Square(new(7.9f, -272), 0.45f), new Square(new(0, -272), 0.45f),
    new Square(new(-7.9f, -272), 0.45f), new Square(new(24, -240), 0.45f), new Square(new(24, -256), 0.45f), new Square(new(-24, -240), 0.45f), new Square(new(-24, -256), 0.45f)];
    private static readonly Shape[] union2 = [new Circle(new(-19.5f, -228.4f), 3), new Circle(new(19.5f, -228.4f), 3), new Circle(new(-19.5f, -267.6f), 3),
    new Circle(new(19.5f, -267.6f), 3), new Circle(new(-21.3f, -243.6f), 1.5f), new Circle(new(-21.3f, -252.4f), 1.5f), new Square(new(-23, -243.4f), 0.7f),
    new Square(new(-23, -252.6f), 0.7f), new Circle(new(21.3f, -243.6f), 1.5f), new Circle(new(21.3f, -252.4f), 1.5f), new Square(new(23, -243.4f), 0.7f),
    new Square(new(23, -252.6f), 0.7f)];
    public static readonly ArenaBoundsComplex arena = new(union, difference, union2);
}
