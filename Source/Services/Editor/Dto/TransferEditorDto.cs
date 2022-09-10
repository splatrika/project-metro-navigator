using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services.Editor.Dto;

#nullable disable

public class TransferEditorDto : EditorDto, IWayEditorDto
{
    public int FromId { get; set; }
    public int ToId { get; set; }
}

