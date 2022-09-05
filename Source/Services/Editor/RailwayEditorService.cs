
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public class RailwayEditorService : WayEditorService<RailwayEditorDto>
{
    public RailwayEditorService(IMapRepository repository,
        IMapAppearanceService appearance) : base(repository, null)
    {
        throw new NotImplementedException();
    }

    public override Task AfterRecreate(RailwayEditorDto dto, int newId)
    {
        throw new NotImplementedException();
    }

    public override Task AfterUpdate(RailwayEditorDto dto)
    {
        throw new NotImplementedException();
    }

    public override Task<Station[]> SelectDefaultStations()
    {
        throw new NotImplementedException();
    }

    public override Task ValidateUpdate(RailwayEditorDto dto)
    {
        throw new NotImplementedException();
    }
}

