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

    public override async Task<bool> ContainsAsync(int id)
    {
        var foundId = await _context.Maps.Select(x => x.Id)
            .SingleOrDefaultAsync(x => x == id);

        return foundId != default;
    }


    public override Task DeleteAsync(int id)
    {
        var map = GetAsync(id);
        _context.Remove(map);
        return Task.CompletedTask;
    }


    public override async Task<Map> GetAsync(int id, bool tracking = true)
    {
        return await GetAsync(id, tracking, _context.Maps);
    }


    public async Task<List<Map>> GetList()
    {
        return await _context.Maps.ToListAsync();
    }


    public async Task<Map> GetFull(int mapId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Lines)
                .Include(x => x.Stations)
                .ThenInclude(x => x.Line)
                .Include(x => x.Railways)
                .ThenInclude(x => x.Duration)
                .Include(x => x.Railways)
                .ThenInclude(x => x.To)
                .Include(x => x.Railways)
                .ThenInclude(x => x.From)
                .Include(x => x.Transfers)
                .ThenInclude(x => x.Duration)
                .Include(x => x.Transfers)
                .ThenInclude(x => x.From)
                .Include(x => x.Transfers)
                .ThenInclude(x => x.To));
    }  


    public async Task<Map> GetWithLine(int mapId, int lineId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Lines.Where(x => x.Id == lineId)));
    }  


    public async Task<Map> GetWithLines(int mapId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Lines));
    }  


    public async Task<Map> GetWithRailway(int mapId, int railwayId,
        bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Railways.Where(x => x.Id == railwayId))
                .ThenInclude(x => x.Duration));
    }  


    public async Task<Map> GetWithRailwayAndStationsBetween(int mapId,
        int railwayId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Railways.Where(x => x.Id == railwayId))
                .ThenInclude(x => x.From)
                .Include(x => x.Railways.Where(x => x.Id == railwayId))
                .ThenInclude(x => x.To)
                .Include(x => x.Railways.Where(x => x.Id == railwayId))
                .ThenInclude(x => x.Duration));
    }  


    public async Task<Map> GetWithRailways(int mapId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Railways)
                .ThenInclude(x => x.Duration));
    }  


    public async Task<Map> GetWithStation(int mapId, int stationId,
        bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Stations.Where(x => x.Id == stationId)));
    }  


    public async Task<Map> GetWithStations(int mapId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Stations));
    }  


    public async Task<Map> GetWithStationsAndLines(int mapId,
        bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Stations)
                .ThenInclude(x => x.Line)
                .Include(x => x.Lines));
    }  


    public async Task<Map> GetWithTransfer(int mapId, int transferId,
        bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Transfers.Where(x => x.Id == transferId))
                .ThenInclude(x => x.Duration));
    }  


    public async Task<Map> GetWithTransferAndStationsBetween(int mapId,
        int transferId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Transfers.Where(x => x.Id == transferId))
                .ThenInclude(x => x.From)
                .Include(x => x.Transfers.Where(x => x.Id == transferId))
                .ThenInclude(x => x.To)
                .Include(x => x.Transfers.Where(x => x.Id == transferId))
                .ThenInclude(x => x.Duration));
    }  


    public async Task<Map> GetWithTransfers(int mapId, bool tracking = true)
    {
        return await GetAsync(mapId, tracking,
            _context.Maps
                .Include(x => x.Transfers)
                .ThenInclude(x => x.Duration));
    }  
}  

