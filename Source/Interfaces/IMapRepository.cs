using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface IMapRepository : IRepository<Map>
{
    Task<Map> GetFullAsync(int mapId, bool tracking = true);
    Task<Map> GetWithLinesAsync(int mapId, bool tracking = true);
    Task<Map> GetWithStationsAsync(int mapId, bool tracking = true);
    Task<Map> GetWithStationsAsync(int mapId, int linemapId, bool tracking = true);
    Task<Map> GetWithWaysAsync(int mapId, bool tracking = true);
    Task<Map> GetWithWaysAsync(int mapId, int stationmapId, bool tracking = true);
}
