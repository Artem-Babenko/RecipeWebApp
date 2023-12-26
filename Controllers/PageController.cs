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
}
