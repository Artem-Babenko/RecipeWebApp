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
    [Route("/recipes/{id:int}")]
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

    /// <summary>
    /// Отримує інформацію про коментар та додає його до рецепту.
    /// </summary>
    [HttpPost]
    [Route("/recipe/comment/{recipeId:int}")]
    public async Task<IActionResult> CreateComment(int recipeId)
    {
        var comment = await Request.ReadFromJsonAsync<Comment>();
        if (comment is null || comment.UserName is null) return BadRequest();

        var recipe = await db.Recipes.FindAsync(recipeId);
        if (recipe is null) return BadRequest();

        await db.Comments.AddAsync(comment);
        recipe.Comments.Add(comment);

        await db.SaveChangesAsync();

        return Json(comment);
    }
}
