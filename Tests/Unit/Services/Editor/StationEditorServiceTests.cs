using System.Reflection;
using System.Linq.Expressions;
using Moq;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Exceptions;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;
using Splatrika.MetroNavigator.Tests.Unit.Builders;

namespace Splatrika.MetroNavigator.Tests.Unit.Services.Editor;

public class StationEditorServiceTests
{
    private const int mapId = 12;
    private const int lineId = 22;
    private const int stationId = 101;


    [Fact]
    public void Create()
    {
        Station? created = null;

        var line = new Line("Line", lineId);
        var saved = false;

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line },
            createCallbacks: new()
            {
                CreateStaiton = station =>
                {
                    created = station;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithLines(mapId, true),
            saveCallback: () => saved = true);

        var appearanceService = AppearanceServiceBuilder.Empty();
        var service = new StationEditorService(repository, appearanceService);

        service.Create(mapId);
        Assert.NotNull(created);
        Assert.True(saved);
        Assert.Equal(StationEditorService.DefaultName, created.Name);
    }


    [Fact]
    public async Task CreateWithNoLines()
    {
        var map = MapBuilder.WithItems(mapId,
            linesReference: new());

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithLines(mapId, true));

        var appearanceService = AppearanceServiceBuilder.Empty();
        var service = new StationEditorService(repository, appearanceService);

        await Assert.ThrowsAsync<EditorException>(
            async () => await service.Create(mapId));
    }


    [Fact]
    public async Task Get()
    {
        var line = new Line("Line", lineId);
        var station = new Station("Station", line, stationId);
        var appearance = new StationAppearance(station.Id,
            new Position(2, -3.4f),
            new Caption(Offset: new(2, -3), TextAlign.Center));

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line },
            stationsReference: new() { station });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x =>
                x.GetWithStationsAndLines(mapId, It.IsAny<bool>()));

        var appearanceService = AppearanceServiceBuilder
            .WithConfiguredStation(mapId, appearance);

        var service = new StationEditorService(repository, appearanceService);

        var result = await service.Get(mapId, station.Id);
        Assert.Equal(mapId, result.MapId);
        Assert.Equal(station.Id, result.ElementId);
        Assert.Equal(station.Name, result.Name);
        Assert.Equal(station.Line.Id, result.LineId);
        Assert.Equal(appearance.Position, result.Position);
        Assert.Equal(appearance.Caption, result.Caption);
    }


    [Fact]
    public async Task UpdateNameAndPosition()
    {
        var line = new Line("Line", lineId);
        var station = new Station("Station", line, stationId);
        StationAppearance? updatedAppearance = null;
        var saved = false;

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line },
            stationsReference: new() { station });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetFull(mapId, true),
            saveCallback: () => saved = true);

        var appearanceService = AppearanceServiceBuilder.Empty(
            updateCallbacks: new()
            {
                UpdateStationCallback =
                    (map, stationId, position, caption) =>
                    {
                        if (map == mapId)
                        {
                            updatedAppearance = new(stationId, position, caption);
                        }
                    }
            });

        var service = new StationEditorService(repository, appearanceService);
        var updateArgs = new StationEditorDto
        {
            MapId = mapId,
            ElementId = station.Id,
            LineId = station.Line.Id,
            Name = "London",
            Position = new(2, 30),
            Caption = new(Offset: new(-20, -20), TextAlign.Right)
        };

        await service.Update(updateArgs);
        Assert.NotNull(updatedAppearance);
        Assert.True(saved);
        Assert.Equal(updateArgs.Name, station.Name);
        Assert.Equal(updateArgs.Position, updatedAppearance.Position);
        Assert.Equal(updateArgs.Caption, updatedAppearance.Caption);
        Assert.Equal(updateArgs.ElementId, updatedAppearance.StationId);
    }


    [Fact]
    public async Task UpdateLine()
    {
        var newId = stationId + 100;

        var removedStationId = -1;
        var removedAppearanceId = -1;
        var saved = false;

        Station? created = null;
        StationAppearance? createdApearance = null;

        var line1 = new Line("Line1", lineId);
        var line2 = new Line("Line2", line1.Id + 10);
        var lastStation = new Station("Station", line1, stationId);

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line1, line2 },
            stationsReference: new() { lastStation },
            removeCallbacks: new()
            {
                RemoveStation = id =>
                {
                    removedStationId = id;
                    saved = false;
                }
            },
            createCallbacks: new()
            {
                CreateStaiton = station =>
                {
                    created = station;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetFull(mapId, true),
            saveCallback: () => {
                saved = true;
                if (created != null) EntityUtility.ChangeId(created, newId);
            });

        var appearanceService = AppearanceServiceBuilder.Empty(
            cleanUpCallbacks: new()
            {
                CleanUpStationCallback = (map, elementId) =>
                {
                    if (map == mapId) removedAppearanceId = elementId;
                }
            },
            updateCallbacks: new()
            {
                UpdateStationCallback =
                (map, elementId, position, caption) =>
                {
                    if (map == mapId) createdApearance =
                        new(elementId, position, caption);
                }
            });

        var updateArgs = new StationEditorDto
        {
            MapId = mapId,
            ElementId = lastStation.Id,
            LineId = line2.Id,
            Name = lastStation.Name,
            Position = new(2, 35),
            Caption = new(Offset: new(2, -4), TextAlign.Center)
        };

        var service = new StationEditorService(repository, appearanceService);
        await service.Update(updateArgs);
        Assert.Equal(lastStation.Id, removedStationId);
        Assert.Equal(lastStation.Id, removedAppearanceId);
        Assert.NotNull(created);
        Assert.NotNull(createdApearance);
        Assert.Equal(newId, created.Id);
        Assert.Equal(line2.Id, created.Line.Id);
        Assert.Equal(updateArgs.Name, created.Name);
        Assert.Equal(newId, createdApearance.StationId);
        Assert.Equal(updateArgs.Position, createdApearance.Position);
        Assert.Equal(updateArgs.Caption, createdApearance.Caption);
        Assert.True(saved);
    }


    [Fact]
    public async Task Remove()
    {
        var station = new Station("Station", null, stationId);
        var removedStationId = -1;
        var removedAppearanceId = -1;
        var saved = false;

        var map = MapBuilder.WithItems(mapId,
            stationsReference: new() { station },
            removeCallbacks: new()
            {
                RemoveStation = id =>
                {
                    removedStationId = id;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithStation(mapId, stationId, true),
            saveCallback: () => saved = true);

        var appearanceService = AppearanceServiceBuilder.Empty(
            cleanUpCallbacks: new()
            {
                CleanUpStationCallback = (map, elementId) =>
                {
                    if (map == mapId) removedAppearanceId = elementId;
                }
            });

        var service = new StationEditorService(repository, appearanceService);
        await service.Delete(mapId, station.Id);

        Assert.Equal(station.Id, removedStationId);
        Assert.Equal(station.Id, removedAppearanceId);
        Assert.True(saved);
    }
}

