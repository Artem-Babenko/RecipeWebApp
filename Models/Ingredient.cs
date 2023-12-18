
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeWebApp.Models;

/// <summary>
/// Клас, що представляє інгредієнт.
/// </summary>
public class Ingredient
{
    /// <summary>
    /// Унікальний ідентифікатор інгредієнта.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Назва інгредієнта.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Рецепт, якому належить цей інгредієнт.
    /// </summary>
    [JsonIgnore]
    public Recipe? Recipe { get; set; }

    /// <summary>
    /// Ідентифікатор рецепту, якому належить цей інгредієнт.
    /// </summary>
    [JsonIgnore]
    public int RecipeId { get; set; }
}