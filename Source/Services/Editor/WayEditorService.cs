using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public abstract class WayEditorService<T> : EditorService<T>
    where T : TransferEditorDto
{
    public static DurationFactor DefaultDuration => new StaticDuration(120);


    public WayEditorService(IMapRepository repository,
        Func<int, int, bool, Task<Map>> getWayAndStationsBetween)
    {
        throw new NotImplementedException();
    }


    public override sealed async Task<int> Create(int mapId)
    {
        throw new NotImplementedException();
    }


    public override sealed async Task Delete(int mapId, int elementId)
    {
        throw new NotImplementedException();
    }


    public override sealed async Task<T> Get(int mapId, int elementId)
    {
        throw new NotImplementedException();
    }


    public override sealed async Task<int> Update(T dto)
    {
        throw new NotImplementedException();
    }


    public abstract Task<Station[]> SelectDefaultStations();
    public abstract Task ValidateUpdate(T dto);
    public abstract Task AfterUpdate(T dto);
    public abstract Task AfterRecreate(T dto, int newId);

}

