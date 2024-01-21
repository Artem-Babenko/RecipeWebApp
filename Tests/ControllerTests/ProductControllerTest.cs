using Microsoft.AspNetCore.Mvc;
using RecipeWebApp.Controllers;
using RecipeWebApp.Models;
using RecipeWebApp.Tests.Common;
using Xunit;

namespace RecipeWebApp.Tests.ControllerTests;

/// <summary>
/// Тестувальний клас для <see cref="ProductController"/>.
/// </summary>
public class ProductControllerTest : TestControllerBase
{
    private readonly ProductController controller;
    public ProductControllerTest()
    {
        // Arrange
        controller = new ProductController(DatabaseContext, MemoryCache);
    }

    [Fact]
    public async Task GetProducts_ReturnsListOfProgucts()
    {
        // Act
        IActionResult result = await controller.GetProducts();

        // Assert
        var json = result as JsonResult;
        Assert.IsType<List<Product>>(json?.Value);
    }
}
