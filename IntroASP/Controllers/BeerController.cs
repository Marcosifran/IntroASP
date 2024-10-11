using IntroASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IntroASP.Controllers;

public class BeerController : Controller
{
    private readonly PostgresContext _context;

    public BeerController(PostgresContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var beers = _context.Beers.Include(b => b.Brand);
        return View(await beers.ToListAsync());
    }

    public IActionResult Create()
    {
        ViewData["Brand"] = new SelectList(_context.Brands, "brandId","name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult>  Create(BeerViewModel model)
    {
        if (ModelState.IsValid)
        {
            var beer = new Beer()
            {
                Name = model.name,
                BrandId = model.brandId
            };
            _context.Add(beer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["Brand"] = new SelectList(_context.Brands, "brandId", "name",model.brandId);
        return View(model);
    }
}
