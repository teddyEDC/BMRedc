namespace BossMod.Dawntrail.Savage.M08SHowlingBlade;

class TwofoldTempestVoidzone(BossModule module) : Components.Voidzone(module, 7f, GetVoidzones)
{
    private static Actor[] GetVoidzones(BossModule module)
    {
        var enemies = module.Enemies((uint)OID.WindVoidzone);
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

class TwofoldTempestTetherAOE(BossModule module) : Components.TankbusterTether(module, ActionID.MakeSpell(AID.TwofoldTempestCircle), (uint)TetherID.TwofoldTempest, 6f);
class TwofoldTempestTetherVoidzone(BossModule module) : Components.TankbusterTether(module, ActionID.MakeSpell(AID.TwofoldTempestCircle), (uint)TetherID.TwofoldTempest, 9f, false);

class TwofoldTempestRect(BossModule module) : Components.GenericBaitStack(module, ActionID.MakeSpell(AID.UltraviolentRay), onlyShowOutlines: true)
{
    private static readonly AOEShapeRect rect = new(40f, 8f);
    private DateTime activation;

    public override void OnTethered(Actor source, ActorTetherInfo tether)
    {
        if (tether.ID == (uint)TetherID.TwofoldTempest)
        {
            if (activation == default)
                activation = WorldState.FutureTime(7.1d);
            var target = WorldState.Actors.Find(tether.Target);
            if (target is Actor t)
                CurrentBaits.Add(new(Module.Enemies((uint)OID.BossP2)[0], t, rect, WorldState.FutureTime(7.1d)));
        }
    }

    public override void OnUntethered(Actor source, ActorTetherInfo tether)
    {
        if (tether.ID == (uint)TetherID.TwofoldTempest)
        {
            CurrentBaits.Clear();
        }
    }

    public override void Update()
    {
        var baits = CollectionsMarshal.AsSpan(CurrentBaits);
        var len = baits.Length;
        for (var i = 0; i < len; ++i)
        {
            ref var b = ref baits[i];
            for (var j = 0; j < 5; ++j)
            {
                var center = ArenaChanges.EndArenaPlatforms[j].Center;
                if (b.Target.Position.InCircle(center, 8f))
                {
                    b.CustomRotation = new(ArenaChanges.PlatformAngles[j]);
                    break;
                }
            }
        }
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        var baits = CollectionsMarshal.AsSpan(CurrentBaits);
        var len = baits.Length;
        if (len == 0)
            return;
        var playerPlatform = 0;
        for (var i = 0; i < 5; ++i)
        {
            if (actor.Position.InCircle(ArenaChanges.EndArenaPlatforms[i].Center, 8f))
            {
                playerPlatform = i;
                break;
            }
        }

        ref readonly var b = ref baits[0];
        var countP = 0;
        var party = Raid.WithoutSlot(false, true, true);
        var pLen = party.Length;

        for (var i = 0; i < pLen; ++i)
        {
            ref readonly var p = ref party[i];
            if (p == actor)
                continue;
            if (p.Position.InCircle(ArenaChanges.EndArenaPlatforms[playerPlatform].Center, 8f))
            {
                if (++countP == 2)
                {
                    hints.Add("More than 2 players on your platform!");
                }
            }
        }
    }
}
