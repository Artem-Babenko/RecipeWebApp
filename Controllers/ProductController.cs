using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;
namespace RecipeWebApp.Controllers;

/// <summary>
/// Контролер, який відповідає за обробку запитів, пов'язаних із продуктами.
/// </summary>
public class ProductController : Controller
{
    private readonly RecipeDbContext db;
    public ProductController(RecipeDbContext db) => this.db = db;

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання списку всіх продуктів.
    /// </summary>
    [HttpGet]
    [Route("/products")]
    public async Task<IActionResult> GetProducts()
    {
        // Асинхронно отримує всі продукти з бази даних.
        var ingredients = await db.Products.ToListAsync();
        // Повертає JSON-представлення списку продуктів.
        return Json(ingredients);
    }
}
