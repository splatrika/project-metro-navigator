using Microsoft.AspNetCore.Mvc;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Controllers.MapEditor;

public class TransferController
    : EditorBase<TransferEditorDto, IEditorService<TransferEditorDto>, Transfer>
{
    public TransferController(TransferEditorService service) : base(service)
    {
    }

    private TransferController() : base(null) { }
}