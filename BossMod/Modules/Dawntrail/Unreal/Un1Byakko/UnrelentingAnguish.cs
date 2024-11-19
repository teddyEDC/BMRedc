namespace BossMod.Dawntrail.Unreal.Un1Byakko;

class UnrelentingAnguish(BossModule module) : Components.PersistentVoidzone(module, 2, m => m.Enemies(OID.AratamaForce).Where(z => !z.IsDead), 2);

// TODO: show something
class OminousWind(BossModule module) : BossComponent(module)
{
    public BitMask Targets;

    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID == SID.OminousWind)
            Targets.Set(Raid.FindSlot(actor.InstanceID));
    }

    public override void OnStatusLose(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID == SID.OminousWind)
            Targets.Clear(Raid.FindSlot(actor.InstanceID));
    }
}

class GaleForce(BossModule module) : Components.BaitAwayIcon(module, new AOEShapeCircle(6), (uint)IconID.Bombogenesis, ActionID.MakeSpell(AID.GaleForce), 8.1f, true);

class VacuumClaw(BossModule module) : Components.PersistentVoidzone(module, 12, m => m.Enemies(OID.VacuumClaw).Where(z => !z.IsDead));
class VacuumBlade(BossModule module) : Components.CastCounter(module, ActionID.MakeSpell(AID.VacuumBlade));
