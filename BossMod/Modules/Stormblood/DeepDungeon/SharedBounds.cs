namespace BossMod.Stormblood.DeepDungeon.HeavenOnHigh;

public abstract class HoHBoss(WorldState ws, Actor primary) : BossModule(ws, primary, arena.Center, arena)
{
    private static readonly ArenaBoundsComplex arena = new([new Polygon(new(-300f, -300f), 24.5f * CosPI.Pi48th, 48, 3.75f.Degrees())], [new Rectangle(new(-300f, -325f), 20f, 1.25f)]);
}

public abstract class HoHBoss2(WorldState ws, Actor primary) : BossModule(ws, primary, arena.Center, arena)
{
    private static readonly ArenaBoundsComplex arena = new([new Polygon(new(-300f, -300f), 24.5f, 48)], [new Rectangle(new(-299.839f, -325.43f), 20f, 1.25f)]);
}