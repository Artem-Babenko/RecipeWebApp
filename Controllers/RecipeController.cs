using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;

namespace RecipeWebApp.Controllers;

public class RecipeController : Controller
{
    RecipeDbContext db;
    public RecipeController(RecipeDbContext db) => this.db = db;

    /// <summary>
    /// Надсилає рецепти з бази даних.
    /// </summary>
    [HttpGet]
    [Route("/recipes")]
    public async Task<IActionResult> GetRecipes()
    {
        var recipes = await db.Recipes
            .Include(recipe => recipe.Category)
            .Include(recipe => recipe.Comments)
            .ToListAsync();

        return Json(recipes);
    }

    /// <summary>
    /// Надсилає рецепт з бази даних, використовуючи ідентифікатор.
    /// </summary>
    [HttpGet]
    [Route("/recipe/{id:int}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        var recipe = await db.Recipes
            .Include(recipe => recipe.Category)
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.User)
            .Include(recipe => recipe.CookingSteps)
            .Include(recipe => recipe.Comments)
            .FirstOrDefaultAsync(recipe => recipe.Id == id);

        if (recipe is null) return NotFound();

        return Json(recipe);
    }
}
