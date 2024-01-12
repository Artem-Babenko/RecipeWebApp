using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RecipeWebApp.Models;
namespace RecipeWebApp.Controllers;

/// <summary>
/// Контролер, який відповідає за обробку запитів, пов'язаних із продуктами.
/// </summary>
public class ProductController : Controller
{
    private readonly IMemoryCache memoryCache;
    private readonly RecipeDbContext db;
    public ProductController(RecipeDbContext db, IMemoryCache memoryCache)
    {
        this.db = db;
        this.memoryCache = memoryCache;
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання списку всіх продуктів.
    /// </summary>
    [HttpGet]
    [Route("/products")]
    public async Task<IActionResult> GetProducts()
    {
        // Спроба дістати продукти з кешу.
        if(memoryCache.TryGetValue("products", out List<Product>? products))
        {
            return Json(products);
        }

        // Якщо в кеші немає то
        // асинхронне отримання всіх продуктів з бази даних.
        products = await db.Products.ToListAsync();
        // Збереження продуктів у кеш. 
        memoryCache.Set("products", products, TimeSpan.FromHours(1));
        // Повертає JSON-представлення списку продуктів.
        return Json(products);
    }
}
