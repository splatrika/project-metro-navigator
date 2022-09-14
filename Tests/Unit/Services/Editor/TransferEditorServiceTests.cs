using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Exceptions;
using Splatrika.MetroNavigator.Source.Services.Editor;
using Splatrika.MetroNavigator.Tests.Unit.Builders;
using Moq;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Tests.Unit.Services.Editor;

public class TransferEditorServiceTests
{
    private const int mapId = 231;
    private const int transferId = 224;


    [Fact]
    public async Task CreateWithNoStations()
    {
        var line1 = new Line("L", id: 2);
        var station1 = new Station("Station1", line1, id: 41);

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line1 },
            stationsReference: new() { station1 });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithStationsAndLines(mapId, true));

        var service = new TransferEditorService(repository);

        await Assert.ThrowsAsync<EditorException>(async () =>
        {
            await service.Create(mapId);
        });
    }


    [Fact]
    public async Task Create()
    {
        Transfer? created = null;
        var saved = false;

        var line1 = new Line("L", id: 2);
        var station1 = new Station("Station1", line1, id: 41);
        var station2 = new Station("Station2", line1, id: 42);
        var stationIds = new int[] { station1.Id, station2.Id };

        var map = MapBuilder.WithItems(mapId,
            linesReference: new() { line1 },
            stationsReference: new() { station1, station2 },
            createCallbacks: new()
            {
                CreateTransfer = transfer =>
                {
                    created = transfer;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithStationsAndLines(mapId, true),
            saveCallback: () => saved = true);

        var service = new TransferEditorService(repository);

        await service.Create(mapId);
        Assert.NotNull(created);
        Assert.True(saved);
        Assert.Equal(TransferEditorService.DefaultDuration, created.Duration);
        Assert.NotEqual(created.To.Id, created.From.Id);
        Assert.Contains(created.From.Id, stationIds);
        Assert.Contains(created.To.Id, stationIds);
    }


    [Fact]
    public async Task Get()
    {
        var station1 = new Station("Station1", null, id: 41);
        var station2 = new Station("Station2", null, id: 42);
        var transfer = new Transfer(station1, station2,
            new StaticDuration(60), id: 101);

        var map = MapBuilder.WithItems(mapId,
            stationsReference: new() { station1, station2 },
            transfersReference: new() { transfer });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithTransferAndStationsBetween
                (mapId, transfer.Id, It.IsAny<bool>()));

        var service = new TransferEditorService(repository);
        var result = await service.Get(mapId, transfer.Id);
        Assert.Equal(mapId, result.MapId);
        Assert.Equal(transfer.Id, result.ElementId);
        Assert.Equal(transfer.From.Id, result.FromId);
        Assert.Equal(transfer.To.Id, result.ToId);
    }


    [Fact]
    public async Task UpdateDuration()
    {
        var station1 = new Station("Station1", null, id: 41);
        var station2 = new Station("Station2", null, id: 42);
        var transfer = new Transfer(station1, station2,
            new StaticDuration(60), id: 101);
        var saved = false;

        var map = MapBuilder.WithItems(mapId,
            stationsReference: new() { station1, station2 },
            transfersReference: new() { transfer });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetFull(mapId, true),
            saveCallback: () => saved = true);

        var updateArgs = new TransferEditorDto
        {
            MapId = mapId,
            ElementId = transfer.Id,
            FromId = transfer.From.Id,
            ToId = transfer.To.Id
        };

        var service = new TransferEditorService(repository);
        await service.Update(updateArgs);
        Assert.True(saved);
    }


    [Fact]
    public async Task UpdateStations()
    {
        var removedId = -1;
        Transfer? created = null;
        var saved = false;

        var station1 = new Station("Station1", null, id: 41);
        var station2 = new Station("Station2", null, id: 42);
        var station3 = new Station("Station1", null, id: 43);
        var transfer = new Transfer(station1, station2,
            new StaticDuration(60), id: 101);

        var map = MapBuilder.WithItems(mapId,
            stationsReference: new() { station1, station2, station3 },
            transfersReference: new() { transfer },
            removeCallbacks: new()
            {
                RemoveTransfer = id =>
                {
                    removedId = id;
                    saved = false;
                }
            },
            createCallbacks: new()
            {
                CreateTransfer = transfer =>
                {
                    created = transfer;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetFull(mapId, true),
            saveCallback: () => saved = true);

        var updateArgs = new TransferEditorDto
        {
            MapId = mapId,
            ElementId = transfer.Id,
            FromId = station3.Id,
            ToId = station1.Id
        };

        var service = new TransferEditorService(repository);
        await service.Update(updateArgs);
        Assert.Equal(transfer.Id, removedId);
        Assert.NotNull(created);
        Assert.Equal(station3.Id, created.From.Id);
        Assert.Equal(station1.Id, created.To.Id);
        Assert.True(saved);
    }


    [Fact]
    public async Task Remove()
    {
        var removedId = -1;
        var saved = false;

        var transfer = new Transfer(null, null, null, id: 101);

        var map = MapBuilder.WithItems(mapId,
            transfersReference: new() { transfer },
            removeCallbacks: new()
            {
                RemoveTransfer = id =>
                {
                    removedId = id;
                    saved = false;
                }
            });

        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithTransfer(mapId, transfer.Id, true),
            saveCallback: () => saved = true);

        var service = new TransferEditorService(repository);
        await service.Delete(mapId, transfer.Id);
        Assert.Equal(transfer.Id, removedId);
        Assert.True(saved);
    }
}

