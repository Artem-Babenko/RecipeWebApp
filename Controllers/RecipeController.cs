using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;

namespace RecipeWebApp.Controllers;

public class RecipeController : Controller
{
    RecipeDbContext db;
    public RecipeController(RecipeDbContext db) => this.db = db;

    /// <summary>
    /// Метод, який повертає рецепти з бази даних.
    /// </summary>
    /// <returns>Дані про всі рецепти у вигляді JSON.</returns>
    [HttpGet]
    [Route("/recipes")]
    public async Task<IActionResult> GetRecipes()
    {
        var recipes = await db.Recipes.ToListAsync();
        return Json(recipes); 
    }
}
