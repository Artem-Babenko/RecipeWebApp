
using System.Text.Json.Serialization;

namespace RecipeWebApp.Models;

/// <summary>
/// Клас, що представляє рецепт.
/// </summary>
public class Recipe
{
    /// <summary>
    /// Унікальний ідентифікатор рецепту.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Назва рецепту.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Опис рецепту.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Шлях до фотографії рецепту.
    /// </summary>
    public string? PhotoPath { get; set; }

    /// <summary>
    /// Список інгредієнтів, необхідних для приготування рецепту.
    /// </summary>
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    /// <summary>
    /// Дата створення рецепта.
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Користувач, якому належить цей рецепт.
    /// </summary>
    [JsonIgnore]
    public User? User { get; set; }

    /// <summary>
    /// Ідентифікатор користувача, якому належить цей рецепт.
    /// </summary>
    [JsonIgnore]
    public int UserId { get; set; }
}