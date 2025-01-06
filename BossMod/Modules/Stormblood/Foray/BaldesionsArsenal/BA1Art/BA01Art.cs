namespace BossMod.Stormblood.Foray.BaldesionsArsenal.BA01Art;

public enum OID : uint
{
    Boss = 0x265A, // R2.7
    Orlasrach = 0x265B, // R2.7
    Owain = 0x2662, // R2.7
    ShadowLinksHelper = 0x1EA1A1, // R2.0 (if pos -134.917, 750.44)
    Helper = 0x265C
}

public enum AID : uint
{
    AutoAttack = 14678, // Boss->player, no cast, single-target

    Thricecull = 14644, // Boss->player, 4.0s cast, single-target, tankbuster
    Legendspinner = 14633, // Boss->self, 4.5s cast, range 7-22 donut
    Legendcarver = 14632, // Boss->self, 4.5s cast, range 15 circle
    AcallamNaSenorach = 14645, // Boss->self, 4.0s cast, range 60 circle
    AcallamNaSenorachArt = 14628, // Boss->self, 7.0s cast, range 80 circle, enrage if Owain side does not get pulled, Owain teleports to Art
    AcallamNaSenorachOwain = 14629, // Owain->self, 7.0s cast, range 80 circle
    Mythcall = 14631, // Boss->self, 2.0s cast, single-target
    Mythspinner = 14635, // Orlasrach->self, no cast, range 7-22 donut
    Mythcarver = 14634, // Orlasrach->self, no cast, range 15 circle
    LegendaryGeas = 14642, // Boss->location, 4.0s cast, range 8 circle
    DefilersDeserts = 14643, // Helper->self, 3.5s cast, range 35+R width 8 rect
    GloryUnearthedFirst = 14636, // Helper->location, 5.0s cast, range 10 circle
    GloryUnearthedRest = 14637, // Helper->location, no cast, range 10 circle
    Pitfall = 14639, // Boss->location, 5.0s cast, range 80 circle, damage fall off AOE
    PiercingDarkVisual = 14640, // Boss->self, 2.5s cast, single-target
    PiercingDark = 14641, // Helper->player, 5.0s cast, range 6 circle, spread
}

public enum IconID : uint
{
    ChasingAOE = 92, // player->self
}

class LegendMythSpinnerCarver(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeCircle circle = new(15);
    private static readonly AOEShapeDonut donut = new(7, 22);
    private readonly List<AOEInstance> _aoes = new(5);
    private bool mythcall;

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _aoes;

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        void AddAOE(AOEShape shape) => _aoes.Add(new(shape, caster.Position, default, Module.CastFinishAt(spell)));
        void AddAOEs(AOEShape shape)
        {
            var orlasrach = Module.Enemies(OID.Orlasrach);
            for (var i = 0; i < orlasrach.Count; ++i)
                _aoes.Add(new(shape, orlasrach[i].Position, default, Module.CastFinishAt(spell, 2.6f)));
            mythcall = false;
        }
        switch ((AID)spell.Action.ID)
        {
            case AID.Legendcarver:
                AddAOE(circle);
                if (mythcall)
                    AddAOEs(circle);
                break;
            case AID.Legendspinner:
                AddAOE(donut);
                if (mythcall)
                    AddAOEs(donut);
                break;
            case AID.Mythcall:
                mythcall = true;
                break;
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (_aoes.Count != 0 && (AID)spell.Action.ID is AID.Legendcarver or AID.Legendspinner or AID.Mythcarver or AID.Mythspinner)
            _aoes.RemoveAt(0);
    }
}

class Thricecull(BossModule module) : Components.SingleTargetCast(module, ActionID.MakeSpell(AID.Thricecull));
class AcallamNaSenorach(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.AcallamNaSenorach));
class DefilersDeserts(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.DefilersDeserts), new AOEShapeRect(35.5f, 4));
class Pitfall(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.Pitfall), 20);
class LegendaryGeasAOE(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.LegendaryGeas), 8);

class DefilersDesertsPredict(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeCross cross = new(35.5f, 4);
    private readonly List<AOEInstance> _aoes = new(2);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _aoes;

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        void AddAOE(Angle angle) => _aoes.Add(new(cross, spell.LocXZ, angle, Module.CastFinishAt(spell, 6.9f)));
        if ((AID)spell.Action.ID == AID.LegendaryGeas)
        {
            AddAOE(45.Degrees());
            AddAOE(default);
        }
        else if ((AID)spell.Action.ID == AID.DefilersDeserts)
            _aoes.Clear();
    }
}

class LegendaryGeasStay(BossModule module) : Components.StayMove(module)
{
    public override void OnActorEAnim(Actor actor, uint state)
    {
        if ((OID)actor.OID == OID.ShadowLinksHelper && actor.Position.AlmostEqual(new(-134.917f, 750.44f), 1))
        {
            if (state == 0x00010002)
                Array.Fill(PlayerStates, new(Requirement.Stay2, WorldState.CurrentTime, 1));
            else if (state == 0x00040008)
                Array.Clear(PlayerStates);
        }
    }
}

class GloryUnearthed(BossModule module) : Components.OpenWorldChasingAOEs(module, new AOEShapeCircle(10), ActionID.MakeSpell(AID.GloryUnearthedFirst), ActionID.MakeSpell(AID.GloryUnearthedRest), 6.5f, 1.5f, 5, true, (uint)IconID.ChasingAOE);
class PiercingDark(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.PiercingDark), 6);
class Mythcounter(BossModule module) : Components.CastCounterMulti(module, [ActionID.MakeSpell(AID.Mythspinner), ActionID.MakeSpell(AID.Mythcarver)]);

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "The Combat Reborn Team (Malediktus)", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 639, NameID = 7968, PlanLevel = 70)]
public class BA01Art(WorldState ws, Actor primary) : BossModule(ws, primary, arena.Center, arena)
{
    private static readonly ArenaBoundsComplex arena = new([new Polygon(new(-128.98f, 748), 29.5f, 64)], [new Rectangle(new(-129, 718), 20, 1.15f), new Rectangle(new(-129, 778), 20, 1.48f)]);
}
