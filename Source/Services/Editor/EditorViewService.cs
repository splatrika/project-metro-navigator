using System;
using System.Reflection;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.ViewModels;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public class EditorViewService
    : IEditorViewService
{
    public string CreateAction => "Create";
    public string EditAction => "Edit";
    public string DeleteAction => "Delete";

    private Dictionary<Type, string> _registry;
    private readonly IMapRepository _repository;

    public IEnumerable<string> Controllers => _registry.Values;


    public EditorViewService(IMapRepository repository)
    {
        _repository = repository;
        _registry = new();
        SetupRegistry();
    }


    public Task<string> GetController(Type type)
    {
        if (!_registry.ContainsKey(type))
        {
            throw new InvalidOperationException($"There is no editor " +
                $"controller for type {type.Name}");
        }
        return Task.FromResult(_registry[type]);
    }


    public async Task<IEnumerable<ListElement>> GetMapElements(int mapId)
    {
        var map = await _repository.GetFull(mapId);
        var result = new List<ListElement>();
        var collections = new List<IEnumerable<EntityBase>>()
        {
            map.Lines,
            map.Stations,
            map.Railways,
            map.Transfers
        };
        foreach (var list in collections)
        {
            foreach (var element in list)
            {
                result.Add(
                    new(name: element.ToString() ?? "Unknown",
                        controller: await GetController(element.GetType()),
                        elementId: element.Id));
            }
        }
        var additional = await GetMapElements2(mapId);
        if (additional != null) result.AddRange(additional);
        return result;
    }


    protected virtual Task<IEnumerable<ListElement>?> GetMapElements2(int mapId)
    {
        return Task.FromResult<IEnumerable<ListElement>?>(null);
    }


    private void SetupRegistry()
    {
        var assembly = typeof(EditorViewService).Assembly;
        var types = assembly.GetTypes()
            .Where(x => x.GetInterfaces()
                .Any(x => x == typeof(IEditorController)))
            .Where(x => !x.IsAbstract);

        var instances = types
            .Select(x => (IEditorController)Activator.CreateInstance(x,
                nonPublic: true)!);

        foreach (var x in instances)
        {
            _registry.Add(x.EditedType, x.ControllerName);
        }
    }
}

