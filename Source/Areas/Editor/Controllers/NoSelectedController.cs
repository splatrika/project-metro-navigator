using System;
using Microsoft.AspNetCore.Mvc;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Controllers.MapEditor;

[Area("Editor")]
public class NoSelectedController : Controller
{
    private readonly IMapRepository _repository;

    public NoSelectedController(IMapRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index(int mapId)
    {
        if (!await _repository.ContainsAsync(mapId))
        {
            return NotFound();
        }
        return View(mapId);
    }
}
