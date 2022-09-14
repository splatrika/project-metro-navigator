using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.ViewModels;

public class InspectorActions
{
    public string InspectorId { get; set; }
    public IEditorDto Element { get; set; }

    public InspectorActions(string inspectorId, IEditorDto element)
    {
        InspectorId = inspectorId;
        Element = element;
    }
}

