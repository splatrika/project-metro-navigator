using Moq;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor;
using Splatrika.MetroNavigator.Tests.Unit.Builders;

namespace Splatrika.MetroNavigator.Tests.Unit.Services.Editor;

public class LineEditorServiceTests
{
    const int mapId = 101;
    const int lineId = 23;
    private readonly Color DefaultColor = Color.White;


    [Fact]
    public async Task Create()
    {
        Station? created = null;
        var saved = false;

        var map = MapBuilder.WithItems(mapId,
            createCallbacks: new()
            {
                CreateStaiton = station =>
                {
                    created = station;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetAsync(mapId, true),
            saveCallback: () => saved = true);

        var appearanceService = AppearanceServiceBuilder.Empty();

        var service = new LineEditorService(repository, appearanceService);

        await service.Create(mapId);
        Assert.NotNull(created);
        Assert.Equal(LineEditorService.DefaultName, created.Name);
        Assert.True(saved);
    }


    [Fact]
    public async Task Get()
    {
        var line = new Line("London", lineId);
        var lines = new List<Line>() { line };
        var appearance = new LineAppearance(lineId, Color.RedColor);

        var map = MapBuilder.WithItems(mapId, linesReference: lines);

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithLine(mapId, lineId, It.IsAny<bool>()));
        var appearnaceService = AppearanceServiceBuilder
            .WithConfiguredLine(mapId, appearance);

        var service = new LineEditorService(repository, appearnaceService);
        var result = await service.Get(mapId, lineId);

        Assert.Equal(mapId, result.MapId);
        Assert.Equal(line.Id, result.ElementId);
        Assert.Equal(line.Name, result.Name);
        Assert.Equal(appearance.Color, result.Color);
    }


    [Fact]
    public async Task Update()
    {
        var line = new Line("Untitled", lineId);
        var lines = new List<Line> { line };
        var saved = false;
        LineAppearance? appearance = null;

        var map = MapBuilder.WithItems(mapId, linesReference: lines);

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithLine(mapId, lineId, true),
            saveCallback: () => saved = true);

        var appearanceService = AppearanceServiceBuilder.Empty(
            updateCallbacks: new()
            {
                UpdateLineCallback = (map, lineId, color) =>
                {
                    if (map == mapId) appearance = new(lineId, color);
                }
            });

        var service = new LineEditorService(repository, appearanceService);

        var updateArgs = new LineDto
        {
            MapId = mapId,
            ElementId = lineId,
            Name = "London",
            Color = Color.RedColor
        };

        await service.Update(updateArgs);
        Assert.Equal(updateArgs.Name, line.Name);
        Assert.True(saved);
        Assert.NotNull(appearance);
        Assert.Equal(updateArgs.ElementId, appearance.LineId);
        Assert.Equal(updateArgs.Color, appearance.Color);
    }


    [Fact]
    public async Task Remove()
    {
        var line = new Line("London", lineId);
        var lines = new List<Line> { line };
        var removedAppearanceId = -1;
        var removedLineId = -1;
        var saved = false;

        var map = MapBuilder.WithItems(mapId,
            linesReference: lines,
            removeCallbacks: new()
            {
                RemoveLine = id =>
                {
                    removedLineId = id;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithLine(mapId, lineId, true),
            saveCallback: () => saved = true);

        var appearanceService = AppearanceServiceBuilder.Empty(
            cleanUpCallbacks: new()
            {
                CleanUpLineCallback = (map, lineId) =>
                {
                    if (map == mapId) removedAppearanceId = lineId;
                }
            });

        var service = new LineEditorService(repository, appearanceService);

        await service.Delete(mapId, lineId);

        Assert.Equal(lineId, removedLineId);
        Assert.Equal(lineId, removedAppearanceId);
        Assert.True(saved);
    }
}
