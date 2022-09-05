using System.Reflection;
using Moq;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Exceptions;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor;
using Splatrika.MetroNavigator.Tests.Unit.Builders;

namespace Splatrika.MetroNavigator.Tests.Unit.Services.Editor;

public class RailwayEditorServiceTests
{
    private const int mapId = 231;
    private const int railwayId = 224;
    const int durationId = 21;

    [Fact]
    public async Task CreateWithNoStations()
    {
        var line1 = new Line("Line1", 12);
        var line2 = new Line("Line2", 13);
        var station1 = new Station("Station1", line1, 101);
        var station2 = new Station("Station2", line2, 102);

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line1, line2 },
            stationsReference: new() { station1, station2 });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithStationsAndLines(mapId, true));

        var appearanceService = AppearanceServiceBuilder.Empty();
        var service = new RailwayEditorService(repository, appearanceService);
        await Assert.ThrowsAsync<EditorException>(async () =>
        {
            await service.Create(mapId);
        });
    }


    [Fact]
    public async Task Create()
    {
        Railway? created = null;
        var saved = false;

        var line = new Line("Line", 13);
        var station1 = new Station("Station1", line, 101);
        var station2 = new Station("Station2", line, 102);
        var stationIds = new int[] { station1.Id, station2.Id };

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line },
            stationsReference: new() { station1, station2 },
            createCallbacks: new()
            {
                CreateRailway = railway =>
                {
                    created = railway;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithStationsAndLines(mapId, true),
            saveCallback: () => saved = true);

        var appearanceService = AppearanceServiceBuilder.Empty();

        var service = new RailwayEditorService(repository, appearanceService);
        await service.Create(mapId);
        Assert.NotNull(created);
        Assert.True(saved);
        Assert.Equal(RailwayEditorService.DefaultDuration, created.Duration);
        Assert.NotEqual(created.From.Id, created.To.Id);
        Assert.Contains(created.From.Id, stationIds);
        Assert.Contains(created.To.Id, stationIds);
    }


    [Fact]
    public async Task Get()
    {
        var station1 = new Station("Station1", null, 101);
        var station2 = new Station("Station2", null, 102);
        var railway = new Railway(station1, station2,
            new StaticDuration(202), railwayId);
        var appearance = new RailwayAppearance(railway.Id,
            points: new Position[]
            {
                new(2, 3),
                new(-3, 3.5f)
            });

        var map = MapBuilder.WithItems(mapId,
            stationsReference: new() { station1, station2 },
            railwaysReference: new() { railway });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithRailwayAndStationsBetween
                (mapId, railway.Id, It.IsAny<bool>()));

        var appearanceService = AppearanceServiceBuilder
            .WithConfiguredRailway(mapId, appearance);

        var service = new RailwayEditorService(repository, appearanceService);
        var result = await service.Get(mapId, railway.Id);
        Assert.Equal(mapId, result.MapId);
        Assert.Equal(railway.Id, result.ElementId);
        Assert.Equal(railway.From.Id, result.FromId);
        Assert.Equal(railway.To.Id, result.ToId);
        Assert.Equal(railway.Duration, result.Duration);
        Assert.True(result.Points.SequenceEqual(appearance.Points));
    }


    [Fact]
    public async Task UpdateDurationAndAppearance()
    {
        RailwayAppearance? createdAppearance = null;
        var saved = false;

        var line = new Line("Line", 13);
        var station1 = new Station("Station1", line, 101);
        var station2 = new Station("Station2", line, 102);
        var railway = new Railway(station1, station2,
            new StaticDuration(60), railwayId);

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line },
            stationsReference: new() { station1, station2 },
            railwaysReference: new() { railway });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetFull(mapId, true),
            saveCallback: () => saved = false);

        var appearanceService = AppearanceServiceBuilder.Empty(
            updateCallbacks: new()
            {
                UpdateRailwayCallback = (map, railwayId, points) =>
                {
                    if (map == mapId)
                        createdAppearance = new(railwayId, points);
                }
            });

        var updateArgs = new RailwayEditorDto
        {
            MapId = mapId,
            ElementId = railway.Id,
            FromId = railway.From.Id,
            ToId = railway.To.Id,
            Duration = new StaticDuration(40),
            Points = new()
            {
                new(2, 3),
                new(-3.5f, -7)
            }
        };

        var service = new RailwayEditorService(repository, appearanceService);
        await service.Update(updateArgs);
        Assert.True(saved);
        Assert.Equal(updateArgs.Duration, railway.Duration);
        Assert.NotNull(createdAppearance);
        Assert.Equal(railway.Id, createdAppearance.RailwayId);
        Assert.True(createdAppearance.Points.SequenceEqual(updateArgs.Points));
    }


    [Fact]
    public async Task UpdateStations()
    {
        var newId = 500;

        Railway? created = null;
        RailwayAppearance? createdAppearance = null;
        var removedId = -1;
        var removedAppearanceId = -1;
        var saved = false;

        var line1 = new Line("Line1", 13);
        var line2 = new Line("Line2", 13);
        var station1 = new Station("Station1", line1, 101);
        var station2 = new Station("Station2", line1, 102);
        var station3 = new Station("Station1", line2, 103);
        var station4 = new Station("Station2", line2, 104);
        var railway = new Railway(station1, station2,
            new StaticDuration(100), railwayId);

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line1, line2 },
            stationsReference: new() { station1, station2, station3, station4 },
            railwaysReference: new() { railway },
            removeCallbacks: new()
            {
                RemoveRailway = id =>
                {
                    removedId = id;
                    saved = false;
                }
            },
            createCallbacks: new()
            {
                CreateRailway = railway =>
                {
                    created = railway;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetFull(mapId, true),
            saveCallback: () =>
            {
                saved = true;
                if (created != null) EntityUtility.ChangeId(created, newId);
            });

        var appearanceService = AppearanceServiceBuilder.Empty(
            cleanUpCallbacks: new()
            {
                CleanUpRailwayCallback = (map, railwayId) =>
                {
                    if (map == mapId) removedAppearanceId = railwayId;
                }
            },
            updateCallbacks: new()
            {
                UpdateRailwayCallback = (map, railwayId, points) =>
                {
                    if (map == mapId)
                        createdAppearance = new(railwayId, points);
                }
            });

        var updateArgs = new RailwayEditorDto
        {
            MapId = mapId,
            ElementId = railway.Id,
            FromId = station3.Id,
            ToId = station4.Id,
            Duration = new StaticDuration(40),
            Points = new()
        };

        var service = new RailwayEditorService(repository, appearanceService);
        await service.Update(updateArgs);
        Assert.Equal(railway.Id, removedId);
        Assert.Equal(railway.Id, removedAppearanceId);
        Assert.True(saved);
        Assert.NotNull(created);
        Assert.NotNull(createdAppearance);
        Assert.Equal(station3.Id, created.From.Id);
        Assert.Equal(station4.Id, created.To.Id);
        Assert.Equal(newId, created.Id);
        Assert.Equal(newId, createdAppearance.RailwayId);
    }


    [Fact]
    public async Task Remove()
    {
        var removedId = -1;
        var removedAppearanceId = -1;
        var saved = false;

        var railway = new Railway(null, null, null, railwayId);

        var map = MapBuilder.WithItems(mapId,
            railwaysReference: new() { railway },
            removeCallbacks: new()
            {
                RemoveRailway = id =>
                {
                    removedId = id;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithRailway(mapId, railway.Id, true),
            saveCallback: () => saved = true);

        var appearanceService = AppearanceServiceBuilder.Empty(
            cleanUpCallbacks: new()
            {
                CleanUpRailwayCallback = (map, railwayId) =>
                {
                    if (map == mapId) removedAppearanceId = railwayId;
                }
            });

        var service = new RailwayEditorService(repository, appearanceService);
        await service.Delete(mapId, railwayId);
        Assert.Equal(railway.Id, removedId);
        Assert.Equal(railway.Id, removedAppearanceId);
        Assert.True(saved);
    }
}
