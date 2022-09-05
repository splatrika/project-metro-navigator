using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface IMapAppearanceService
{
    Task UpdateStation(int mapId, int stationId, Position position,
        Caption caption);

    Task UpdateLine(int mapId, int lineId, Color color);

    Task UpdateRailway(int mapId, int railwayId, IEnumerable<Position> points);

    Task<StationAppearance> GetStation(int mapId, int stationId);

    Task<LineAppearance> GetLine(int mapId, int lineId);

    Task<RailwayAppearance> GetRailway(int mapId, int railwayId);

    Task CleanUpStation(int mapId, int stationId);

    Task CleanUpLine(int mapId, int lineId);

    Task CleanUpRailway(int mapId, int railwayId);
}

