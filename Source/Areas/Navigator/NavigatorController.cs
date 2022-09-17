using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Splatrika.MetroNavigator.Source.Areas.Navigator;

[Area("Navigator")]
public class NavigatorController : Controller
{
    private readonly IMapRepository _repository;
    private readonly INavigationService _service;


    public NavigatorController(IMapRepository repository,
        INavigationService service)
    {
        _repository = repository;
        _service = service;
    }


    public async Task<IActionResult> Index(string? map, string? from, string? to)
    {
        var mapsList = await _repository.GetList();

        if (map == null)
        {
            if (mapsList.Count == 0)
            {
                return NotFound();
            }
            var defaultMap = mapsList.First();
            return RedirectToAction("Index",
                routeValues: new { map = defaultMap.Name });
        }

        var selectedMap = mapsList.FirstOrDefault(x => x.Name == map);

        if (selectedMap == null)
        {
            return NotFound();
        }

        if (from == null || to == null)
        {
            return View(new Navigation
            {
                MapId = selectedMap.Id
            });
        }

        selectedMap = await _repository.GetWithStations(selectedMap.Id,
            tracking: false);
        var fromStation = selectedMap.Stations.Single(x => x.Name == from);
        var toStation = selectedMap.Stations.Single(x => x.Name == to);

        var route = await _service.GetRoute(
            selectedMap.Id,
            fromStation?.Id ?? -1,
            toStation?.Id ?? -1);

        return View(new Navigation
        {
            MapId = selectedMap.Id,
            From = from,
            To = to,
            Route = route
        });
    }
}

