using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;

namespace RecipeWebApp.Controllers;

public class CategoryController : Controller
{
    RecipeDbContext db;
    public CategoryController(RecipeDbContext db) => this.db = db;

    [HttpGet]
    [Route("/categories")]
    public async Task<IActionResult> GetCategory()
    {
        var categoties = await db.Categories.ToListAsync();

        return Json(categoties);
    }

}
