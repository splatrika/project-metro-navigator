using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Exceptions;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public abstract class EditorService<T> : IEditorService<T>
    where T : EditorDto
{
    protected readonly IMapRepository _repository;


    public EditorService(IMapRepository repository)
    {
        _repository = repository;
    }


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


    protected async Task CheckMap(int mapId)
    {
        if (!await _repository.ContainsAsync(mapId))
        {
            throw new EditorException("There is no map with given id");
        }
    }


    protected TElement GetElement<TElement>(IEnumerable<TElement> collection,
        int elementId)
        where TElement : EntityBase
    {
        var element = collection.SingleOrDefault(x => x.Id == elementId);
        if (element == null) throw new EditorException("Element not found");
        return element;
    }
}

