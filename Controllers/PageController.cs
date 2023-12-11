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
}
