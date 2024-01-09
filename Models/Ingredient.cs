using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeWebApp.Models;

/// <summary>
/// Клас, що представляє інгредієнт та його кількість для приготування рецепту.
/// </summary>
public class Ingredient
{
    /// <summary>
    /// Ідентифікатор інгредієнта для рецепту.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Продукт, як основа для інгредієнту.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Ідентифікатор продукту.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Кількість інгредієнта для приготування.
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// Рецепт, якому належить інгредієнт.
    /// </summary>
    [JsonIgnore]
    public Recipe Recipe { get; set; } = null!;

    /// <summary>
    /// Ідентифікатор рецепта, якому належить інгредієнт.
    /// </summary>
    public int RecipeId { get; set; }
}
