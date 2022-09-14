using Microsoft.AspNetCore.Mvc;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Controllers.MapEditor;

public class RailwayController
    : EditorBase<RailwayEditorDto, IEditorService<RailwayEditorDto>, Railway>
{
    public RailwayController(RailwayEditorService service) : base(service)
    {
    }

    private RailwayController() : base(null) { }
}

