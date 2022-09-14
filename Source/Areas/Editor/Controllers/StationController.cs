using Microsoft.AspNetCore.Mvc;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Controllers.MapEditor;

public class StationController
    : EditorBase<StationEditorDto, IEditorService<StationEditorDto>, Station>
{
    public StationController(StationEditorService service) : base(service)
    {
    }

    private StationController() : base(null) { }
}

