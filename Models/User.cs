﻿
namespace RecipeWebApp.Models;

/// <summary>
/// Клас, що представляє користувача.
/// </summary>
public class User
{
    /// <summary>
    /// Унікальний ідентифікатор користувача.
    /// </summary>
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
    /// Логін користувача.
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Пароль користувача.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Список рецептів, створених користувачем.
    /// </summary>
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
}