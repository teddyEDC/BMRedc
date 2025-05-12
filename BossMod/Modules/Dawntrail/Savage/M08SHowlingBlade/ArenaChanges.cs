namespace BossMod.Dawntrail.Savage.M08SHowlingBlade;

class ArenaChanges(BossModule module) : Components.GenericAOEs(module)
{
    private static readonly M08SHowlingBladeConfig _config = Service.Config.Get<M08SHowlingBladeConfig>();
    private readonly List<Polygon> polygons = new(5);
    private static readonly Polygon[] pillarPolygons =
    [
        new(new(107f, 100f), 5.5f, 20, -89.98f.Degrees()), // east, ENVC 0x10
        new(new(93f, 100f), 5.5f, 20, 89.98f.Degrees()), // west, ENVC 0x11
        new(new(100f, 93f), 5.5f, 20), // north, ENVC 0x012
        new(new(100f, 107f), 5.5f, 20), // southeast, ENVC 0x13
    ];
    public static readonly Polygon[] EndArenaPlatforms =
    [
        new(new(100f, 117.5f), 8f, 40), // south, ENVC 0x16
        new(new(83.357f, 105.408f), 8f, 40), // southwest, ENVC 0x17
        new(new(89.714f, 85.842f), 8f, 40), // northwest, ENVC 0x18
        new(new(110.286f, 85.842f), 8f, 40), // northeast, ENVC 0x19
        new(new(116.643f, 105.408f), 8f, 40), // southeast, ENVC 0x1A
    ];
    public static readonly Angle[] PlatformAngles = CalculateAngles();
    private static readonly WPos[] numberPositions = CalculateNumberPositions();
    private readonly bool[] activePlatforms = new bool[5];
    private bool active;
    private AOEInstance? _aoe;
    public bool Repaired => polygons.Count == 5;
    private static readonly AOEShapeCircle circle = new(8f);

    public override ReadOnlySpan<AOEInstance> ActiveAOEs(int slot, Actor actor) => Utils.ZeroOrOne(ref _aoe);

    public override void OnEventEnvControl(byte index, uint state)
    {
        if (index == 0x01u)
        {
            switch (state)
            {
                case 0x00200010u:
                    _aoe = new(circle, Arena.Center, default, WorldState.FutureTime(5.6d));
                    break;
                case 0x00020001u:
                    _aoe = null;
                    Arena.Bounds = M08SHowlingBlade.DonutArena;
                    break;
                case 0x00080004u:
                    Arena.Bounds = M08SHowlingBlade.StartingArena;
                    break;
            }
        }
        if (index is >= 0x10 and <= 0x13)
        {
            if (state is 0x00020001u or 0x00800040u)
            {
                polygons.Add(pillarPolygons[index - 0x10u]);
                if (polygons.Count == 2)
                {
                    var arena = new ArenaBoundsComplex(M08SHowlingBlade.StartingArenaPolygon, [.. polygons]);
                    Arena.Bounds = arena;
                    Arena.Center = arena.Center;
                }
            }
            else if (state is 0x04000004u or 0x00200004u && Arena.Bounds != M08SHowlingBlade.StartingArena)
            {
                polygons.Clear();
                Arena.Bounds = M08SHowlingBlade.StartingArena;
                Arena.Center = M08SHowlingBlade.ArenaCenter;
            }
        }
        else if (index is >= 0x16 and <= 0x1A)
        {
            if (state is 0x00010001u or 0x00020001u)
            {
                var i = index - 0x16u;
                polygons.Add(EndArenaPlatforms[i]);
                activePlatforms[i] = true;
                if (polygons.Count == 5)
                {
                    var arena = new ArenaBoundsComplex([.. polygons]);
                    Arena.Bounds = arena;
                    Arena.Center = arena.Center;
                    active = true;
                }
            }
            else if (state == 0x00080004u)
            {
                var i = index - 0x16u;
                polygons.Remove(EndArenaPlatforms[i]);
                activePlatforms[i] = false;
                if (polygons.Count == 0)
                {
                    Arena.Bounds = M08SHowlingBlade.StartingArena;
                    return;
                }
                var arena = new ArenaBoundsComplex([.. polygons]);
                Arena.Bounds = arena;
                Arena.Center = arena.Center;
            }
        }
    }

    public override void DrawArenaForeground(int pcSlot, Actor pc)
    {
        if (active && _config.ShowPlatformNumbers)
        {
            for (var i = 0; i < 5; ++i)
            {
                if (activePlatforms[i])
                    Arena.TextWorld(numberPositions[i], $"{i + 1}", _config.PlatformNumberColors[i].ABGR, _config.PlatformNumberFontSize);
            }
        }
    }

    private static Angle[] CalculateAngles()
    {
        Span<Angle> platformAngles = stackalloc Angle[5];
        var angle = -72f.Degrees();
        for (var i = 0; i < 5; ++i)
            platformAngles[i] = angle * i;
        return [.. platformAngles];
    }

    private static WPos[] CalculateNumberPositions()
    {
        Span<WPos> positions = stackalloc WPos[5];
        for (var i = 0; i < 5; ++i)
        {
            positions[i] = M08SHowlingBlade.ArenaCenter + 7f * PlatformAngles[i].ToDirection();
        }
        return [.. positions];
    }
}

class Teleporters(BossModule module) : BossComponent(module)
{
    private readonly List<WPos> activeTeleporters = new(10);
    private static readonly WPos[] teleporters = CalculateTeleporterPositions();

    private static WPos[] CalculateTeleporterPositions()
    {
        Span<WPos> positions = stackalloc WPos[10];
        var index = 0;
        for (var i = 0; i < 5; ++i)
        {
            var zero = i == 0;
            var angle = ArenaChanges.PlatformAngles[i].Deg;
            positions[index++] = ArenaChanges.EndArenaPlatforms[i].Center + 6f * (zero ? -120 : 120f + angle).Degrees().ToDirection();
            positions[zero ? 9 : index++] = ArenaChanges.EndArenaPlatforms[i].Center + 6f * (zero ? 120 : -120f + angle).Degrees().ToDirection();
        }
        return [.. positions];
    }

    public override void OnEventEnvControl(byte index, uint state)
    {
        if (index is >= 0x02 and <= 0x0B)
        {
            if (state is 0x00020001u or 0x00200010u)
            {
                var count = activeTeleporters.Count;
                ref readonly var teleporter = ref teleporters[index - 0x02u];
                for (var i = 0; i < count; ++i)
                {
                    if (activeTeleporters[i] == teleporter) // prevent duplicates
                        return;
                }
                activeTeleporters.Add(teleporter);
            }
            else if (state == 0x00080004u)
            {
                activeTeleporters.Remove(teleporters[index - 0x02u]);
            }
        }
    }

    public override void DrawArenaForeground(int pcSlot, Actor pc)
    {
        var count = activeTeleporters.Count;
        for (var i = 0; i < count; ++i)
        {
            Arena.AddCircle(activeTeleporters[i], 1f, Colors.Object, 2f);
        }
    }
}
