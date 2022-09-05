using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.Services.Editor.Dto;

#nullable disable

public class TransferEditorDto : EditorDto
{
    public int FromId { get; set; }
    public int ToId { get; set; }
    public DurationFactor Duration { get; set; }
}

