using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface IMapAppearanceRepository : IRepository<MapAppearance>
{
    Task<MapAppearance> GetFullAsync(int id, bool tracking = true);
}

