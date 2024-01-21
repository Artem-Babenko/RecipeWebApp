using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RecipeWebApp.Controllers;
using RecipeWebApp.Tests.Common;
using Xunit;

namespace RecipeWebApp.Tests.ControllerTests;

/// <summary>
/// Тестувальний клас для <see cref="PageController"/>.
/// </summary>
public class PageControllerTest : TestControllerBase
{
    private readonly PageController controller;
    public PageControllerTest()
    {
        // Arrange
        controller = new PageController();
    }

    [Fact]
    public void GetMainPage_ReturnsHtmlFile()
    {
        // Act
        IActionResult result = controller.GetMainPage();

        // Assert
        var fileResult = Assert.IsType<VirtualFileResult>(result);
        Assert.Equal("text/html; charset=utf-8", fileResult.ContentType);
    }

    [Fact]
    public void GetRecipePage_ReturnsHtmlFile()
    {
        // Act
        IActionResult result = controller.GetRecipePage();

        // Assert
        var fileResult = Assert.IsType<VirtualFileResult>(result);
        Assert.Equal("text/html; charset=utf-8", fileResult.ContentType);
    }

    [Fact]
    public void GetLoginPage_ReturnsHtmlFile()
    {
        // Act
        IActionResult result = controller.GetLoginPage();

        // Assert
        var fileResult = Assert.IsType<VirtualFileResult>(result);
        Assert.Equal("text/html; charset=utf-8", fileResult.ContentType);
    }

    [Fact]
    public void GetRegistrationPage_ReturnsHtmlFile()
    {
        // Act
        IActionResult result = controller.GetRegistrationPage();

        // Assert
        var fileResult = Assert.IsType<VirtualFileResult>(result);
        Assert.Equal("text/html; charset=utf-8", fileResult.ContentType);
    }

    [Fact]
    public void GetProfilePage_ReturnsHtmlFile()
    {
        // Act
        IActionResult result = controller.GetProfilePage();

        // Assert
        var fileResult = Assert.IsType<VirtualFileResult>(result);
        Assert.Equal("text/html; charset=utf-8", fileResult.ContentType);
    }

    [Fact]
    public void GetCreateRecipePage_ReturnsHtmlFile()
    {
        // Act
        IActionResult result = controller.GetCreateRecipePage();

        // Assert
        var fileResult = Assert.IsType<VirtualFileResult>(result);
        Assert.Equal("text/html; charset=utf-8", fileResult.ContentType);
    }

    [Fact]
    public void GetEditRecipePage_ReturnsHtmlFile()
    {
        // Act
        IActionResult result = controller.GetEditRecipePage();

        // Assert
        var fileResult = Assert.IsType<VirtualFileResult>(result);
        Assert.Equal("text/html; charset=utf-8", fileResult.ContentType);
    }
}
