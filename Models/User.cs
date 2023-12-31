﻿
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeWebApp.Models;

/// <summary>
/// Клас, що представляє користувача.
/// </summary>
public class User
{
    /// <summary>
    /// Унікальний ідентифікатор користувача.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Ім'я користувача.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Прізвище користувача.
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// Пошта користвача користувача.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Пароль користувача.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Вік користувача.
    /// </summary>
    public int? Age { get; set; }

    /// <summary>
    /// Стать користувача.
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// Список рецептів, створених користувачем.
    /// </summary>
    [JsonIgnore]
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
}