using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeWebApp.Models;

/// <summary>
/// Клас, що представляє категорію рецептів.
/// </summary>
public class Category
{
    /// <summary>
    /// Ідентифікатор категорії рецептів.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Назва категорії страв.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Кількість страв у категорії.
    /// </summary>
    public int RecipeCount { get; set; }

    /// <summary>
    /// Список дочірніх категорій.
    /// </summary>
    public List<Category> Subcategories { get; set; } = new List<Category>();

    /// <summary>
    /// Список рецептів, які належать цій категорії.
    /// </summary>
    [JsonIgnore]
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();

    /// <summary>
    /// Ідентифікатор батьківської категорії.
    /// </summary>
    public int? ParentCategoryId { get; set; }

    /// <summary>
    /// Батьківська категорія.
    /// </summary>
    [JsonIgnore]
    public Category? ParentCategory { get; set; }
}
