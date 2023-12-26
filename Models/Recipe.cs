
using System.ComponentModel.DataAnnotations;
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
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Назва рецепту.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Опис рецепту.
    /// </summary>
    public string? Description { get; set; } = "";

    /// <summary>
    /// Шлях до фотографії рецепту.
    /// </summary>
    public string? PhotoName { get; set; }

    /// <summary>
    /// Рейтинг страви від 0.0 до 5.
    /// </summary>
    public float Rating { get; set; } = 0.0f;

    /// <summary>
    /// Складність приготування завдання.
    /// </summary>
    public string? Difficulty { get; set; }

    /// <summary>
    /// Кількість переглядів рецепту.
    /// </summary>
    public int Views { get; set; } = 0;

    /// <summary>
    /// Час на приготування страви.
    /// </summary>
    public TimeSpan CookingTime { get; set; }

    /// <summary>
    /// Список інгредієнтів, необхідних для приготування рецепту.
    /// </summary>
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    /// <summary>
    /// Список кроків приготування страви.
    /// </summary>
    public List<CookingStep> CookingSteps { get; set; } = new List<CookingStep>();

    /// <summary>
    /// Список коментарів, які належать рецепту.
    /// </summary>
    public List<Comment> Comments { get; set; } = new List<Comment>();

    /// <summary>
    /// Дата створення рецепта.
    /// </summary>
    public DateTime CreateDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Користувач, якому належить цей рецепт.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Ідентифікатор користувача, якому належить цей рецепт.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Категорія, якій належить рецепт страви.
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// Ідентифікатор категорії, якій належить рецепт страви.
    /// </summary>
    public int CategoryId { get; set; }
}