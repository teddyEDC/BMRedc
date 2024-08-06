using BossMod;
using BossMod.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossModReborn.Stormblood.Dungeon.D01SirensongSea.D013Lorelei;

public enum OID : uint
{
    Boss = 0x1AFE,   // R3.360, x?
    GenExit = 0x1E850B, // R0.500, x?, EventObj type
    GenActor1e8f2f = 0x1E8F2F, // R0.500, x?, EventObj type
    GenActor1ea2f6 = 0x1EA2F6, // R2.000, x?, EventObj type
    GenActor1e8fb8 = 0x1E8FB8, // R2.000, x?, EventObj type
    GenActor1ea2ff = 0x1EA2FF, // R2.000, x?, EventObj type
    GenActor1ea2f7 = 0x1EA2F7, // R2.000, x?, EventObj type
    GenActor1ea300 = 0x1EA300, // R0.500, x?, EventObj type
}

public enum AID : uint
{
    VoidWater = 8040
}

class VoidWater(BossModule module) : LocationTargetedAOEs(module, ActionID.MakeSpell(AID.VoidWater), 10);

class Puddles(BossModule module) : GenericAOEs(module)
{
    private readonly List<AOEInstance> aoes = [];

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => aoes;

    public override void OnActorCreated(Actor actor)
    {
        if((OID)actor.OID == OID.GenActor1ea300)
        {
            aoes.Add(new AOEInstance(new AOEShapeCircle(7f), actor.Position));
        }
    }

    public override void OnActorDestroyed(Actor actor)
    {
        if ((OID)actor.OID == OID.GenActor1ea300)
        {
            aoes.Remove(aoes.First());
        }
    }
}

class D013LoreleiStates : StateMachineBuilder
{
    public D013LoreleiStates(BossModule module) : base(module)
    {
        TrivialPhase().
            ActivateOnEnter<Puddles>().
            ActivateOnEnter<StayInBounds>().
            ActivateOnEnter<VoidWater>();
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Contributed, Contributors = "erdelf", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 238, NameID = 6074)]
public class D013Lorelei(WorldState ws, Actor primary) : BossModule(ws, primary, new WPos(-44.564f, 465.154f), new ArenaBoundsCircle(15))
{
    protected override void DrawEnemies(int pcSlot, Actor pc) => Arena.Actors(Enemies(OID.Boss));
}
