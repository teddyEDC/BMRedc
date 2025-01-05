namespace BossMod.Shadowbringers.Foray.DelubrumReginae;

public abstract class TrinitySeeker(WorldState ws, Actor primary) : BossModule(ws, primary, StartingArena.Center, StartingArena)
{
    private static readonly WPos ArenaCenter = new(0, 278);
    private static readonly DonutSegmentV[] barricades = [.. GenerateBarricades(), .. GenerateBarricadesHitBoxCushion()];

    private static List<DonutSegmentV> GenerateBarricades()
    {
        List<DonutSegmentV> barricades = new(4);
        for (var i = 0; i < 4; ++i)
            barricades.Add(new(ArenaCenter, 18.7f, 21.6f, 45.Degrees() + 90.Degrees() * i, 22.5f.Degrees(), 6));
        return barricades;
    }
    private static List<DonutSegmentV> GenerateBarricadesHitBoxCushion()
    {
        List<DonutSegmentV> barricades = new(8);
        for (var i = 0; i < 4; ++i)
        {
            barricades.Add(new(ArenaCenter, 18.7f, 21.6f, 26.1f.Degrees() + 90.Degrees() * i, 5.Degrees(), 2));
            barricades.Add(new(ArenaCenter, 18.7f, 21.6f, 63.9f.Degrees() + 90.Degrees() * i, 5.Degrees(), 2));
        }
        return barricades;
    }

    public static readonly ArenaBoundsComplex StartingArena = new([new Polygon(ArenaCenter, 29.5f, 48)], [.. barricades, new Rectangle(new(0, 248), 20, 1.25f), new Rectangle(new(0, 308), 20, 1.43f)]);
    public static readonly ArenaBoundsComplex DefaultArena = new([new Polygon(ArenaCenter, 25, 48)], barricades);
}
