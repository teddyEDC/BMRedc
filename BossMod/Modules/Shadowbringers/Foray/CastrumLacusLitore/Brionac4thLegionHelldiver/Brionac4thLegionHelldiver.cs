using static BossMod.Shadowbringers.Foray.CastrumLacusLitore.CLL1Brionac4thLegionHelldiver.CLL1Brionac4thLegionHelldiver;

namespace BossMod.Shadowbringers.Foray.CastrumLacusLitore.CLL1Brionac4thLegionHelldiver;

class ElectricAnvil(BossModule module) : Components.SingleTargetCast(module, (uint)AID.ElectricAnvil)
{
    private bool top;

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (ArenaTop.Contains(actor.Position - ArenaCenterTop))
        {
            top = true;
        }
        else
        {
            top = false;
            base.AddAIHints(slot, actor, assignment, hints);
        }
    }

    public override void AddGlobalHints(GlobalHints hints)
    {
        if (top)
        {
            base.AddGlobalHints(hints);
        }
    }
}

class MagitekMissiles(BossModule module) : Components.SingleTargetCast(module, (uint)AID.MagitekMissiles)
{
    private bool bottom;

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (ArenaTop.Contains(actor.Position - ArenaCenterBottom))
        {
            bottom = true;
        }
        else
        {
            bottom = false;
            base.AddAIHints(slot, actor, assignment, hints);
        }
    }

    public override void AddGlobalHints(GlobalHints hints)
    {
        if (bottom)
        {
            base.AddGlobalHints(hints);
        }
    }
}

class MRVMissile(BossModule module) : Components.RaidwideCast(module, (uint)AID.MRVMissile)
{
    private bool bottom;

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (ArenaTop.Contains(actor.Position - ArenaCenterTop))
        {
            bottom = true;
        }
        else
        {
            bottom = false;
            base.AddAIHints(slot, actor, assignment, hints);
        }
    }

    public override void AddGlobalHints(GlobalHints hints)
    {
        if (bottom)
        {
            base.AddGlobalHints(hints);
        }
    }
}

class LightningShower(BossModule module) : Components.RaidwideCast(module, (uint)AID.LightningShower)
{
    private bool top;

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        if (ArenaTop.Contains(actor.Position - ArenaCenterTop))
        {
            top = true;
        }
        else
        {
            top = false;
            base.AddAIHints(slot, actor, assignment, hints);
        }
    }

    public override void AddGlobalHints(GlobalHints hints)
    {
        if (top)
        {
            base.AddGlobalHints(hints);
        }
    }
}

class FalseThunder(BossModule module) : Components.SimpleAOEGroupsByTimewindow(module, [(uint)AID.FalseThunder1, (uint)AID.FalseThunder2], new AOEShapeCone(47f, 65f.Degrees()))
{
    public override ReadOnlySpan<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (ArenaTop.Contains(actor.Position - ArenaCenterTop))
            return base.ActiveAOEs(slot, actor);
        else
            return [];
    }
}

class Voltstream(BossModule module) : Components.SimpleAOEs(module, (uint)AID.Voltstream, new AOEShapeRect(40f, 5f), 3)
{
    public override ReadOnlySpan<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (ArenaTop.Contains(actor.Position - ArenaCenterTop))
            return base.ActiveAOEs(slot, actor);
        else
            return [];
    }
}

class SurfaceMissile(BossModule module) : Components.SimpleAOEs(module, (uint)AID.SurfaceMissile, 6f)
{
    public override ReadOnlySpan<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (ArenaTop.Contains(actor.Position - ArenaCenterBottom))
            return base.ActiveAOEs(slot, actor);
        else
            return [];
    }
}

class CommandSuppressiveFormation(BossModule module) : Components.ChargeAOEs(module, (uint)AID.CommandSuppressiveFormation, 3f)
{
    public override ReadOnlySpan<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (ArenaTop.Contains(actor.Position - ArenaCenterBottom))
            return base.ActiveAOEs(slot, actor);
        else
            return [];
    }
}

class DetermineArena(BossModule module) : BossComponent(module)
{
    private int arena; // 1 = top, 2 = bottom
    public override void DrawArenaBackground(int pcSlot, Actor pc)
    {
        if (arena != 2 && ArenaBottom.Contains(pc.Position - ArenaCenterBottom))
        {
            arena = 2;
            Arena.Center = ArenaCenterBottom;
            Arena.Bounds = ArenaBottom;
        }
        else if (arena != 1 && ArenaTop.Contains(pc.Position - ArenaCenterTop))
        {
            arena = 1;
            Arena.Center = ArenaCenterTop;
            Arena.Bounds = ArenaTop;
        }
    }
}

class BossHealths(BossModule module) : BossComponent(module)
{
    private readonly Actor? _bossHellDiver = module.Enemies((uint)OID.FourthLegionHelldiver1)[0];

    public override void AddGlobalHints(GlobalHints hints)
    {
        hints.Add($"Top: {Module.PrimaryActor.HPRatio * 100f:f1}%, Bottom: {_bossHellDiver?.HPRatio * 100f:f1}%");
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "The Combat Reborn Team (Malediktus)", GroupType = BossModuleInfo.GroupType.CastrumLacusLitore, GroupID = 735, NameID = 9436)]
public class CLL1Brionac4thLegionHelldiver : BossModule
{
    public CLL1Brionac4thLegionHelldiver(WorldState ws, Actor primary) : base(ws, primary, ArenaCenterBottom, ArenaBottom)
    {
        ActivateComponent<DetermineArena>();
    }

    private Actor? _bossHellDiver;
    public Actor? BossHelldiver() => _bossHellDiver;

    protected override void UpdateModule()
    {
        // TODO: this is an ugly hack, think how multi-actor fights can be implemented without it...
        // the problem is that on wipe, any actor can be deleted and recreated in the same frame
        if (_bossHellDiver == null)
        {
            if (StateMachine.ActivePhaseIndex == 0)
            {
                var b = Enemies((uint)OID.FourthLegionHelldiver1);
                _bossHellDiver = b.Count != 0 ? b[0] : null;
            }
        }
    }

    protected override bool CheckPull() => base.CheckPull() || (_bossHellDiver?.InCombat ?? false);

    protected override void DrawEnemies(int pcSlot, Actor pc)
    {

        if (Arena.Center == ArenaCenterTop)
            Arena.Actor(PrimaryActor);
        else
        {
            Arena.Actors(Enemies((uint)OID.FourthLegionHelldiver3));
            Arena.Actor(_bossHellDiver);
        }
        var skyarmors = Enemies((uint)OID.FourthLegionSkyArmor);
        var count = skyarmors.Count;
        for (var i = 0; i < count; ++i)
        {
            var skyarmor = skyarmors[i];
            if (InBounds(skyarmor.Position))
                Arena.Actor(skyarmor);
        }
    }

    public static readonly WPos ArenaCenterBottom = new(80f, -179.41f);
    public static readonly ArenaBoundsRect ArenaBottom = new(29.58f, 24.59f);
    public static readonly WPos ArenaCenterTop = new(new(80f, -222f));
    public static readonly ArenaBoundsRect ArenaTop = new(29.5f, 14.5f);

    protected override void CalculateModuleAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        var count = hints.PotentialTargets.Count;
        var potHints = CollectionsMarshal.AsSpan(hints.PotentialTargets);
        var center = Arena.Center;
        for (var i = 0; i < count; ++i)
        {
            ref readonly var e = ref potHints[i].Actor;
            ref var enemyPrio = ref potHints[i].Priority;
            ref readonly var oid = ref e.OID;
            if (center == ArenaCenterTop)
            {
                if (oid == (uint)OID.MagitekCore)
                {
                    enemyPrio = 1;
                }
                else if (e == PrimaryActor && e.HPRatio - _bossHellDiver?.HPRatio < -0.1f)
                    enemyPrio = AIHints.Enemy.PriorityForbidden;
                else if (oid != (uint)OID.Boss)
                    enemyPrio = AIHints.Enemy.PriorityPointless;
                else if (oid == (uint)OID.FourthLegionSkyArmor && InBounds(e.Position))
                    enemyPrio = 0;
            }
            else if (center == ArenaCenterBottom)
            {
                if (oid == (uint)OID.FourthLegionHelldiver3)
                {
                    enemyPrio = 1;
                }
                else if (e == _bossHellDiver && e.HPRatio - PrimaryActor.HPRatio < -0.1f)
                    enemyPrio = AIHints.Enemy.PriorityForbidden;
                else if (oid != (uint)OID.FourthLegionHelldiver1)
                    enemyPrio = AIHints.Enemy.PriorityPointless;
                else if (oid == (uint)OID.FourthLegionSkyArmor && InBounds(e.Position))
                    enemyPrio = 0;
            }
        }
    }
}
