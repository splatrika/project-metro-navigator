using Splatrika.MetroNavigator.Source.ViewModels;

namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface IEditorViewService
{
    string CreateAction { get; }
    string EditAction { get; }
    string DeleteAction { get; }
    IEnumerable<string> Controllers { get; }

    Task<string> GetController(Type type);
    Task<IEnumerable<ListElement>> GetMapElements(int mapId);
}

