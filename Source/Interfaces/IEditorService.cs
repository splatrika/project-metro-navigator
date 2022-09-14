namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface IEditorService
{
    Task<int> Create(int mapId);
    Task<IEditorDto> Read(int mapId, int elementId);
    Task<int> Update(IEditorDto dto);
    Task Delete(int mapId, int elementId);
}


public interface IEditorService<TDto> : IEditorService
{
    Task<TDto> Get(int mapId, int elementId);
    Task<int> Update(TDto dto);
}