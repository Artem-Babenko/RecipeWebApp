using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Security.Claims;

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

        // Додавання перегляду.
        recipe.Views++;
        await db.SaveChangesAsync();

        // Надсилання відповіді у вигляді JSON-об'єкта.        
        return Json(recipe);
    }

    /// <summary>
    /// Обробляє HTTP DELETE-запит для видалення рецепта за ідентифікатором.
    /// </summary>
    [HttpDelete]
    [Authorize]
    [Route("/recipes/{id:int}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var recipe = await db.Recipes.FindAsync(id);
        if (recipe is null) return NotFound();

        // Видалення рецепту з бази даних.
        db.Recipes.Remove(recipe);
        // Встановлення фото тимчасовим для того, щоб воно видалилось службою.
        await db.TemporaryPhotos.AddAsync(new TemporaryPhoto() { Path = recipe.PhotoName ?? "" });
        await db.SaveChangesAsync();

        return Ok();
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

    /// <summary>
    /// Обробляє HTTP POST-запит для надсилання моделі рецепта для його створення.
    /// </summary>
    [HttpPost]
    [Authorize]
    [Route("/recipes")]
    public async Task<IActionResult> CreateRecipe()
    {
        var recipeModel = await Request.ReadFromJsonAsync<RecipeModel>();
        if (recipeModel is null) return BadRequest();

        // Створення списку інгредієнтів.
        List<Ingredient> ingredients = new List<Ingredient>();
        foreach (IngredientModel ingredientModel in recipeModel.ingredients)
        {
            var ingredient = new Ingredient();
            ingredient.Amount = ingredientModel.amount;
            ingredient.ProductId = ingredientModel.id;
            ingredients.Add(ingredient);
        }

        // Створення списку кроків приготування.
        List<CookingStep> cookingSteps = recipeModel.cookingSteps.ToList();

        // Знаходження категорії.
        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.Name == recipeModel.category);

        // Користувач.
        int id = int.Parse(HttpContext.User.FindFirstValue("Id") ?? "");
        var user = await db.Users.FindAsync(id);

        // Створення рецепта.
        Recipe newRecipe = new Recipe()
        {
            Name = recipeModel.name,
            PhotoName = recipeModel.photoName,
            Description = recipeModel.description,
            Difficulty = recipeModel.difficulty,
            CookingTime = TimeSpan.FromMinutes(recipeModel.cookingTime),
            Ingredients = ingredients,
            CookingSteps = cookingSteps,
            Category = category,
            User = user
        };

        // Додавання нового рецепта до бази даних.
        await db.Recipes.AddAsync(newRecipe);
        // Видалення фото рецепта з тимчасових фото.
        var tempPhoto = await db.TemporaryPhotos.FirstOrDefaultAsync(t => t.Path == newRecipe.PhotoName);
        db.TemporaryPhotos.Remove(tempPhoto!);
        await db.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Обробляє HTTP PUT-запит для надсилання моделі рецепта та оновлення його даних.
    /// </summary>
    [HttpPut]
    [Authorize]
    [Route("/recipes")]
    public async Task<IActionResult> UpdateRecipe()
    {
        var recipeModel = await Request.ReadFromJsonAsync<RecipeModel>();
        if (recipeModel is null) return BadRequest("Invalid requst model!");

        var recipe = await db.Recipes
            .Include(r => r.CookingSteps)
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipeModel.id);
        if (recipe is null) return BadRequest("Invalid recipe id!");

        // Оновлення даних.
        recipe.Name = recipeModel.name;
        recipe.Description = recipeModel.description;
        recipe.Difficulty = recipeModel.difficulty;
        recipe.CookingTime = TimeSpan.FromMinutes(recipeModel.cookingTime);
        
        // Оновлення фото.
        if(recipe.PhotoName != recipeModel.photoName)
        {
            // Старе фото додається у тимчасові для побальшого видалення службою.
            await db.TemporaryPhotos.AddAsync(new TemporaryPhoto() { Path = recipe.PhotoName! });
            recipe.PhotoName = recipeModel.photoName;
        }

        // Оновлення категорії.
        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.Name == recipeModel.category);
        recipe.Category = category;

        // Видалення старих інгредієнті.
        var oldIngredients = recipe.Ingredients.ToList();
        db.Ingredients.RemoveRange(oldIngredients);
        // Створення списку інгредієнтів.
        List<Ingredient> ingredients = new List<Ingredient>();
        foreach (IngredientModel ingredientModel in recipeModel.ingredients)
        {
            var ingredient = new Ingredient();
            ingredient.Amount = ingredientModel.amount;
            ingredient.ProductId = ingredientModel.id;
            ingredients.Add(ingredient);
        }
        // Встановлення нових інгедієнтів.
        recipe.Ingredients = ingredients;

        // Видалення старих кроків.
        var oldCookingSteps = recipe.CookingSteps.ToList();
        db.CookingSteps.RemoveRange(oldCookingSteps);
        // Створення нового списку кроків приготування.
        List<CookingStep> cookingSteps = recipeModel.cookingSteps.ToList();
        // Встановлення нових кроків приготування.
        recipe.CookingSteps = cookingSteps;

        await db.SaveChangesAsync();
        return Ok();
    }

}

record RecipeModel(int id, string category, string name,
    string description, string photoName,
    int cookingTime, string difficulty,
    IngredientModel[] ingredients,
    CookingStep[] cookingSteps);

record IngredientModel(int id, float amount);