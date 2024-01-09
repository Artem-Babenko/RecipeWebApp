using Microsoft.AspNetCore.Mvc;

namespace RecipeWebApp.Controllers;

/// <summary>
/// Контролер, який відповідає за надсилання сторінок.
/// </summary>
public class PageController : Controller
{
    /// <summary>
    /// Обробляє HTTP GET-запит для отримання головної сторінки.
    /// </summary>
    [HttpGet]
    [Route("/")]
    public IActionResult GetMainPage()
    {
        return File("html/index.html", "text/html; charset=utf-8;");
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання рецепта.
    /// </summary>
    [HttpGet]
    [Route("/recipe")]
    public IActionResult GetRecipePage()
    {
        return File("html/recipe.html", "text/html; charset=utf-8;");
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання сторінки входу.
    /// </summary>
    [HttpGet]
    [Route("/login")]
    public IActionResult GetLoginPage()
    {
        return File("html/login.html", "text/html; charset=utf-8;");
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання сторінки реєстрації.
    /// </summary>
    [HttpGet]
    [Route("/registration")]
    public IActionResult GetRegistrationPage()
    {
        return File("html/registration.html", "text/html; charset=utf-8;");
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання сторінки профілю користувача.
    /// </summary>
    [HttpGet]
    [Route("/profile")]
    public IActionResult GetProfilePage()
    {
        return File("html/profile.html", "text/html; charset=utf-8;");
    }

    /// <summary>
    /// Обробляє HTTP GET-запит для отримання сторінки створеня рецепту.
    /// </summary>
    [HttpGet]
    [Route("/create")]
    public IActionResult GetCreateRecipePage()
    {
        return File("html/create.html", "text/html; charset=utf-8;");
    }
}
