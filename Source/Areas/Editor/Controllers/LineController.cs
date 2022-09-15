using Microsoft.AspNetCore.Mvc;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Controllers.MapEditor;

public class LineController : EditorBase<LineEditorDto, IEditorService<LineEditorDto>, Line>
{
    public LineController(LineEditorService service) : base(service)
    {
    }

    private LineController() : base(null) { }
}


