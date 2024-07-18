namespace BossMod.Dawntrail.Raid.M1NBlackCat;

class ArenaChanges(BossModule module) : BossComponent(module)
{
    private ArenaBounds? arena;
    public static readonly WPos ArenaCenter = new(100, 100);
    public static readonly ArenaBoundsSquare DefaultBounds = new(20);
    private static readonly Square defaultSquare = new(ArenaCenter, 20);
    private static readonly WPos[] tilePositions =
    [
        new(85, 85), new(95, 85), new(105, 85), new(115, 85), new(85, 95),
        new(95, 95), new(105, 95), new(115, 95), new(85, 105), new(95, 105),
        new(105, 105), new(115, 105), new(85, 115), new(95, 115), new(105, 115),
        new(115, 115)
    ];
    public readonly List<Shape> DamagedTiles = [];
    private readonly List<Shape> brokenTiles = [];

    private static readonly Square[] tiles = tilePositions.Select(pos => new Square(pos, 5)).ToArray();

    public override void OnEventEnvControl(byte index, uint state)
    {
        if (state == 0x00020001)
            AddTileToList(DamagedTiles, index);
        else if (state == 0x00200010)
            MoveTileBetweenLists(DamagedTiles, brokenTiles, index);
        else if (state is 0x01000004 or 0x00800004)
            RemoveTileFromLists(index);
    }

    private void AddTileToList(List<Shape> list, byte index)
    {
        if (index < tiles.Length)
            list.Add(tiles[index]);
    }

    private void MoveTileBetweenLists(List<Shape> fromList, List<Shape> toList, byte index)
    {
        if (index < tiles.Length)
        {
            var tile = tiles[index];
            fromList.Remove(tile);
            toList.Add(tile);
            UpdateArenaBounds();
        }
    }

    private void RemoveTileFromLists(byte index)
    {
        if (index < tiles.Length)
        {
            var tile = tiles[index];
            DamagedTiles.Remove(tile);
            brokenTiles.Remove(tile);
            UpdateArenaBounds();
        }
    }

    private void UpdateArenaBounds()
    {
        arena = new ArenaBoundsComplex([defaultSquare], brokenTiles, Offset: -0.5f);
        Module.Arena.Bounds = arena;
    }
}

class Mouser(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];
    private static readonly AOEShapeRect rect = new(5, 5, 5);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        if (_aoes.Count > 0)
        {
            var aoeCount = Math.Clamp(_aoes.Count, 0, NumCasts > 2 ? 2 : 3);
            for (var i = aoeCount; i < _aoes.Count; i++)
                yield return _aoes[i];
            for (var i = 0; i < aoeCount; i++)
                yield return _aoes[i] with { Color = ArenaColor.Danger };
        }
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.MouserTelegraphFirst or AID.MouserTelegraphSecond)
            _aoes.Add(new(rect, caster.Position, spell.Rotation, Module.WorldState.FutureTime(9.7f)));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if (_aoes.Count > 0 && (AID)spell.Action.ID == AID.Mouser)
        {
            _aoes.RemoveAt(0);
            if (++NumCasts == 19)
                NumCasts = 0;
        }
    }
}
