﻿using System.IO;
using System.Reflection;

namespace BossMod.Pathfinding;

[ConfigDisplay(Name = "Obstacle map development", Order = 8)]
public sealed class ObstacleMapConfig : ConfigNode
{
    [PropertyDisplay("Developer mode: load obstacle maps from source rather than from plugin distribution")]
    public bool LoadFromSource;

    [PropertyDisplay("Developer mode: source path", tooltip: "Should be <repo root>/BossMod/Pathfinding/ObstacleMaps/maplist.json")]
    public string SourcePath = "";
}

public sealed class ObstacleMapManager : IDisposable
{
    public readonly WorldState World;
    public readonly ObstacleMapDatabase Database = new();
    public string RootPath { get; private set; } = ""; // empty or ends with slash
    private readonly ObstacleMapConfig _config = Service.Config.Get<ObstacleMapConfig>();
    private readonly EventSubscriptions _subscriptions;
    private readonly List<(ObstacleMapDatabase.Entry entry, Bitmap data)> _entries = [];

    public bool LoadFromSource => _config.LoadFromSource;

    public ObstacleMapManager(WorldState ws)
    {
        World = ws;
        _subscriptions = new(
            _config.Modified.Subscribe(ReloadDatabase),
            ws.CurrentZoneChanged.Subscribe(op => LoadMaps(op.Zone, op.CFCID))
        );
        ReloadDatabase();
    }

    public void Dispose()
    {
        _subscriptions.Dispose();
    }

    public (ObstacleMapDatabase.Entry? entry, Bitmap? data) Find(Vector3 pos) => _entries.FirstOrDefault(e => e.entry.Contains(pos));
    public bool CanEditDatabase() => _config.LoadFromSource;
    public uint ZoneKey(ushort zoneId, ushort cfcId) => ((uint)zoneId << 16) | cfcId;
    public uint CurrentKey() => ZoneKey(World.CurrentZone, World.CurrentCFCID);

    public void ReloadDatabase()
    {
        Service.Log($"Loading obstacle database from {(_config.LoadFromSource ? _config.SourcePath : "<embedded>")}");
        RootPath = _config.LoadFromSource ? _config.SourcePath[..(_config.SourcePath.LastIndexOfAny(['\\', '/']) + 1)] : "";

        try
        {
            using var dbStream = _config.LoadFromSource ? File.OpenRead(_config.SourcePath) : GetEmbeddedResource("maplist.json");
            Database.Load(dbStream);
        }
        catch (Exception ex)
        {
            Service.Log($"Failed to load obstacle database: {ex}");
            Database.Entries.Clear();
        }

        LoadMaps(World.CurrentZone, World.CurrentCFCID);
    }

    public void SaveDatabase()
    {
        if (!_config.LoadFromSource)
            return;
        Database.Save(_config.SourcePath);
        LoadMaps(World.CurrentZone, World.CurrentCFCID);
    }

    private void LoadMaps(ushort zoneId, ushort cfcId)
    {
        _entries.Clear();
        if (Database.Entries.TryGetValue(ZoneKey(zoneId, cfcId), out var entries))
        {
            foreach (var e in entries)
            {
                try
                {
                    using var eStream = _config.LoadFromSource ? File.OpenRead(RootPath + e.Filename) : GetEmbeddedResource(e.Filename);
                    var bitmap = new Bitmap(eStream);
                    _entries.Add((e, bitmap));
                }
                catch (Exception ex)
                {
                    Service.Log($"Failed to load map {e.Filename} from {(_config.LoadFromSource ? RootPath : "<embedded>")} for {zoneId}.{cfcId}: {ex}");
                }
            }
        }
    }

    private Stream GetEmbeddedResource(string name) => Assembly.GetExecutingAssembly().GetManifestResourceStream($"BossMod.Pathfinding.ObstacleMaps.{name}") ?? throw new InvalidDataException($"Missing embedded resource {name}");
}
