using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Exceptions;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public class TransferEditorService : EditorService<TransferEditorDto>
{
    public static DurationFactor DefaultDuration => new StaticDuration(120);


    public TransferEditorService(IMapRepository repository) : base(repository)
    {
    }


    public override async Task<int> Create(int mapId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithStations(mapId);
        var from = map.Stations.FirstOrDefault();
        var to = map.Stations.FirstOrDefault(x => x.Id != from?.Id);
        if (from == null || to == null)
        {
            throw new EditorException("There is no stations to create transfer " +
                "between them");
        }
        var transfer = map.CreateTransfer(from.Id, to.Id, DefaultDuration);
        await _repository.SaveChangesAsync();
        return transfer.Id;
    }
    

    public override async Task Delete(int mapId, int elementId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithTransfer(mapId, elementId);
        map.RemoveTransfer(elementId);
        await _repository.SaveChangesAsync();
    }


    public override async Task<TransferEditorDto> Get(int mapId, int elementId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithTransferAndStationsBetween(mapId,
            elementId, tracking: false);
        var transfer = GetElement(map.Transfers, elementId);
        return new()
        {
            MapId = mapId,
            ElementId = elementId,
            FromId = transfer.From.Id,
            ToId = transfer.To.Id
        };
    }


    public override async Task<int> Update(TransferEditorDto dto)
    {
        await CheckMap(dto.MapId);
        var map = await _repository.GetFull(dto.MapId);
        var transfer = GetElement(map.Transfers, dto.ElementId);

        var stationsChanged = dto.FromId != transfer.From.Id ||
            dto.ToId != transfer.To.Id;
        if (stationsChanged)
        {
            map.RemoveTransfer(transfer.Id);
            transfer = map.CreateTransfer(dto.FromId, dto.ToId, transfer.Duration);
            await _repository.SaveChangesAsync();
            return transfer.Id;
        }
        await _repository.SaveChangesAsync();
        return transfer.Id;
    }
}

