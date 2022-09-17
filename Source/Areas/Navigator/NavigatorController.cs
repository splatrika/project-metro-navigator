using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Splatrika.MetroNavigator.Source.Interfaces;

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
        throw new NotImplementedException();
    }
}

