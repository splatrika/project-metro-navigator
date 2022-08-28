using Microsoft.EntityFrameworkCore;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Data.Repositories;

public class MapRepository : EntityFrameworkRepository<Map, ApplicationContext>,
    IMapRepository
{
    public MapRepository(ApplicationContext context) : base(context)
    {
    }
    

    public async Task<Map> GetFullAsync(int mapId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Lines)
                .Include(x => x.Stations)
                .Include(x => x.Railways)
                .ThenInclude(x => x.Duration)
                .Include(x => x.Railways)
                .ThenInclude(x => x.Duration));
    }


    public async Task<Map> GetWithLinesAsync(int mapId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Lines));
    }


    public async Task<Map> GetWithStationsAsync(int mapId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Stations));
    }


    public async Task<Map> GetWithStationsAsync(int mapId, int lineId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x =>
                    x.Stations.Where(x => x.Line.Id == lineId))
                .ThenInclude(x => x.Line));
    }


    public async Task<Map> GetWithWaysAsync(int mapId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Railways)
                .Include(x => x.Transfers));
    }


    public async Task<Map> GetWithWaysAsync(int mapId, int stationId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x =>
                    x.Stations.Where(x => x.Id == stationId))
                .Include(x =>
                    x.Transfers.Where(x => x.From.Id == stationId || x.To.Id == stationId))
                .Include(x =>
                    x.Railways.Where(x => x.From.Id == stationId || x.To.Id == stationId)));
    }


    public override async Task DeleteAsync(int id)
    {
        var instance = await _context.Maps
            .Select(x => new Map("", id))
            .SingleAsync(x => x.Id == id);
        _context.Maps.Remove(instance);
    }


    public override async Task<Map> GetAsync(int id, bool tracking = true)
    {
        return await GetAsync(id, tracking, _context.Maps);
    }


    public override async Task<bool> ContainsAsync(int id)
    {
        var queryResult = await _context.Maps.Select(x => x.Id)
            .SingleOrDefaultAsync(x => x == id);
        return queryResult != default;
    }
}

