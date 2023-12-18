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
    /// Назва категорії у множині.
    /// </summary>
    public string? PluralName { get; set; }

    /// <summary>
    /// Список рецептів, які належать цій категорії.
    /// </summary>
    [JsonIgnore]
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
}
