using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface IMapRepository : IRepository<Map>
{
    Task<Map> GetFull(int mapId, bool tracking = true);

    Task<Map> GetWithStation(int mapId, int stationId, bool tracking = true);

    Task<Map> GetWithLine(int mapId, int lineId, bool tracking = true);

    Task<Map> GetWithTransfer(int mapId, int transferId, bool tracking = true);

    Task<Map> GetWithRailway(int mapId, int railwayId, bool tracking = true);

    Task<Map> GetWithStations(int mapId, bool tracking = true);

    Task<Map> GetWithLines(int mapId, bool tracking = true);

    Task<Map> GetWithTransfers(int mapId, bool tracking = true);

    Task<Map> GetWithRailways(int mapId, bool tracking = true);

    Task<Map> GetWithStationsAndLines(int mapId, bool tracking = true);

    Task<Map> GetWithTransferAndStationsBetween(int mapId, int transferId,
        bool tracking = true);

    Task<Map> GetWithRailwayAndStationsBetween(int mapId, int railwayId,
        bool tracking = true);
}
