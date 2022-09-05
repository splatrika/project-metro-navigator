using Moq;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Services;
using Splatrika.MetroNavigator.Tests.Unit.Builders;

namespace Splatrika.MetroNavigator.Tests.Unit.Services.Editor;

public class MapAppearanceServiceTests
{
    private const int mapId = 101;
    private const int lineId = 20;
    private const int stationId = 43;
    private const int railwayId = 493;


    [Fact]
    public async Task GetNewElement()
    {
        var appearance = MapAppearanceBuilder.WithItems(mapId);

        var repository = MapAppearanceRepositoryBuilder.WithSingle(appearance,
            allowedQuery: x => x.GetFullAsync(mapId, It.IsAny<bool>()));

        var service = new MapAppearanceService(repository);
        await CheckDefaults(service);

    }


    [Fact]
    public async Task GetNewMap()
    {
        var repository = MapAppearanceRepositoryBuilder.Empty();

        var service = new MapAppearanceService(repository);
        await CheckDefaults(service);
    }


    [Fact]
    public async Task Get()
    {
        var line = new LineAppearance(lineId, Color.RedColor);
        var station = new StationAppearance(stationId, new Position(2, 3),
            new Caption(Offset: new(2, -3), TextAlign.Center));
        var railway = new RailwayAppearance(railwayId, points: new List<Position>()
        {
            new(2, -3),
            new(2.5f, -2.5f)
        });

        var appearance = MapAppearanceBuilder.WithItems(mapId,
            linesReference: new() { line },
            stationsReference: new() { station },
            railwaysReference: new() { railway });

        var repository = MapAppearanceRepositoryBuilder.WithSingle(appearance,
            allowedQuery: x => x.GetFullAsync(mapId, It.IsAny<bool>()));

        var service = new MapAppearanceService(repository);

        var returnedLine = await service.GetLine(mapId, line.LineId);
        Assert.Same(line, returnedLine);

        var returnedStation = await service.GetStation(mapId, station.StationId);
        Assert.Same(station, returnedStation);

        var returnedRailway = await service.GetRailway(mapId, railway.RailwayId);
        Assert.Same(railway, returnedRailway);
    }


    [Fact]
    public async Task UpdateNewMap()
    {
        MapAppearance? createdAggregate = null;

        var repository = MapAppearanceRepositoryBuilder.Empty(
            addCallback: appearance => createdAggregate = appearance);
        var service = new MapAppearanceService(repository);

        await service.UpdateLine(mapId, lineId, Color.White);
        CheckCreated(createdAggregate);
        createdAggregate = null;

        await service.UpdateStation(mapId, stationId, new(), Caption.Default);
        CheckCreated(createdAggregate);
        createdAggregate = null;

        await service.UpdateRailway(mapId, railwayId, new List<Position>());
        CheckCreated(createdAggregate);

        static void CheckCreated(MapAppearance? createdAggregate)
        {
            Assert.NotNull(createdAggregate);
            Assert.Equal(mapId, createdAggregate.MapId);
        }
    }


    [Fact]
    public async Task Update()
    {
        LineAppearance? updatedLine = null;
        StationAppearance? updatedStation = null;
        RailwayAppearance? updatedRailway = null;
        var saved = false;

        var exceptedLine = new LineAppearance(lineId, Color.RedColor);
        var exceptedStation = new StationAppearance(stationId, new Position(2, 3),
            new Caption(Offset: new(2, -2), TextAlign.Right));
        var exceptedRailway = new RailwayAppearance(railwayId, points: new List<Position>()
        {
            new(2, -2),
            new(3, -3)
        });

        var appearance = MapAppearanceBuilder.WithItems(mapId,
            updateCallbacks: new()
            {
                UpdateLine = line =>
                {
                    updatedLine = line;
                    saved = false;
                },
                UpdateStation = staiton =>
                {
                    updatedStation = staiton;
                    saved = false;
                },
                UpdateRailway = railway =>
                {
                    updatedRailway = railway;
                    saved = false;
                }
            });

        var repository = MapAppearanceRepositoryBuilder.WithSingle(appearance,
            allowedQuery: x => x.GetFullAsync(mapId, true),
            saveCallback: () => saved = true);

        var service = new MapAppearanceService(repository);

        await service.UpdateLine(mapId, exceptedLine.LineId, exceptedLine.Color);
        Assert.Equal(exceptedLine, updatedLine);
        Assert.True(saved);

        await service.UpdateStation(mapId, exceptedStation.StationId,
            exceptedStation.Position, exceptedStation.Caption);
        Assert.Equal(exceptedStation, updatedStation);
        Assert.True(saved);

        await service.UpdateRailway(mapId, exceptedRailway.RailwayId,
            exceptedRailway.Points);
        Assert.Equal(exceptedRailway, updatedRailway);
        Assert.True(saved);
    }


    [Fact]
    public async Task CleanUp()
    {
        var cleanedLineId = -1;
        var cleanedStationId = -1;
        var cleanedRailwayId = -1;
        var saved = false;

        var appearance = MapAppearanceBuilder.WithItems(mapId,
            cleanUpCallbacks: new()
            {
                CleanUpStation = id =>
                {
                    cleanedStationId = id;
                    saved = false;
                },
                CleanUpLine = id =>
                {
                    cleanedLineId = id;
                    saved = false;
                },
                CleanUpRailway = id =>
                {
                    cleanedRailwayId = id;
                    saved = false;
                }
            });

        var repository = MapAppearanceRepositoryBuilder.WithSingle(appearance,
            allowedQuery: x => x.GetFullAsync(mapId, true),
            saveCallback: () => saved = true);

        var service = new MapAppearanceService(repository);

        await service.CleanUpLine(mapId, lineId);
        Assert.True(saved);
        Assert.Equal(lineId, cleanedLineId);

        await service.CleanUpStation(mapId, stationId);
        Assert.True(saved);
        Assert.Equal(stationId, cleanedStationId);

        await service.CleanUpRailway(mapId, railwayId);
        Assert.True(saved);
        Assert.Equal(railwayId, cleanedRailwayId);

    }


    private async Task CheckDefaults(MapAppearanceService service)
    {
        var railway = await service.GetRailway(mapId, railwayId: 100);
        Assert.Equal(RailwayAppearance.GetDefault(id: 100), railway);

        var station = await service.GetStation(mapId, stationId: 50);
        Assert.Equal(StationAppearance.GetDefault(id: 50), station);

        var line = await service.GetLine(mapId, lineId: 22);
        Assert.Equal(LineAppearance.GetDefault(id: 22), line);
    }
}

