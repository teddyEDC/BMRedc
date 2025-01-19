namespace BossMod.Stormblood.Foray.BaldesionArsenal.BA3AbsoluteVirtue;

class BrightDarkAurora(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeRect rect = new(30, 50);
    public readonly List<AOEInstance> _aoes = new(3);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _aoes;

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        void AddAOE() => _aoes.Add(new(rect, spell.LocXZ, spell.Rotation, Module.CastFinishAt(spell)));
        switch ((AID)spell.Action.ID)
        {
            case AID.DarkAurora1:
            case AID.DarkAurora2:
                if (caster.FindStatus(SID.UmbralEssence) != null)
                    AddAOE();
                break;
            case AID.BrightAurora1:
            case AID.BrightAurora2:
                if (caster.FindStatus(SID.AstralEssence) != null)
                    AddAOE();
                break;
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if (_aoes.Count != 0 && (AID)spell.Action.ID is AID.BrightAurora1 or AID.BrightAurora2 or AID.DarkAurora1 or AID.DarkAurora2)
            _aoes.RemoveAt(0);
    }
}

class AstralUmbralRays(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeCircle circleSmall = new(8), circleBig = new(16);
    public readonly List<AOEInstance> _aoes = new(9);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _aoes;

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        void AddAOE(bool big) => _aoes.Add(new(big ? circleBig : circleSmall, spell.LocXZ, default, Module.CastFinishAt(spell)));
        switch ((AID)spell.Action.ID)
        {
            case AID.UmbralRays1:
            case AID.UmbralRays2:
                AddAOE(Module.PrimaryActor.FindStatus(SID.UmbralEssence) != null);
                break;
            case AID.AstralRays1:
            case AID.AstralRays2:
                AddAOE(Module.PrimaryActor.FindStatus(SID.AstralEssence) != null);
                break;
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.UmbralRays1 or AID.UmbralRays2 or AID.AstralRays1 or AID.AstralRays2)
            _aoes.Clear();
    }
}

class BrightDarkAuroraTethers(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly AOEShapeCircle circlePuddle = new(2), circleAOE = new(8);
    private readonly List<(Actor actor, bool occupied, bool isLight)> puddles = new(8);
    private readonly List<(Actor source, ulong target, bool isLightTether)> tetherByActor = new(8);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        var count = tetherByActor.Count;
        if (count == 0)
            return [];

        var isActorTarget = false;
        var isLightTether = false;

        for (var i = 0; i < count; ++i)
        {
            var tether = tetherByActor[i];
            if (tether.target == actor.InstanceID)
            {
                isActorTarget = true;
                isLightTether = tether.isLightTether;
                break;
            }
        }
        var puddlecount = puddles.Count;

        List<AOEInstance> aoes = new(count + puddlecount);
        for (var i = 0; i < count; ++i)
        {
            var tether = tetherByActor[i];
            if (tether.target != actor.InstanceID)
                aoes.Add(new(circleAOE, tetherByActor[i].source.Position, Risky: !isActorTarget));
        }

        if (!isActorTarget)
        {
            for (var i = 0; i < puddlecount; ++i)
                aoes.Add(new(circlePuddle, puddles[i].actor.Position, Risky: false));
        }
        else
        {
            for (var i = 0; i < puddlecount; ++i)
            {
                var puddle = puddles[i];
                var isSafe = puddle.isLight != isLightTether && (IsOccupyingPuddle(actor, puddle.actor.Position) || !puddle.occupied);
                aoes.Add(new(circlePuddle, puddle.actor.Position, Color: isSafe ? Colors.SafeFromAOE : 0, Risky: false));
            }
        }
        return aoes;
    }

    public static bool IsOccupyingPuddle(Actor actor, WPos pos) => actor.Position.InCircle(pos, 2);

    public override void OnActorEAnim(Actor actor, uint state)
    {
        if ((OID)actor.OID is OID.BrightAuroraHelper or OID.DarkAuroraHelper)
        {
            if (state is 0x00100020 or 0x00400080) // puddle becomes occupied / unoccupied
            {
                for (var i = 0; i < puddles.Count; ++i)
                {
                    var puddle = puddles[i];
                    if (puddle.actor == actor)
                    {
                        puddles[i] = puddle with { occupied = state == 0x00100020 };
                        break;
                    }
                }
            }
            else if (state == 0x00040008) // puddle disappears
            {
                for (var i = 0; i < puddles.Count; ++i)
                {
                    var puddle = puddles[i];
                    if (puddle.actor == actor)
                    {
                        puddles.Remove(puddle);
                        break;
                    }
                }
            }
        }
    }

    public override void OnActorCreated(Actor actor)
    {
        if ((OID)actor.OID is OID.BrightAuroraHelper or OID.DarkAuroraHelper)
            puddles.Add(new(actor, false, (OID)actor.OID == OID.BrightAuroraHelper));
    }

    public override void OnTethered(Actor source, ActorTetherInfo tether)
    {
        tetherByActor.Add((source, tether.Target, tether.ID == (uint)TetherID.BrightAurora));
    }

    public override void OnUntethered(Actor source, ActorTetherInfo tether)
    {
        tetherByActor.Remove((source, tether.Target, tether.ID == (uint)TetherID.BrightAurora));
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        var count = tetherByActor.Count;
        if (count == 0)
            return;
        base.AddAIHints(slot, actor, assignment, hints);
        var puddleCount = puddles.Count;

        var isActorTarget = false;
        var isLightTether = false;

        for (var i = 0; i < count; ++i)
        {
            var tether = tetherByActor[i];
            if (tether.target == actor.InstanceID)
            {
                isActorTarget = true;
                isLightTether = tether.isLightTether;
                break;
            }
        }
        var forbiddenInverted = new List<Func<WPos, float>>(8);
        var forbidden = new List<Func<WPos, float>>(8);
        for (var i = 0; i < puddleCount; ++i)
        {
            var puddle = puddles[i];
            if (isActorTarget && puddle.isLight != isLightTether && (IsOccupyingPuddle(actor, puddle.actor.Position) || !puddle.occupied))
            {
                forbiddenInverted.Add(ShapeDistance.InvertedCircle(puddle.actor.Position, 2));
            }
            else if (!isActorTarget)
                forbidden.Add(ShapeDistance.Circle(puddle.actor.Position, 2));
        }
        float maxDistanceFunc(WPos pos)
        {
            var minDistance = float.MinValue;
            for (var i = 0; i < forbiddenInverted.Count; ++i)
            {
                var distance = forbiddenInverted[i](pos);
                if (distance > minDistance)
                    minDistance = distance;
            }
            return minDistance;
        }
        float minDistanceFunc(WPos pos)
        {
            var minDistance = float.MaxValue;
            for (var i = 0; i < forbidden.Count; ++i)
            {
                var distance = forbidden[i](pos);
                if (distance < minDistance)
                    minDistance = distance;
            }
            return minDistance;
        }
        var activation = WorldState.FutureTime(5);
        if (forbiddenInverted.Count != 0)
            hints.AddForbiddenZone(maxDistanceFunc, activation);
        if (forbidden.Count != 0)
            hints.AddForbiddenZone(minDistanceFunc, activation);
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        var count = tetherByActor.Count;
        if (count == 0)
            return;

        var isActorTarget = false;
        var isLightTether = false;

        for (var i = 0; i < count; ++i)
        {
            var tether = tetherByActor[i];
            if (tether.target == actor.InstanceID)
            {
                isActorTarget = true;
                isLightTether = tether.isLightTether;
                break;
            }
        }

        if (isActorTarget)
            hints.Add($"Stand in a {(isLightTether ? "dark" : "bright")} puddle!");
        else
            base.AddHints(slot, actor, hints);
    }

    public override void DrawArenaForeground(int pcSlot, Actor pc)
    {
        base.DrawArenaForeground(pcSlot, pc);
        var count = tetherByActor.Count;
        if (count == 0)
            return;

        Actor? source = null;
        for (var i = 0; i < count; ++i)
        {
            var tether = tetherByActor[i];
            if (tether.target == pc.InstanceID)
            {
                source = tether.source;

                break;
            }
        }
        if (source != null)
        {
            Arena.AddLine(source.Position, pc.Position);
            Arena.Actor(source, Colors.Object, true);
        }
    }
}
