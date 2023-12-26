using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeWebApp.Models;

/// <summary>
/// Представляє користувацький коментар рецепту.
/// </summary>
public class Comment
{
    /// <summary>
    /// Ідентифікатор коментаря.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Ім'я користувача, який створив коментар.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Текст коментаря.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Дата створення коментаря.
    /// </summary>
    public DateTime CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Кількість лайків коментаря.
    /// </summary>
    public int CountOfLikes { get; set; } = 0;

    /// <summary>
    /// Оцінка користувача у цьому коментарі.
    /// </summary>
    public float Rating { get; set; }

    /// <summary>
    /// Рецепт, якому належить коментар.
    /// </summary>
    [JsonIgnore]
    public Recipe? Recipe { get; set; }

    /// <summary>
    /// Ідентифікатор рецепта, якому належить коментар.
    /// </summary>
    public int RecipeId { get; set; }
}
