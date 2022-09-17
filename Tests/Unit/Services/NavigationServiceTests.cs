using Splatrika.MetroNavigator.Source.Services;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Tests.Unit.Builders;
using Moq;
using Splatrika.MetroNavigator.Source.Exceptions;

namespace Splatrika.MetroNavigator.Tests.Unit.Services;

public class NavigationServiceTests
{
    private DurationFactor DefaultDuration => new StaticDuration(100);


    [Fact]
    public async Task GetRoute()
    {
        var map = GenerateMap(out Station from, out Station to);
        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetFull(map.Id, It.IsAny<bool>()));
        var service = new NavigationService(repository);

        var route = await service.GetRoute(map.Id, from.Id, to.Id);
        Assert.True(IsValid(map, route));
        Assert.Equal(from, route.First());
        Assert.Equal(to, route.Last());
    }


    [Fact]
    public async Task TryGetUnexistedWay()
    {
        var map = GenerateMapWithoutWays(out Station from, out Station to);
        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetFull(map.Id, It.IsAny<bool>()));
        var service = new NavigationService(repository);

        await Assert.ThrowsAsync<NavigationException>(async
            () => await service.GetRoute(map.Id, from.Id, to.Id));
    }


    [Fact]
    public async Task TryGetBetweenUnexistentStation()
    {
        var map = GenerateMapWithUnexistentStation(
            out Station from, out Station to);
        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetFull(map.Id, It.IsAny<bool>()));
        var service = new NavigationService(repository);

        await Assert.ThrowsAsync<NavigationException>(async
            () => await service.GetRoute(map.Id, from.Id, to.Id));
    }


    private Map GenerateMap(out Station from, out Station to)
    {
        var line1 = new Line("line1", 1);
        var line2 = new Line("line2", 2);
        var station11 = new Station("station11", line1, 1);
        var station12 = new Station("station12", line1, 2);
        var station13 = new Station("station13", line1, 3);
        var station21 = new Station("station21", line2, 4);
        var station22 = new Station("station22", line2, 5);
        var station23 = new Station("station23", line2, 6);
        var railway1 = new Railway(station11, station12, DefaultDuration, id: 1);
        var railway2 = new Railway(station12, station13, DefaultDuration, id: 2);
        var railway3 = new Railway(station21, station22, DefaultDuration, id: 3);
        var railway4 = new Railway(station22, station23, DefaultDuration, id: 4);
        var transfer = new Transfer(station12, station22, DefaultDuration, id: 1);

        var map = MapBuilder.WithItems(mapId: 1,
            linesReference: new() { line1, line2 },
            stationsReference: new()
            {
                station11,
                station12,
                station13,
                station21,
                station22,
                station23
            },
            railwaysReference: new()
            {
                railway1,
                railway2,
                railway3,
                railway4
            },
            transfersReference: new() { transfer });

        from = station11;
        to = station21;
        return map;
    }


    private Map GenerateMapWithoutWays(out Station from, out Station to)
    {
        var line1 = new Line("line1", 1);
        var line2 = new Line("line2", 2);

        var station11 = new Station("station11", line1, 1);
        var station12 = new Station("station12", line1, 2);
        var station13 = new Station("station13", line1, 3);

        var station21 = new Station("station21", line2, 4);
        var station22 = new Station("station22", line2, 5);
        var station23 = new Station("station23", line2, 6);

        var railway1 = new Railway(station11, station12, DefaultDuration, id: 1);
        var railway2 = new Railway(station12, station13, DefaultDuration, id: 2);
        var railway3 = new Railway(station21, station22, DefaultDuration, id: 3);
        var railway4 = new Railway(station22, station23, DefaultDuration, id: 4);

        from = station11;
        to = station21;

        var map = MapBuilder.WithItems(mapId: 1,
            linesReference: new() { line1, line2 },
            stationsReference: new()
            {
                station11,
                station12,
                station13,
                station21,
                station22,
                station23
            },
            railwaysReference: new()
            {
                railway1,
                railway2,
                railway3,
                railway4
            },
            transfersReference: new());

        return map;
    }


    private Map GenerateMapWithUnexistentStation(out Station unexistent1,
        out Station unexistent2)
    {
        var line1 = new Line("line1", 1);

        var station1 = new Station("station1", line1, 1);
        var station2 = new Station("station2", line1, 2);
        var station3 = new Station("station3", line1, 3);
        var station4 = new Station("station4", line1, 4);

        var map = MapBuilder.WithItems(mapId: 1,
            linesReference: new() { line1 },
            stationsReference: new() { station1, station2 },
            railwaysReference: new(),
            transfersReference: new());

        unexistent1 = station3;
        unexistent2 = station4;

        return map;
    }


    private bool IsValid(Map map, Station[] route)
    {
        for (int i = 0; i < route.Length - 1; i++)
        {
            var a = route[i];
            var b = route[i + 1];
            var valid =
                map.Railways.Any(x => x.From == a && x.To == b) ||
                map.Transfers.Any(x => x.From == a && x.To == b) ||
                map.Railways.Any(x => x.To == a && x.From == b) ||
                map.Transfers.Any(x => x.To == a && x.From == b);
            if (!valid)
            {
                return false;
            }
        }
        return true;
    }
}

