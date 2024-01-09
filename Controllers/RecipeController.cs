using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace RecipeWebApp.Controllers;

/// <summary>
/// Контролер, який відповідає за обробку запитів, пов'язаних із рецептами.
/// </summary>
public class RecipeController : Controller
{
    RecipeDbContext db;
    public RecipeController(RecipeDbContext db) => this.db = db;

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання рецептів.
    /// </summary>
    [HttpGet]
    [Route("/recipes")]
    public async Task<IActionResult> GetRecipes()
    {
        var recipes = await db.Recipes
            .Include(recipe => recipe.Category)
            .Include(recipe => recipe.Comments)
            .ToListAsync();
        // Надсилання відповіді у вигляді JSON-списку.
        return Json(recipes);
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання рецепта за ідентифікатором.
    /// </summary>
    [HttpGet]
    [Route("/recipes/{id:int}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        // Знаходження рецепта та додавання об'єктів які пов'язані з ним.
        var recipe = await db.Recipes
            .Include(recipe => recipe.Category)
            .Include(recipe => recipe.Ingredients)
            .ThenInclude(rIngredient => rIngredient.Product)
            .Include(recipe => recipe.User)
            .Include(recipe => recipe.CookingSteps)
            .Include(recipe => recipe.Comments)
            .FirstOrDefaultAsync(recipe => recipe.Id == id);

        if (recipe is null) return NotFound();
        // Надсилання відповіді у вигляді JSON-об'єкта.        
        return Json(recipe);
    }


    /// <summary>
    /// Обробляє HTTP POST-запит для надсилання та створення коментаря.
    /// </summary>
    [HttpPost]
    [Route("/recipe/comments/{recipeId:int}")]
    public async Task<IActionResult> CreateComment(int recipeId)
    {
        // Отримання з запиту об'єкт "коментар".
        var comment = await Request.ReadFromJsonAsync<Comment>();
        if (comment is null || comment.UserName is null) return BadRequest();

        // Знаходження рецепта, якому буде доданий коментар. 
        var recipe = await db.Recipes.FindAsync(recipeId);
        if (recipe is null) return BadRequest();

        // Додавання коментаря до бази даних та у рецепт.
        await db.Comments.AddAsync(comment);
        recipe.Comments.Add(comment);

        // Збереження база даних.
        await db.SaveChangesAsync();
        // Надсилання збереженого коментаря у відповідь.
        return Json(comment);
    }

    /// <summary>
    /// Обробляє HTTP POST-запит для завантаження фото страви.
    /// </summary>
    [HttpPost]
    [Authorize]
    [Route("/recipes/photo")]
    public async Task<IActionResult> UploadPhoto()
    {
        // Завантажуємо фотографії.
        var file = Request.Form.Files[0];
        var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/photos";
        Directory.CreateDirectory(uploadPath);

        // Назва завантаженого фото, але вже з Guid-ідентифікатором.
        string fileName = Guid.NewGuid().ToString() + file.FileName;
        string fullPath = $"{uploadPath}/{fileName}";

        // Створення FileStream для збереження завантаженого файлу.
        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            // Асинхронне копіювання вмісту завантаженого файлу до FileStream.
            await file.CopyToAsync(fileStream);
        }

        // Код для зменшення розміру фото.
        using (var image = Image.Load(fullPath))
        {
            int targetWidth = 1600;
            int targetHeight = 900;

            if (image.Width > targetWidth || image.Height > targetHeight)
            {
                double scale = Math.Min((double)targetWidth / image.Width, (double)targetHeight / image.Height);
                int newWidth = (int)(image.Width * scale);
                int newHeight = (int)(image.Height * scale);

                image.Mutate(operation => operation.Resize(new ResizeOptions
                {
                    Size = new Size(newWidth, newHeight),
                    Mode = ResizeMode.Max,
                }));
            }

            image.Save(fullPath, new JpegEncoder());
        }

        // Додавання тимчасового фото.
        var newTempPhoto = new TemporaryPhoto() { Path = fileName };
        db.TemporaryPhotos.Add(newTempPhoto);

        // Збереження змін у базі даних.
        await db.SaveChangesAsync();
        // Надсилання нової назви доданого фото.
        return Json(new { fileName = fileName });
    }
}
