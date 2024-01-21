
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RecipeWebApp.Controllers;
using RecipeWebApp.Models;
using RecipeWebApp.Tests.Common;
using Xunit;

namespace RecipeWebApp.Tests.ControllerTests;

/// <summary>
/// Контролер для тестування <see cref="CategoryController"/>.
/// </summary>
public class CategoryControllerTest : TestControllerBase
{
    private readonly CategoryController controller;
    public CategoryControllerTest()
    {
        // Arrange
        controller = new CategoryController(DatabaseContext, MemoryCache);
    }

    [Fact]
    public async Task GetCategories_ReturnsListOfCategories()
    {
        // Act
        IActionResult result = await controller.GetCategories();
        // Assert
        JsonResult? jsonResult = result as JsonResult;
        Assert.IsType<List<Category>>(jsonResult?.Value);
    }

    [Fact]
    public async Task GetCategory_ReturnsCategoryWithRecipes()
    {
        // Arrange
        Category newCategory = new Category() { Name = "Test name" };
        await DatabaseContext.Categories.AddAsync(newCategory);
        await DatabaseContext.SaveChangesAsync();

        // Act
        IActionResult result = await controller.GetCategory(newCategory.Id);

        // Assert
        JsonResult? jsonResult = result as JsonResult;
        Assert.NotNull(jsonResult?.Value);

        var response = jsonResult.Value as dynamic;
        Assert.NotNull(response);

        var category = response.Category as Category;
        Assert.NotNull(category);
        Assert.Equal(newCategory.Id, category.Id);
        Assert.Equal(newCategory.Name, category.Name);

        var recipes = response.Recipes as List<Recipe>;
        Assert.NotNull(recipes);
    }

    [Fact]
    public async Task GetMainCategories_ReturnsListOfMainCategories()
    {
        // Act
        IActionResult result = await controller.GetMainCategories();

        // Assert
        JsonResult? jsonResult = result as JsonResult;
        Assert.NotNull(jsonResult?.Value);

        var mainCategories = jsonResult.Value as List<Category>;
        Assert.NotNull(mainCategories);
    }

    [Fact]
    public async Task GetLastCategories_ReturnsListOfLastCategories()
    {
        // Act
        IActionResult result = await controller.GetLastCategories();

        // Assert
        JsonResult? jsonResult = result as JsonResult;
        Assert.NotNull(jsonResult?.Value);

        var lastCategories = jsonResult.Value as List<Category>;
        Assert.NotNull(lastCategories);
    }

    [Fact]
    public async Task GetLastCategories_ReturnsCachedCategories()
    {
        // Arrange
        List<Category> cachedCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Category1" },
            new Category { Id = 2, Name = "Category2" },
        };
        MemoryCache.Set("lastCategories", cachedCategories, TimeSpan.FromHours(1));

        // Act
        IActionResult result = await controller.GetLastCategories();

        // Assert
        JsonResult? jsonResult = result as JsonResult;
        Assert.NotNull(jsonResult?.Value);

        var lastCategories = jsonResult.Value as List<Category>;
        Assert.NotNull(lastCategories);

        Assert.Equal(cachedCategories, lastCategories);
    }
}
