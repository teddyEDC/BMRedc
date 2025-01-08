namespace BossMod.Stormblood.Foray.BaldesionsArsenal.BA1Owain;

class ElementalMagicks(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeCircle circle = new(13);
    public readonly List<AOEInstance> AOEs = new(5);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => AOEs;

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        void AddAOEs(SID sid)
        {
            var mundberg = Module.Enemies(OID.Munderg);
            var activation = Module.CastFinishAt(spell);
            for (var i = 0; i < mundberg.Count; ++i)
            {
                var spear = mundberg[i];
                if (spear.FindStatus(sid) != null)
                    AOEs.Add(new(circle, spear.Position, default, activation));
            }
            AOEs.Add(new(circle, spell.LocXZ, default, activation));
        }
        switch ((AID)spell.Action.ID)
        {
            case AID.ElementalMagicksFireBoss:
                AddAOEs(SID.SoulOfFire);
                break;
            case AID.ElementalMagicksIceBoss:
                AddAOEs(SID.SoulOfIce);
                break;
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (AOEs.Count != 0 && (AID)spell.Action.ID is AID.ElementalMagicksFireBoss or AID.ElementalMagicksFireSpears or AID.ElementalMagicksIceBoss or AID.ElementalMagicksIceSpears)
            AOEs.RemoveAt(0);
    }
}

class Thricecull(BossModule module) : Components.SingleTargetCast(module, ActionID.MakeSpell(AID.Thricecull));
class AcallamNaSenorach(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.AcallamNaSenorach));
class LegendaryImbas(BossModule module) : Components.RaidwideCast(module, ActionID.MakeSpell(AID.LegendaryImbas)); // applies dorito stacks, seems to get skipped if less than 4 people alive?
class Pitfall(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.Pitfall), 20);

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "The Combat Reborn Team (Malediktus)", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 639, NameID = 7970, PlanLevel = 70)]
public class BA1Owain(WorldState ws, Actor primary) : BossModule(ws, primary, arena.Center, arena)
{
    private static readonly ArenaBoundsComplex arena = new([new Polygon(new(128.98f, 748), 29.5f, 64)], [new Rectangle(new(129, 718), 20, 1.15f), new Rectangle(new(129, 778), 20, 1.48f)]);

    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actor(PrimaryActor);
        Arena.Actors(Enemies(OID.IvoryPalm));
    }

    protected override void CalculateModuleAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        for (var i = 0; i < hints.PotentialTargets.Count; ++i)
        {
            var e = hints.PotentialTargets[i];
            e.Priority = (OID)e.Actor.OID switch
            {
                OID.IvoryPalm => 1,
                _ => 0
            };
        }
    }
}
