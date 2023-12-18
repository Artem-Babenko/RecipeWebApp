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
    /// Cписок категорії рецептів у базі даних.
    /// </summary>
    public DbSet<Category> Categories { get; set; } = null!;

    /// <summary>
    /// Список інгредієнтів у базі даних.
    /// </summary>
    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    /// <summary>
    /// Конструктор для створення контексту бази даних <see cref="RecipeDbContext"/>.
    /// </summary>
    /// <param name="options"></param>
    public RecipeDbContext(DbContextOptions options) : base(options)
    {
        /*Database.EnsureDeleted();
        Database.EnsureCreated();*/
    }

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

        // Встановлення зв'язку між багатька Рецептами та одною Категорією.
        modelBuilder.Entity<Recipe>()
            .HasOne(recipe => recipe.Category)
            .WithMany(category => category.Recipes)
            .HasForeignKey(recipe => recipe.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        List<User> users = new List<User>()
        {
            new User() {Id = 1, Name = "Артем", Login = "artem123", Password = "12345678"}
        };

        List<Category> categories = new List<Category>()
        {
            new Category() {Id = 1, Name = "Піца", PluralName = "Піци"}, 
            new Category() {Id = 2, Name = "Cалат", PluralName = "Салати"}, 
            new Category() {Id = 3, Name = "Десерт", PluralName = "Десерти"}, 
            new Category() {Id = 4, Name = "Випічка", PluralName = "Випічка"}, 
            new Category() {Id = 5, Name = "Суп", PluralName = "Супи"}, 
            new Category() {Id = 6, Name = "Каша", PluralName = "Каші"},
        };

        List<Recipe> recipes = new List<Recipe>()
        {
            new Recipe() { Id = 1,
                PhotoPath = "pizza-cheese-tomatoes-olives.jpg",
                Name = "Оливкова піца",
                Description = "Оливка піца з сиром та томатним соусом - це прекрасна комбінація смаків та ароматів, яка задовольнить ваші гастрономічні бажання. Ця піца поєднує в собі солодкий смак томатного соусу, ароматний сир та інтенсивний смак оливок.\r\n\r\nПіца приготована на тонкому або товстому тісті, в залежності від вашого вибору. Тонке тісто надає піці легкість, тоді як товсте тісто робить її більш насиченою та ситною.\r\n\r\nНа тонкому або товстому коржі рівномірно розподілено томатний соус, який надає піці основний та смачний смак. Верхню частину покриває або ковбаса моцарелли, або комбінація сирів, таких як пармезан, чеддер або гауда.\r\n\r\nОсновна ідея полягає в тому, щоб нарізати оливки на тонкі кільця та рівномірно розподілити їх по всій піці. Це додає піці не тільки смак оливок, але й гарний зовнішній вигляд.\r\n\r\nПісля того, як піца випічена, на неї можна додати свіжі зелені, такі як базилік чи рукола, для додаткового аромату та свіжості.",
                Rating = 4.7f,
                Views = 76,
                CookingTime = TimeSpan.FromMinutes(40),
                UserId = 1,
                CategoryId = 1 },

            new Recipe() { Id = 2,
                PhotoPath = "chef-salad.jpg",
                Name = "Шеф-салат",
                Description = "Шеф-салат - це вишукана та смачна страва, яка зазвичай подається в ресторанах як підкреслення кулінарної майстерності шеф-кухаря. Одним із класичних варіантів є \"Цезар\", який являє собою комбінацію свіжих листків салату, курячого філе, гартованого яйця, гарячого бекону, пармезану та ароматного соусу.\r\n\r\nОсновні складники шеф-салату можуть варіюватися залежно від рецепту та кухні, але зазвичай вони включають в себе свіжі овочі, м'ясо або рибу, сир, гарнір та соус.",
                Rating = 3.8f,
                Views = 43,
                CookingTime = TimeSpan.FromMinutes(25),
                UserId = 1,
                CategoryId = 2 },

            new Recipe() { Id = 3,
                PhotoPath = "chocolate-cake.jpg",
                Name = "Шоколадний тортик з полуницями",
                Description = "Шоколадний торт з полуницями - це чудовий десерт, який поєднує в собі солодкість шоколадного бісквіта з ароматом свіжих полуниць.",
                Rating = 4.3f,
                Views = 58,
                CookingTime = TimeSpan.FromMinutes(80),
                UserId = 1,
                CategoryId = 3 }
        };

        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Recipe>().HasData(recipes);
    }
}
