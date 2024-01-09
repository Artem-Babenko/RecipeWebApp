
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeWebApp.Models;

/// <summary>
/// Клас, що представляє продукт.
/// </summary>
public class Product
{
    /// <summary>
    /// Унікальний ідентифікатор продукту.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Назва продукту.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Кількість продукту за замовчуванням.
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// Додаткова властивітсь - вага одиниці продукту. Для інгредієнтів які вимірюються у "шт".
    /// </summary>
    public double? Weight { get; set; }

    /// <summary>
    /// Одиниці вимірювання продукту.
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Кількість калорій на одну одиницю продукту.
    /// </summary>
    public float Calories { get; set; }

    /// <summary>
    /// Кількість білків на одну одиницю продукту.
    /// </summary>
    public float Proteins { get; set; }

    /// <summary>
    /// Кількість вуглеводів на одну одиницю продукту.
    /// </summary>
    public float Carbohydrate { get; set; }

    /// <summary>
    /// Кількість жирів на одну одиницю продукту.
    /// </summary>
    public float Fats { get; set; }

    /// <summary>
    /// Список інгредієнтів рецептів, які посилаються на цей продукт.
    /// </summary>
    [JsonIgnore]
    public List<Ingredient> Ingregients { get; set; } = new List<Ingredient>();
}