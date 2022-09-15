using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.ViewModels;

namespace Splatrika.MetroNavigator.Source.Areas.Admin;

[Area("Admin")]
public class ManageMapsController : Controller
{
    private readonly IMapRepository _repository;


    public ManageMapsController(IMapRepository repository)
    {
        _repository = repository;
    }

    
    public async Task<IActionResult> Index()
    {
        var list = await _repository.GetList();
        return View(list);
    }

    
    public async Task<IActionResult> Details(int id)
    {
        if (!await _repository.ContainsAsync(id))
        {
            return NotFound();
        }

        var map = await _repository.GetAsync(id, tracking: false);
        return View(map);
    }

    
    public IActionResult Create()
    {
        return View();
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] MapManage model)
    {
        var map = new Map(model.Name);
        if (ModelState.IsValid)
        {
            await _repository.AddAsync(map);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(map);
    }

    
    public async Task<IActionResult> Edit(int id)
    {
        if (!await _repository.ContainsAsync(id))
        {
            return NotFound();
        }
        var map = await _repository.GetAsync(id);
        return View(map);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Name")] MapManage model)
    {
        if (id != model.Id || !await _repository.ContainsAsync(model.Id))
        {
            return NotFound();
        }

        var map = await _repository.GetAsync(id);

        if (ModelState.IsValid)
        {
            try
            {
                map.Name = model.Name;
                await _repository.UpdateAsync(map);
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ContainsAsync(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(map);
    }

    
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _repository.ContainsAsync(id))
        {
            return NotFound();
        }
        var map = await _repository.GetAsync(id);

        return View(map);
    }

    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!await _repository.ContainsAsync(id))
        {
            return NotFound();
        }
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
