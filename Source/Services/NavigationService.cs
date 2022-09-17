using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services;

public class NavigationService : INavigationService
{
    private readonly IMapRepository _repository;


    public NavigationService(IMapRepository repository)
    {
        _repository = repository;
    }


    public async Task<Station[]> GetRoute(
        int mapId, int fromStation, int toStation)
    {
        throw new NotImplementedException();
    }
}

