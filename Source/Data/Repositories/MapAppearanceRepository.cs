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
        var queryResult = await _context.MapAppearance
            .Select(x => x.Id)
            .SingleOrDefaultAsync(x => x == id);
        return queryResult != default;
    }


    public override async Task DeleteAsync(int id)
    {
        var mapAppearance = await GetAsync(id, false);
        _context.MapAppearance.Remove(mapAppearance);
    }


    public override async Task<MapAppearance> GetAsync(int id, bool tracking = true)
    {
        return await GetAsync(id, tracking, _context.MapAppearance);
    }


    public async Task<MapAppearance> GetFullAsync(int id, bool tracking = true)
    {
        return await GetAsync(id, tracking,
            _context.MapAppearance
                .Include(x => x.Lines)
                .Include(x => x.Railways)
                .Include(x => x.Stations));
    }
}

