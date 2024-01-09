using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;
using System.Security.Claims;

namespace RecipeWebApp.Controllers;

/// <summary>
/// Контролер, який відповідає за обробку запитів, пов'язаних із користувачами.
/// </summary>
public class UserController : Controller
{
    RecipeDbContext db;
    public UserController(RecipeDbContext db) => this.db = db;

    /// <summary>
    /// Обробляє HTTP POST-запит надсилання даних користувача для входу. 
    /// Створення аутентифікаційних кук для користувача.
    /// </summary>
    [HttpPost]
    [Route("/user/login")]
    public async Task<IActionResult> UserLogin()
    {
        LoginModel? loginModel = await Request.ReadFromJsonAsync<LoginModel>();

        if (loginModel is null || loginModel.Email is null || loginModel.Password is null) return BadRequest("Invalid payload");

        Console.WriteLine(loginModel.Email + loginModel.Password);
        User? user = await db.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);

        if (user is null) return Unauthorized();

        var claims = new List<Claim>()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name ?? ""),
            new Claim(ClaimTypes.Surname, user.Surname ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(principal);

        Response.Cookies.Append("UserName", user.Name ?? "");
        Response.Cookies.Append("UserSurname", user.Surname ?? "");

        return Ok();
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для видалення аутентифікаційних кук. 
    /// Перехід на головну сторінку.
    /// </summary>
    [HttpGet]
    [Authorize]
    [Route("/user/logout")]
    public async Task<IActionResult> UserLogout()
    {
        await HttpContext.SignOutAsync();
        Response.Cookies.Delete("UserName");
        Response.Cookies.Delete("UserSurname");
        return Redirect("/");
    }

    /// <summary>
    /// Обробляє HTTP POST-запит для надсилання реєстраційних даних користувача. 
    /// Додає користувача до бази даних та створює аутентифікаційні куки.
    /// </summary>
    [HttpPost]
    [Route("/user/registration")]
    public async Task<IActionResult> UserRegistration()
    {
        var newUser = await Request.ReadFromJsonAsync<User>();
        if (newUser is null) return BadRequest();

        await db.Users.AddAsync(newUser);
        await db.SaveChangesAsync();

        Console.WriteLine(newUser.Id);

        var claims = new List<Claim>()
        {
            new Claim("Id", newUser.Id.ToString()),
            new Claim(ClaimTypes.Name, newUser.Name ?? ""),
            new Claim(ClaimTypes.Surname, newUser.Surname ?? ""),
            new Claim(ClaimTypes.Email, newUser.Email ?? "")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(principal);

        Response.Cookies.Append("UserName", newUser.Name ?? "");
        Response.Cookies.Append("UserSurname", newUser.Surname ?? "");

        return Redirect("/");
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання поточного користувача з його усіма даними.
    /// </summary>
    [HttpGet]
    [Authorize]
    [Route("/user")]
    public async Task<IActionResult> GetUser()
    {
        int id = int.Parse(HttpContext.User.FindFirstValue("Id") ?? "");

        var user = await db.Users
            .Include(u => u.Recipes)
                .ThenInclude(r => r.Category)
            .Include(u => u.Recipes)
                .ThenInclude(r => r.Comments)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return Unauthorized();

        // Отримати список категорій та кількість рецептів для кожної категорії
        var categoriesWithRecipeCount = user.Recipes
            .GroupBy(r => r.Category)
            .Select(group => new
            {
                Category = group.Key,
                RecipeCount = group.Count()
            })
            .ToList();

        // Отримати кількість коментарів, середній рейтинг коментарів та кількість переглядів рецептів
        var totalComments = user.Recipes.Sum(r => r.Comments.Count);
        var averageRating = user.Recipes.SelectMany(r => r.Comments).Average(c => c.Rating);
        var totalViews = user.Recipes.Sum(r => r.Views);

        // Перетворити об'єкт користувача та список категорій у формат JSON
        return Json(new
        {
            User = user,
            Categories = categoriesWithRecipeCount,
            TotalComments = totalComments,
            Recipes = user.Recipes,
            AverageRating = averageRating,
            TotalViews = totalViews
        });
    }

    /// <summary>
    /// Обробляє HTTP PUT-запит для надсилання нових даних користувача.
    /// </summary>
    [HttpPut]
    [Route("/user")]
    [Authorize]
    public async Task<IActionResult> EditUser()
    {
        var userData = await Request.ReadFromJsonAsync<User>();
        if (userData is null || userData.Name is null) return BadRequest("Bad request data.");

        var user = await db.Users.FindAsync(userData.Id);
        if (user is null) return BadRequest("Uncorrect user id.");

        user.Name = userData.Name;
        user.Surname = userData.Surname;
        user.Age = userData.Age;
        user.Gender = userData.Gender;
        user.Email = userData.Email;
        user.Password = userData.Password;

        await db.SaveChangesAsync();

        return Ok();
    }
}

public record LoginModel(string Email, string Password);