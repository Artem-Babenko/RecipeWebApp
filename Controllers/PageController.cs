using Microsoft.AspNetCore.Mvc;

namespace RecipeWebApp.Controllers;

public class PageController : Controller
{
    /// <summary>
    /// Надсилає головну сторінку.
    /// </summary>
    [HttpGet]
    [Route("/")]
    public IActionResult GetMainPage()
    {
        return File("html/index.html", "text/html; charset=utf-8;");
    }

    /// <summary>
    /// Надсилає сторінку рецепту.
    /// </summary>
    [HttpGet]
    [Route("/recipe")]
    public IActionResult GetRecipePage()
    {
        return File("html/recipe.html", "text/html; charset=utf-8;");
    }

    /// <summary>
    /// Надсилає сторінку входу.
    /// </summary>
    [HttpGet]
    [Route("/login")]
    public IActionResult GetLoginPage()
    {
        return File("html/login.html", "text/html; charset=utf-8;");
    }

    /// <summary>
    /// Надсилає сторінку реєстрації.
    /// </summary>
    [HttpGet]
    [Route("/registration")]
    public IActionResult GetRegistrationPage()
    {
        return File("html/registration.html", "text/html; charset=utf-8;");
    }

    /// <summary>
    /// Надсилає сторінку профілю.
    /// </summary>
    [HttpGet]
    [Route("/profile")]
    public IActionResult GetProfilePage()
    {
        return File("html/profile.html", "text/html; charset=utf-8;");
    }
}
