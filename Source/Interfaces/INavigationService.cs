using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface INavigationService
{
    Task<Station[]> GetRoute(int mapId, int fromStation, int toStation);
}

