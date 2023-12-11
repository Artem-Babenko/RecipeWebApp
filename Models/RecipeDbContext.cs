using Microsoft.EntityFrameworkCore;

namespace RecipeWebApp.Models;

/// <summary>
/// Представляє контекст бази даних.
/// </summary>
public class RecipeDbContext : DbContext
{
    /// <summary>
    /// Список користувачів у базі даних.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Список рецептів у базі даних.
    /// </summary>
    public DbSet<Recipe> Recipes { get; set; } = null!;

    /// <summary>
    /// Список інгредієнтів у базі даних.
    /// </summary>
    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    /// <summary>
    /// Конструктор для створення контексту бази даних <see cref="RecipeDbContext"/>.
    /// </summary>
    /// <param name="options"></param>
    public RecipeDbContext(DbContextOptions options) : base(options)
    { }

    /// <summary>
    /// Метод, який виконається при створенні бази даних.
    /// Використовується для налаштування моделі даних, визначення зв'язків
    /// між сутностями та встановлення інших правил бази даних.
    /// </summary>
    /// <param name="modelBuilder">Об'єкт, який надає можливість конфігурувати модель даних.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Встановлення зв'язку між багатьма Рецептами та одним Користувачем.
        modelBuilder.Entity<Recipe>()
            .HasOne(recipe => recipe.User)
            .WithMany(user => user.Recipes)
            .HasForeignKey(recipe => recipe.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Встановлення зв'язку між багатьма Інгредієнтами та одним Рецептом.
        modelBuilder.Entity<Ingredient>()
            .HasOne(ingredient => ingredient.Recipe)
            .WithMany(recipe => recipe.Ingredients)
            .HasForeignKey(ingredient => ingredient.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        List<User> users = new List<User>()
        {
            new User() {Id = 1, Name = "Артем", Login = "artem123", Password = "12345678"}
        };

        List<Recipe> recipes = new List<Recipe>()
        {
            new Recipe() { Id = 1, Name = "Тестовий рецепт", UserId = 1}
        };

        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Recipe>().HasData(recipes);
    }
}
