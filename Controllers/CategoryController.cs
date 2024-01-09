using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;

namespace RecipeWebApp.Controllers;

/// <summary>
/// Контролер, який оброблює запити пов'язані з категоріями рецептів.
/// </summary>
public class CategoryController : Controller
{
    /// <summary>
    /// Контекст бази даних.
    /// </summary>
    RecipeDbContext db;
    public CategoryController(RecipeDbContext db) => this.db = db;

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання списоку усіх категорій.
    /// </summary>
    [HttpGet]
    [Route("/categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await db.Categories.ToListAsync();
        return Json(categories);
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання списоку "Головних" категорій.
    /// </summary>
    [HttpGet]
    [Route("/categories/main")]
    public async Task<IActionResult> GetMainCategories()
    {
        var mainCategories = await db.Categories.Where(c => c.ParentCategoryId == null).ToListAsync();
        return Json(mainCategories);
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання інформації, що є у категорії: дочірні категорії та рецепти з усіх них.
    /// </summary>
    /// <param name="id">Ідентифікатор категорії.</param>
    [HttpGet]
    [Route("/categories/{id:int}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        // Знаходження категорії та додавання об'єктів які їй належать.
        var category = await db.Categories
            .Include(c => c.Recipes)
            .Include(c => c.Subcategories)
                .ThenInclude(c => c.Recipes)
                .ThenInclude(r => r.Comments)
            .Include(c => c.Subcategories)
                .ThenInclude(c => c.Subcategories)
                .ThenInclude(c => c.Recipes)
                .ThenInclude(r => r.Comments)
            .Include(c => c.Subcategories)
                .ThenInclude(c => c.Subcategories)
                    .ThenInclude(c => c.Subcategories)
                    .ThenInclude(c => c.Recipes)
                    .ThenInclude(r => r.Comments)
            .Include(c => c.Subcategories)
                .ThenInclude(c => c.Subcategories)
                    .ThenInclude(c => c.Subcategories)
                        .ThenInclude(c => c.Subcategories)
                        .ThenInclude(c => c.Recipes)
                        .ThenInclude(r => r.Comments)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null) return NotFound();
        await db.Comments.LoadAsync();

        // Встановлення кількості рецептів у кожній дочірній категорії.
        SetRecipeCountInCategory(category);

        // Отримання рецептів які є у категорії та у дочірніх.
        var recipes = GetRecipesInCategory(category);

        return Json(new
        {
            Category = category,
            Recipes = recipes
        });
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання списоку категорій, які є найнижчими у дереві категорій.
    /// Саме вони призначенні для того, щоб містити рецепти.
    /// </summary>
    [HttpGet]
    [Route("/categories/last")]
    public async Task<IActionResult> GetLastCategories()
    {
        // Вибірка категоій які немають дочірніх.
        List<Category> categories = await db.Categories
            .Where(c => c.Subcategories.Count == 0)
            .ToListAsync();
        return Json(categories);
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

        // Додайте рецепти поточної категорії.
        recipes.AddRange(category.Recipes);

        // Рекурсивно додайте рецепти для дочірніх категорій.
        foreach (var subcategory in category.Subcategories)
        {
            recipes.AddRange(GetRecipesInCategory(subcategory));
        }

        return recipes;
    }
}
