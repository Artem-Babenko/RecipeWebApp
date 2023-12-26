using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeWebApp.Models;

/// <summary>
/// Представляє крок приготування рецепту.
/// </summary>
public class CookingStep
{
    /// <summary>
    /// Ідентифікатор кроку приготування рецепту.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Номер кроку. (для послідовності)
    /// </summary>
    public int StepIndex { get; set; }

    /// <summary>
    /// Опис кроку приготування рецепту.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Рецепт, якому належить крок приготування.
    /// </summary>
    [JsonIgnore]
    public Recipe? Recipe { get; set; }

    /// <summary>
    /// Ідентифікатор рецепта, якому належить крок приготування.
    /// </summary>
    public int RecipeId { get; set; }
}
