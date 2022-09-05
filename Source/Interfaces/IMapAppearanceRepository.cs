using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface IMapAppearanceRepository : IRepository<MapAppearance>
{
    Task<MapAppearance> GetFullAsync(int mapId, bool tracking = true);
    Task<MapAppearance> GetSingleLineOnly(int mapId, int lineId, bool tracking = true);
    Task<MapAppearance> GetSingleStationOnly(int mapId, int stationId, bool tracking = true);
    Task<MapAppearance> GetSingleRailwayOnly(int mapId, int railwayId, bool tracking = true);
    Task<bool> ContainsForMapAsync(int mapId);
}

