using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services;

public class MapAppearanceService : IMapAppearanceService
{
    private readonly IMapAppearanceRepository _repository;


    public MapAppearanceService(IMapAppearanceRepository repository)
    {
        _repository = repository;
    }


    public async Task CleanUpLine(int mapId, int lineId)
    {
        await Clean(mapId,
            appearance => appearance.CleanUpLine(lineId));
    }


    public async Task CleanUpRailway(int mapId, int railwayId)
    {
        await Clean(mapId,
            appearance => appearance.CleanUpRailway(railwayId));
    }


    public async Task CleanUpStation(int mapId, int stationId)
    {
        await Clean(mapId,
            appearance => appearance.CleanUpStation(stationId));
    }


    public async Task<LineAppearance> GetLine(int mapId, int lineId)
    {
        return await Get(mapId,
            get: appearance => appearance.GetLine(lineId),
            defaultAppearance: LineAppearance.GetDefault(lineId));
    }


    public async Task<RailwayAppearance> GetRailway(int mapId, int railwayId)
    {
        return await Get(mapId,
            get: appearance => appearance.GetRailway(railwayId),
            defaultAppearance: RailwayAppearance.GetDefault(railwayId));
    }


    public async Task<StationAppearance> GetStation(int mapId, int stationId)
    {
        return await Get(mapId,
            get: appearance => appearance.GetStation(stationId),
            defaultAppearance: StationAppearance.GetDefault(stationId));
    }


    public async Task UpdateLine(int mapId, int lineId, Color color)
    {
        await Update(mapId,
            appearance => appearance.UpdateLine(lineId, color));
    }


    public async Task UpdateRailway(int mapId, int railwayId,
        IEnumerable<Position> points)
    {
        await Update(mapId,
            appearance => appearance.UpdateRailway(railwayId, new(points)));
    }


    public async Task UpdateStation(int mapId, int stationId, Position position,
        Caption caption)
    {
        await Update(mapId,
            appearance => appearance.UpdateStation(stationId, position, caption));
    }


    private async Task Clean(int mapId, Action<MapAppearance> action)
    {
        if (!await _repository.ContainsAsync(mapId)) return;

        var appearance = await _repository.GetFullAsync(mapId);
        action.Invoke(appearance);
        await _repository.SaveChangesAsync();
    }


    private async Task Update(int mapId, Action<MapAppearance> action)
    {
        MapAppearance? created = null;

        if (!await _repository.ContainsAsync(mapId))
        {
            created = new MapAppearance(mapId);
            await _repository.AddAsync(created);
        }

        var appearance = created ?? await _repository.GetFullAsync(mapId);
        action.Invoke(appearance);
        await _repository.SaveChangesAsync();
    }


    private async Task<T> Get<T>(int mapId, Func<MapAppearance, T> get,
        T defaultAppearance)
    {
        if (!await _repository.ContainsAsync(mapId))
        {
            return defaultAppearance;
        }

        var appearance = await _repository.GetFullAsync(mapId);
        return get.Invoke(appearance);
    }
}

