using System;
using Microsoft.AspNetCore.Mvc;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Controllers.MapEditor;

[Area("Editor")]
public abstract class EditorBase<TDto, TService, TEditedType>
    : Controller, IEditorController
    where TDto : IEditorDto
    where TService : IEditorService<TDto>
{
    public Type EditedType => typeof(TEditedType);
    public string ControllerName => GetType().Name.Replace("Controller", "");

    private readonly TService _service;


    protected EditorBase(TService service)
    {
        _service = service;
    }


    [HttpGet]
    public virtual async Task<IActionResult> Edit(int mapId, int elementId)
    {
        var properties = await _service.Read(mapId, elementId);
        return View(properties);
    }


    [HttpPost]
    public virtual async Task<IActionResult> Update(int mapId, TDto args)
    {
        var updatedId = await _service.Update(args);
        return RedirectToAction(nameof(Edit),
            routeValues: new { mapId = mapId, elementId = updatedId });
    }


    [HttpPost]
    public virtual async Task<IActionResult> Create(int mapId)
    {
        var createdId = await _service.Create(mapId);
        return RedirectToAction(nameof(Edit),
            routeValues: new { mapId = mapId, elementId = createdId });
    }


    [HttpPost]
    public virtual async Task<IActionResult> Delete(int mapId, int elementId)
    {
        await _service.Delete(mapId, elementId);
        return RedirectToAction(
            actionName: "Index",
            controllerName: "NoSelected");
    }
}

