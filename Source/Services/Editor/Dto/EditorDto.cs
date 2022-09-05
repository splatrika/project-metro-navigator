using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public abstract class EditorDto : IEditorDto
{
    public int MapId { get; set; }
    public int ElementId { get; set; }
}

