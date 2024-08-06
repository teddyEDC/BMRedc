using BossMod;
using BossMod.Components;

namespace BossModReborn.Stormblood.Dungeon.D01SirensongSea.D012TheGovernor;

public enum OID : uint
{
    GenActor1e8f2f = 0x1E8F2F,  // R0.500, x?, EventObj type
    GenActor1ea2f0 = 0x1EA2F0,  // R2.000, x?, EventObj type
    GenActor1ea2fd = 0x1EA2FD,  // R2.000, x?, EventObj type
    Gen = 0x18D6,     // R0.500, x?
    GenActor1ea2f9 = 0x1EA2F9,  // R2.000, x?, EventObj type
    GenActor1e8fb8 = 0x1E8FB8,  // R2.000, x?, EventObj type
    GenActor1ea2fb = 0x1EA2FB,  // R2.000, x?, EventObj type
    GenActor1ea2fa = 0x1EA2FA, // R2.000, x?, EventObj type
    Boss = 0x1AFC,   // R3.500, x?
    GenTheGroveller = 0x1AFD,   // R1.500, x?
    Gen2 = 0xF9747,  // R0.500, x?, EventNpc type
    GenActor1e8536 = 0x1E8536, // R2.000, x?, EventObj type
    GenShortcut = 0x1E873C, // R0.500, x?, EventObj type
    GenActor1ea2f1 = 0x1EA2F1, // R2.000, x?, EventObj type
}

public enum AID : uint
{
    ShadowFlow = 8030,
    Cone = 8031,
    EnterNight = 8032
}

class Tether(BossModule module) : GenericAOEs(module)
{
    private AOEInstance aoe = new(new AOEShapeCircle(module.Arena.Bounds.Radius - 2), module.Center);
    private bool active;

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (active)
        {
            yield return aoe;
        }
    }

    public override void OnTethered(Actor source, ActorTetherInfo tether)
    {
        if (tether.ID == 61 && tether.Target == Module.WorldState.Party.Player()?.InstanceID)
        {
            active = true;
        }
    }

    public override void OnUntethered(Actor source, ActorTetherInfo tether)
    {
        if (tether.ID == 61)
        {
            active = false;
        }
    }
}


class ShadowFlow(BossModule module) : GenericAOEs(module)
{
    public List<AOEInstance> aoes = [];
    public List<Actor> Grovellers = [];

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        aoes.RemoveAll(aoe => aoe.Activation < Module.WorldState.CurrentTime);

        foreach (AOEInstance aoe in aoes)
        {
            yield return aoe;
        }
        foreach (Actor groveller in Grovellers)
        {
            yield return new AOEInstance(new AOEShapeCircle(7f), groveller.Position);
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID == AID.Cone)
        {
            aoes.Add(new AOEInstance(new AOEShapeCone(30f, 25.Degrees()), Module.Center, caster.Rotation, WorldState.FutureTime(10)));
        }
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if((AID)spell.Action.ID == AID.ShadowFlow)
        {
            aoes.Add(new AOEInstance(new AOEShapeCircle(7f), Module.Center, Activation: Module.CastFinishAt(spell, 10f)));
        }
    }

    public override void OnActorCreated(Actor actor)
    {
        if((OID)actor.OID == OID.GenTheGroveller)
        {
            Grovellers.Add(actor);
        }
    }

    public override void OnActorDestroyed(Actor actor)
    {
        if ((OID)actor.OID == OID.GenTheGroveller)
        {
            Grovellers.Remove(actor);
        }
    }
}


class DO12TheGovernorStates : StateMachineBuilder
{
    public DO12TheGovernorStates(BossModule module) : base(module)
    {
        TrivialPhase().
            ActivateOnEnter<ShadowFlow>().
            ActivateOnEnter<Tether>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Contributed, Contributors = "erdelf", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 238, NameID = 6072)]
public class DO12TheGovernor : BossModule
{
    public DO12TheGovernor(WorldState ws, Actor primary) : base(ws, primary, new WPos(-8, 79), new ArenaBoundsCircle(20))
    {
    }

    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actors(Enemies(OID.Boss));
        Arena.Actors(Enemies(OID.GenTheGroveller));
    }
}
