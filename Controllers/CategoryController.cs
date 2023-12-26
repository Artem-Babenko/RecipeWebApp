using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;

namespace RecipeWebApp.Controllers;

/// <summary>
/// Контролер, який оброблює запити пов'язані з Категоріями рецептів.
/// </summary>
public class CategoryController : Controller
{
    /// <summary>
    /// Контекст бази даних.
    /// </summary>
    RecipeDbContext db;
    public CategoryController(RecipeDbContext db) => this.db = db;

    /// <summary>
    /// Надсилає список усіх категорій.
    /// </summary>
    [HttpGet]
    [Route("/categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await db.Categories.ToListAsync();

        return Json(categories);
    }


    /// <summary>
    /// Надсилає список головних категорій.
    /// </summary>
    [HttpGet]
    [Route("/categories/main")]
    public async Task<IActionResult> GetMainCategories()
    {
        var mainCategories = await db.Categories.Where(c => c.ParentCategoryId == null).ToListAsync();

        return Json(mainCategories);
    }


    /// <summary>
    /// Надсилає інформацію про категорію та все що у ній є, за допомогою ідентифікатора.
    /// </summary>
    /// <param name="id">Ідентифікатор категорії.</param>
    [HttpGet]
    [Route("/categories/{id:int}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await db.Categories
            .Include(c => c.Recipes)
            .Include(c => c.Subcategories)
                .ThenInclude(c => c.Recipes)
            .Include(c => c.Subcategories)
                .ThenInclude(c => c.Subcategories)
                .ThenInclude(c => c.Recipes)
            .Include(c => c.Subcategories)
                .ThenInclude(c => c.Subcategories)
                    .ThenInclude(c => c.Subcategories)
                    .ThenInclude(c => c.Recipes)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null) return NotFound();

        // Встановлення кількості рецептів у кожній дочірній категорії.
        SetRecipeCountInCategory(category);

        var recipes = GetRecipesInCategory(category);

        return Json(new
        {
            Category = category,
            Recipes = recipes
        });
    }

    /// <summary>
    /// Рекурсивний метод встановлення кількості рецептів у кожній з дочірніх категорій.
    /// </summary>
    /// <param name="category">Головна категоріяю.</param>
    private void SetRecipeCountInCategory(Category category)
    {
        // Встановлення властивості кількості рецептів.
        category.RecipeCount = GetRecipesInCategory(category).Count;

        if (category.Subcategories != null)
        {
            foreach (var subcategory in category.Subcategories)
            {
                SetRecipeCountInCategory(subcategory);
            }
        }
    }

    /// <summary>
    /// Метод отримання всіх рецептів, які належать категорії, включаючі дочірні категорії.
    /// </summary>
    /// <param name="category">Головна категорія</param>
    /// <returns>Список рецептів.</returns>
    private List<Recipe> GetRecipesInCategory(Category category)
    {
        var recipes = new List<Recipe>();

        // Додайте рецепти поточної категорії
        recipes.AddRange(category.Recipes);

        // Рекурсивно додайте рецепти для дочірніх категорій
        foreach (var subcategory in category.Subcategories)
        {
            recipes.AddRange(GetRecipesInCategory(subcategory));
        }

        return recipes;
    }
}
