using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

#nullable disable

public class RailwayEditorDto : TransferEditorDto
{
    public List<Position> Points { get; set; }
}

