using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Exceptions;
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
        var map = await _repository.GetFull(mapId, tracking: false);

        var ways = new List<Way>();
        ways.AddRange(map.Railways);
        ways.AddRange(map.Transfers);

        var from = map.Stations.SingleOrDefault(x => x.Id == fromStation);
        var to = map.Stations.SingleOrDefault(x => x.Id == toStation);
        if (from == null || to == null)
        {
            throw new NavigationException("unexistent", "unexistent");
        }

        var weights = new Dictionary<Station, float>();
        var done = new List<Station>();
        var parents = new Dictionary<Station, Station>();

        foreach (var station in map.Stations)
        {
            weights.Add(station, float.PositiveInfinity);
        }
        weights[from] = 0;

        while (done.Count != map.Stations.Count())
        {
            var withMinWeight = map.Stations.MinBy(station =>
            {
                return done.Contains(station)
                    ? float.PositiveInfinity
                    : weights[station];
            })!;
            var selected = withMinWeight;

            var relatedWays = ways.Where(way =>
                way.From == selected || way.To == selected);

            foreach (var way in relatedWays)
            {
                var stationB = way.From == selected
                    ? way.To
                    : way.From;
                if (done.Contains(stationB))
                {
                    continue;
                }
                var newWeight = weights[selected] + way.Duration.Seconds;
                if (weights[stationB] > newWeight)
                {
                    weights[stationB] = newWeight;
                    parents.Add(stationB, selected);
                }
                await Task.Yield();
            }
            done.Add(selected);
            await Task.Yield();
        }

        var route = new List<Station>() { to };
        var last = route.Last();
        while (last != from)
        {
            if (!parents.ContainsKey(last))
            {
                throw new NavigationException(from.ToString(), to.ToString());
            }
            route.Add(parents[last]);
            last = route.Last();
        }
        route.Reverse();
        return route.ToArray();
    }
}

