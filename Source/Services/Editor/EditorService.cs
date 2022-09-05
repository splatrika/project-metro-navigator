using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public abstract class EditorService<T> : IEditorService
    where T : EditorDto
{
    public abstract Task<int> Create(int mapId);
    public abstract Task Delete(int mapId, int elementId);
    public abstract Task<T> Get(int mapId, int elementId);
    public abstract Task<int> Update(T dto);


    async Task<IEditorDto> IEditorService.Read(int mapId, int elementId)
    {
        return await Get(mapId, elementId);
    }


    async Task<int> IEditorService.Update(IEditorDto dto)
    {
        return await Update((T)dto);
    }
}

