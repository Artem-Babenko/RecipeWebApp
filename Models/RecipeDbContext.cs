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
    /// Список продуктів у базі даних.
    /// </summary>
    public DbSet<Product> Products { get; set; } = null!;

    /// <summary>
    /// Список інгредієнтів у базі даних.
    /// </summary>
    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    /// <summary>
    /// Список кроків приготування рецепту у базі даних.
    /// </summary>
    public DbSet<CookingStep> CookingSteps { get; set; } = null!;

    /// <summary>
    /// Список коментарів рецептів у базі даних.
    /// </summary>
    public DbSet<Comment> Comments { get; set; } = null!;

    /// <summary>
    /// Список тимчасовий фото у базі даних.
    /// </summary>
    public DbSet<TemporaryPhoto> TemporaryPhotos { get; set; } = null!;


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

        // Встановлення зв'язку між багатьма Інгредієнтами (для рецептів) та одним Продуктом.
        modelBuilder.Entity<Ingredient>()
            .HasOne(rIngredient => rIngredient.Product)
            .WithMany(ingredient => ingredient.Ingregients)
            .HasForeignKey(rIngredient => rIngredient.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Встановлення зв'язку між багатька Рецептами та одною Категорією.
        modelBuilder.Entity<Recipe>()
            .HasOne(recipe => recipe.Category)
            .WithMany(category => category.Recipes)
            .HasForeignKey(recipe => recipe.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Встановлення зв'язку між багатьма Кроками та одним Рецептом.
        modelBuilder.Entity<CookingStep>()
            .HasOne(step => step.Recipe)
            .WithMany(recipe => recipe.CookingSteps)
            .HasForeignKey(step => step.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Встановлення зв'язку між багатьма Коментарями та одним Рецептом.
        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.Recipe)
            .WithMany(recipe => recipe.Comments)
            .HasForeignKey(comment => comment.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
           .HasMany(c => c.Subcategories)  // Категорія має багато підкатегорій
           .WithOne(c => c.ParentCategory)
           .HasForeignKey(c => c.ParentCategoryId);  // Вказуємо зовнішній ключ для ідентифікації батьківської категорії

        // Користувачі.
        List<User> users = new List<User>()
        {
            new User() {Id = 1, Name = "Артем", Surname = "Бабенко", Email = "babenkoartem505@gmail.com", Password = "12345678"}
        };

        // Категорії рецептів.
        List<Category> categories = new List<Category>()
        {
            // Головні категорії.
            new Category() { Id = 1, Name = "Перші страви" },
            new Category() { Id = 2, Name = "Другі страви" },
            new Category() { Id = 3, Name = "Закуски" },
            new Category() { Id = 4, Name = "Салати" },
            new Category() { Id = 5, Name = "Випічка" },
            new Category() { Id = 6, Name = "Десерти" },

            // Другорядні категорії.
            // Другорядні категорії для перших страв.
            new Category() { Id = 7, Name = "Гарячі супи", ParentCategoryId = 1 },
            new Category() { Id = 8, Name = "Холодні супи", ParentCategoryId = 1 },
            new Category() { Id = 9, Name = "Бульйони", ParentCategoryId = 1 },

            // Другорядні категорії для других страв.
            new Category() { Id = 10, Name = "Страви з м'яса", ParentCategoryId = 2 },
            new Category() { Id = 11, Name = "Страви з риби", ParentCategoryId = 2 },
            new Category() { Id = 111, Name = "Страви з морепродуктів", ParentCategoryId = 2 },
            new Category() { Id = 12, Name = "Страви з овочів, грибів", ParentCategoryId = 2 },
            new Category() { Id = 13, Name = "Страви з я'єць", ParentCategoryId = 2 },
            new Category() { Id = 14, Name = "Гарнір", ParentCategoryId = 2 },
            new Category() { Id = 15, Name = "Паста", ParentCategoryId = 2 },
            new Category() { Id = 16, Name = "Запіканки", ParentCategoryId = 2 },

            // Другорядні категорії для закусок.
            new Category() { Id = 17, Name = "Бутерброди", ParentCategoryId = 3 },
            new Category() { Id = 18, Name = "Рулети", ParentCategoryId = 3 },
            new Category() { Id = 19, Name = "Паштети", ParentCategoryId = 3 },
            new Category() { Id = 20, Name = "Снеки", ParentCategoryId = 3 },

            // Другорядні категорії для салатів.
            new Category() { Id = 21, Name = "Салати з м'яса", ParentCategoryId = 4 },
            new Category() { Id = 22, Name = "Салати з риби", ParentCategoryId = 4 },
            new Category() { Id = 222, Name = "Салати з морепродуктів", ParentCategoryId = 4 },
            new Category() { Id = 23, Name = "Салити з овочів, грибів", ParentCategoryId = 4 },

            // Трьохрядні категорії для Салати з м'яса.
            new Category() { Id = 324, Name = "Caлат з курятини", ParentCategoryId = 21 },
            new Category() { Id = 325, Name = "Caлат з cвинини", ParentCategoryId = 21 },
            new Category() { Id = 326, Name = "Caлат з яловичини", ParentCategoryId = 21 },

            // Другорядні категорії для випічки.
            new Category() { Id = 24, Name = "Млинці, оладки, сирники", ParentCategoryId = 5 },
            new Category() { Id = 25, Name = "Піца", ParentCategoryId = 5 },
            new Category() { Id = 26, Name = "Вироби з тіста", ParentCategoryId = 5 },

            // Другорядні категорії для десертів.
            new Category() { Id = 27, Name = "Торти", ParentCategoryId = 6 },
            new Category() { Id = 28, Name = "Печиво", ParentCategoryId = 6 },
            new Category() { Id = 29, Name = "Тістечка", ParentCategoryId = 6 },
            new Category() { Id = 30, Name = "Цукерки", ParentCategoryId = 6 },
            new Category() { Id = 31, Name = "Морозиво", ParentCategoryId = 6 },
            new Category() { Id = 32, Name = "Фруктові десерти", ParentCategoryId = 6 }
        };

        // Список продуктів.
        List<Product> products = new List<Product>()
        {
            new Product() { Id = 1,
                Name = "Філе минтая",
                Amount = 100,
                Unit = "г",
                Calories = 72,
                Proteins = 16,
                Carbohydrate = 0,
                Fats = 1
            },
            new Product() { Id = 2,
                Name = "Білий хліб",
                Amount = 100,
                Unit = "г",
                Calories = 257,
                Proteins = 7.7f,
                Carbohydrate = 52,
                Fats = 1
            },
            new Product() { Id = 3,
                Name = "Молоко 2.5%",
                Amount = 100,
                Unit = "мл",
                Calories = 51,
                Proteins = 3,
                Carbohydrate = 4.7f,
                Fats = 2.5f
            },
            new Product() { Id = 4,
                Name = "Яйце куряче",
                Amount = 1,
                Weight = 55,
                Unit = "шт",
                Calories = 83,
                Proteins = 7,
                Carbohydrate = 0.5f,
                Fats = 6
            },
            new Product() { Id = 5,
                Name = "Цибуля ріпчаста",
                Amount = 1,
                Weight = 70,
                Unit = "шт",
                Calories = 43,
                Proteins = 1.5f,
                Carbohydrate = 9,
                Fats = 0.25f
            },
            new Product() { Id = 6,
                Name = "Олія соняшникова",
                Amount = 1,
                Weight = 9.3f,
                Unit = "ст. л.",
                Calories = 90,
                Proteins = 0.01f,
                Carbohydrate = 0.01f,
                Fats = 9.95f
            },
            new Product() { Id = 7, 
                Name = "Сіль, чорний перець"
            },
            new Product() { Id = 8, 
                Name = "Сухарі панірувальні", 
                Amount = 10,
                Unit = "г",
                Calories = 24,
                Proteins = 1,
                Carbohydrate = 5,
                Fats = 0.2f
            },
        };

        // Cписок інгредієнтів.
        List<Ingredient> ingredients = new List<Ingredient>()
        {
            new Ingredient() { Id = 1, 
                Amount = 500, 
                ProductId = 1,
                RecipeId = 4
            },
            new Ingredient() { Id = 2,
                Amount = 100,
                ProductId = 2,
                RecipeId = 4
            },
            new Ingredient() { Id = 3,
                Amount = 100,
                ProductId = 3,
                RecipeId = 4
            },
            new Ingredient() { Id = 4,
                Amount = 1,
                ProductId = 4,
                RecipeId = 4
            },
            new Ingredient() { Id = 5,
                Amount = 1,
                ProductId = 5,
                RecipeId = 4
            },
            new Ingredient() { Id = 6,
                Amount = 2,
                ProductId = 6,
                RecipeId = 4
            },
            new Ingredient() { Id = 7,
                Amount = 50,
                ProductId = 8,
                RecipeId = 4
            },
            new Ingredient() { Id = 8,
                ProductId = 7,
                RecipeId = 4
            },
        };

        // Коментарі рецептів.
        List<Comment> comments = new List<Comment>()
        {
            new Comment() { Id = 1, UserName = "Олег", Title = "Все вийшло, рецепт дуже класний, раджу всім!", Rating = 5, RecipeId = 4 },
            new Comment() { Id = 2, UserName = "Катерина", Title = "Спочатку трішки не вийшло з чисткою риби, але потім все зробила так як треба, чоловіку дуже сподобалось:)", Rating = 4, RecipeId = 4 }
        };

        // Кроки для приготування рецепту.
        List<CookingStep> cookingSteps = new List<CookingStep>()
        {
            // Для рибних котлет.
            new CookingStep() { Id = 1, StepIndex = 1, Title = "Для приготування рибних котлет філе минтая промиваємо холодною водою і просушуємо. Потім ріжемо на середні шматочки.", RecipeId = 4 },
            new CookingStep() { Id = 2, StepIndex = 2, Title = "Очищаємо від лушпиння цибулю та ділимо її на дві частини.", RecipeId = 4 },
            new CookingStep() { Id = 3, StepIndex = 3, Title = "Половину цибулі нарізаємо маленькими шматочками та смажимо до прозорості на розігрітій соняшниковій олії. Вона додасть котлетам особливої солодкості та смаку.", RecipeId = 4 },
            new CookingStep() { Id = 4, StepIndex = 4, Title = "Минтай перекручуємо разом із другою половинкою цибулі через м'ясорубку із середньою решіткою.", RecipeId = 4 },
            new CookingStep() { Id = 5, StepIndex = 5, Title = "Замочуємо в молоці скибочки білого хліба. Потім їх добре віджимаємо, перекручуємо на м’ясорубці та кладемо в рибний фарш.", RecipeId = 4 },
            new CookingStep() { Id = 6, StepIndex = 6, Title = "Додаємо у фарш куряче яйце, подрібнений свіжий кріп і смажену цибулю. Посипаємо фарш для рибних котлет сіллю та чорним перцем і рукою добре замішуємо.", RecipeId = 4 },
            new CookingStep() { Id = 7, StepIndex = 7, Title = "Щоб котлети було легше ліпити, і вони зберегли форму під час смаження, поставимо фарш у холодильник на 20-30 хвилин.", RecipeId = 4 },
            new CookingStep() { Id = 8, StepIndex = 8, Title = "Руками, змоченими в холодній воді, або за допомогою ложки, робимо з фаршу красиві однакові котлетки.", RecipeId = 4 },
            new CookingStep() { Id = 9, StepIndex = 9, Title = "Насипаємо у мисочку панірувальні сухарі й кожну котлету обвалюємо в них.", RecipeId = 4 },
            new CookingStep() { Id = 10, StepIndex = 10, Title = "Смажимо рибні котлети у розігрітій соняшниковій олії до отримання рум'яної скоринки з обох боків.", RecipeId = 4 },
            new CookingStep() { Id = 11, StepIndex = 11, Title = "Додаємо до рибних котлет на гарнір картоплю або рис і подаємо. Смачного!", RecipeId = 4 },
        };

        // Рецепти.
        List<Recipe> recipes = new List<Recipe>()
        {
            new Recipe() { Id = 1,
                PhotoName = "pizza-cheese-tomatoes-olives.jpg",
                Name = "Оливкова піца",
                Description = "Оливка піца з сиром та томатним соусом - це прекрасна комбінація смаків та ароматів, яка задовольнить ваші гастрономічні бажання. Ця піца поєднує в собі солодкий смак томатного соусу, ароматний сир та інтенсивний смак оливок.\r\n\r\nПіца приготована на тонкому або товстому тісті, в залежності від вашого вибору. Тонке тісто надає піці легкість, тоді як товсте тісто робить її більш насиченою та ситною.\r\n\r\nНа тонкому або товстому коржі рівномірно розподілено томатний соус, який надає піці основний та смачний смак. Верхню частину покриває або ковбаса моцарелли, або комбінація сирів, таких як пармезан, чеддер або гауда.\r\n\r\nОсновна ідея полягає в тому, щоб нарізати оливки на тонкі кільця та рівномірно розподілити їх по всій піці. Це додає піці не тільки смак оливок, але й гарний зовнішній вигляд.\r\n\r\nПісля того, як піца випічена, на неї можна додати свіжі зелені, такі як базилік чи рукола, для додаткового аромату та свіжості.",
                Difficulty = "Нормально",
                Views = 76,
                CookingTime = TimeSpan.FromMinutes(40),
                UserId = 1,
                CategoryId = 25 },

            new Recipe() { Id = 2,
                PhotoName = "chef-salad.jpg",
                Name = "Шеф-салат",
                Description = "Шеф-салат - це вишукана та смачна страва, яка зазвичай подається в ресторанах як підкреслення кулінарної майстерності шеф-кухаря. Одним із класичних варіантів є \"Цезар\", який являє собою комбінацію свіжих листків салату, курячого філе, гартованого яйця, гарячого бекону, пармезану та ароматного соусу.\r\n\r\nОсновні складники шеф-салату можуть варіюватися залежно від рецепту та кухні, але зазвичай вони включають в себе свіжі овочі, м'ясо або рибу, сир, гарнір та соус.",
                Difficulty = "Легко",
                Views = 43,
                CookingTime = TimeSpan.FromMinutes(25),
                UserId = 1,
                CategoryId = 324 },

            new Recipe() { Id = 3,
                PhotoName = "chocolate-cake.jpg",
                Name = "Шоколадний тортик з полуницями",
                Description = "Шоколадний торт з полуницями - це чудовий десерт, який поєднує в собі солодкість шоколадного бісквіта з ароматом свіжих полуниць.",
                Difficulty = "Складно",
                Views = 58,
                CookingTime = TimeSpan.FromMinutes(80),
                UserId = 1,
                CategoryId = 27 },

            new Recipe() { Id = 4,
                PhotoName = "rybni-kotlety.jpg",
                Name = "Рибні котлети",
                Description = "Рибні котлети – це ароматна, неймовірно смачна та дієтична страва. Вони на тривалий час насичують організм, ідеально поєднується з різноманітними гарнірами та соусами. Ніжні рибні котлети стануть чудовою альтернативою звичайній смаженій рибі.",
                Difficulty = "Нормально",
                Views = 22,
                CookingTime = TimeSpan.FromMinutes(60),
                UserId = 1,
                CategoryId = 11 }
        };

        // Занесення даних у базу даних.
        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Product>().HasData(products);
        modelBuilder.Entity<Ingredient>().HasData(ingredients);
        modelBuilder.Entity<Comment>().HasData(comments);
        modelBuilder.Entity<CookingStep>().HasData(cookingSteps);
        modelBuilder.Entity<Recipe>().HasData(recipes);
    }
}
