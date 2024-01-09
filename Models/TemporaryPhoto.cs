using System.ComponentModel.DataAnnotations;

namespace RecipeWebApp.Models;

/// <summary>
/// Представляє тимчасову фотографію
/// </summary>
public class TemporaryPhoto
{
    /// <summary>
    /// Ідентифікатор тимчасового фото.
    /// </summary>
    [Key] public int Id { get; set; }

    /// <summary>
    /// Шлях до тимчасової фотографії.
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// Дата створення тимчасового фото.
    /// </summary>
    public DateTime CreationTime { get; set; } = DateTime.Now;
}
