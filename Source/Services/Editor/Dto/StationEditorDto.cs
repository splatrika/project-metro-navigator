using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Services.Editor.Dto;

#nullable disable

public class StationEditorDto : EditorDto
{
    public string Name { get; set; }
    public int LineId { get; set; }
    public Position Position { get; set; }
    public Caption Caption { get; set; }
}

