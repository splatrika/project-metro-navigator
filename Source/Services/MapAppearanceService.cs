using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services;

public class MapAppearanceService : IMapAppearanceService
{
    public MapAppearanceService(IMapAppearanceRepository repository)
    {
        throw new NotImplementedException();
    }


    public Task CleanUpLine(int mapId, int lineId)
    {
        throw new NotImplementedException();
    }


    public Task CleanUpRailway(int mapId, int railwayId)
    {
        throw new NotImplementedException();
    }


    public Task CleanUpStation(int mapId, int stationId)
    {
        throw new NotImplementedException();
    }


    public Task<LineAppearance> GetLine(int mapId, int lineId)
    {
        throw new NotImplementedException();
    }


    public Task<RailwayAppearance> GetRailway(int mapId, int railwayId)
    {
        throw new NotImplementedException();
    }


    public Task<StationAppearance> GetStation(int mapId, int stationId)
    {
        throw new NotImplementedException();
    }


    public Task UpdateLine(int mapId, int lineId, Color color)
    {
        throw new NotImplementedException();
    }


    public Task UpdateRailway(int mapId, int railwayId, IEnumerable<Position> points)
    {
        throw new NotImplementedException();
    }


    public Task UpdateStation(int mapId, int stationId, Position position, Caption caption)
    {
        throw new NotImplementedException();
    }
}

