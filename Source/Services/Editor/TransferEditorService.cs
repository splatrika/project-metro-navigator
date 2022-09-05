using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public class TransferEditorService : WayEditorService<TransferEditorDto>
{
    public TransferEditorService(IMapRepository repository)
        : base(repository, (mapId, wayId, tracking) => repository
            .GetWithTransferAndStationsBetween(mapId, wayId, tracking))
    {
        throw new NotImplementedException();
    }

    public override Task AfterRecreate(TransferEditorDto dto, int newId)
    {
        throw new NotImplementedException();
    }

    public override Task AfterUpdate(TransferEditorDto dto)
    {
        throw new NotImplementedException();
    }

    public override Task<Station[]> SelectDefaultStations()
    {
        throw new NotImplementedException();
    }

    public override Task ValidateUpdate(TransferEditorDto dto)
    {
        throw new NotImplementedException();
    }
}

