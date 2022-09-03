using Microsoft.EntityFrameworkCore;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Data.Repositories;

public class MapAppearanceRepository :
    EntityFrameworkRepository<MapAppearance, ApplicationContext>,
    IMapAppearanceRepository
{
    public MapAppearanceRepository(ApplicationContext context) : base(context)
    {
    }


    public override async Task<bool> ContainsAsync(int id)
    {
        var foundId = await _context.MapAppearance
            .Select(x => x.Id)
            .SingleOrDefaultAsync(x => x == id);
        return foundId != default;
    }

    public async Task<bool> ContainsForMapAsync(int mapId)
    {
        var queryResult = await _context.MapAppearance
            .Select(x => x.MapId)
            .SingleOrDefaultAsync(x => x == mapId);
        return queryResult != default;
    }


    public override async Task DeleteAsync(int id)
    {
        var mapAppearance = await GetAsync(id, false);
        _context.MapAppearance.Remove(mapAppearance);
    }



    public override async Task<MapAppearance> GetAsync(int id,
        bool tracking = true)
    {
        return await GetAsync(id, tracking, _context.MapAppearance);
    }


    public async Task<MapAppearance> GetFullAsync(int mapId,
        bool tracking = true)
    {
        return await GetForMapAsync(mapId, tracking,
            _context.MapAppearance
                .Include(x => x.Stations)
                .Include(x => x.Lines)
                .Include(x => x.Railways));
    }


    public async Task<MapAppearance> GetSingleLineOnly(int mapId, int lineId,
        bool tracking = true)
    {
        return await GetForMapAsync(mapId, tracking,
            _context.MapAppearance
                .Include(x =>
                    x.Lines.Where(x => x.LineId == lineId)));
    }


    public async Task<MapAppearance> GetSingleRailwayOnly(int mapId,
        int railwayId, bool tracking = true)
    {
        return await GetForMapAsync(mapId, tracking,
            _context.MapAppearance
                .Include(x =>
                    x.Railways.Where(x => x.RailwayId == railwayId)));
    }


    public async Task<MapAppearance> GetSingleStationOnly(int mapId,
        int stationId, bool tracking = true)
    {
        return await GetForMapAsync(mapId, tracking,
            _context.MapAppearance
                .Include(x =>
                    x.Stations.Where(x => x.StationId == stationId)));
    }


    private async Task<MapAppearance> GetForMapAsync(int mapId, bool tracking,
        IQueryable<MapAppearance> query)
    {
        if (!tracking)
        {
            query = query.AsNoTracking();
        }
        return await query.SingleAsync(x => x.MapId == mapId);
    }
}

