namespace BossMod.Dawntrail.Savage.M05SDancingGreen;

class WavelengthAlphaBeta(BossModule module) : BossComponent(module)
{
    private readonly (DateTime Expiration, Actor Actor, DateTime ExpirationReal)[] expirationBySlot = new (DateTime, Actor, DateTime)[8]; // first expiration is rounded to find matching partner more easily
    private int numCasts;

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        if (numCasts == 8)
        {
            ref readonly var player = ref expirationBySlot[slot];

            bool? inRisk = null;
            for (var i = 0; i < 8; ++i)
            {
                ref readonly var exp = ref expirationBySlot[i];
                if (exp == default || slot == i)
                    continue;
                var remaining = Math.Max(0d, (exp.ExpirationReal - WorldState.CurrentTime).TotalSeconds);
                var check = remaining < 5d;
                var partner = exp.Actor;
                if (exp.Expiration == player.Expiration)
                {
                    hints.Add($"Stack with: {partner.Name} in {remaining:f0}s", check);
                }
                else if (check)
                {
                    if (actor.Position.InCircle(partner.Position, 2f))
                        inRisk = true;
                }
            }
            if (inRisk != null)
                hints.Add($"GTFO from incorrect stacks!");
        }
    }

    public override void DrawArenaForeground(int pcSlot, Actor pc)
    {
        if (numCasts == 8)
        {
            ref readonly var player = ref expirationBySlot[pcSlot].Expiration;
            for (var i = 0; i < 8; ++i)
            {
                ref readonly var exp = ref expirationBySlot[i];
                if (exp == default || pcSlot == i)
                    continue;
                var remaining = Math.Max(0d, (exp.ExpirationReal - WorldState.CurrentTime).TotalSeconds) < 5d;
                var partner = exp.Actor;
                if (!remaining)
                    continue;
                if (exp.Expiration == player)
                {
                    Arena.AddCircle(partner.Position, 2f, Colors.Safe);
                }
                else
                {
                    Arena.AddCircle(partner.Position, 2f);
                }
            }
        }
    }
    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if (status.ID is (uint)SID.WavelengthAlpha or (uint)SID.WavelengthBeta)
        {
            var slot = WorldState.Party.FindSlot(actor.InstanceID);
            if (slot < 0)
                return;
            expirationBySlot[slot] = (status.ExpireAt.Round(TimeSpan.FromSeconds(2)), actor, status.ExpireAt);
        }
    }

    public override void OnStatusLose(Actor actor, ActorStatus status)
    {
        if (status.ID is (uint)SID.WavelengthAlpha or (uint)SID.WavelengthBeta)
        {
            var slot = WorldState.Party.FindSlot(actor.InstanceID);
            if (slot < 0)
                return;
            expirationBySlot[slot] = default;
        }
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (spell.Action.ID == (uint)AID.GetDownBait)
            ++numCasts;
    }
}
