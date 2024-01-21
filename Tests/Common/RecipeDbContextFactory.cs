using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;

namespace RecipeWebApp.Tests.Common;

/// <summary>
/// Фабрика для створення та управління екземплярами бази даних <see cref="RecipeDbContext"/> в режимі іншої бази даних у пам'яті.
/// </summary>
public abstract class RecipeDbContextFactory
{
    /// <summary>
    /// Створює новий екземпляр бази даних <see cref="RecipeDbContext"/> у режимі іншої бази даних у пам'яті.
    /// </summary>
    /// <returns>Екземпляр бази даних <see cref="RecipeDbContext"/>.</returns>
    public static RecipeDbContext Create()
    {
        var dbContextOption = new DbContextOptionsBuilder<RecipeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var db = new RecipeDbContext(dbContextOption);
        db.Database.EnsureCreated();

        // Користувачі.
        List<User> users = new List<User>()
        {
            new User() {Name = "Артем", Surname = "Бабенко", Email = "babenkoartem505@gmail.com", Password = "12345678"}
        };

        // Категорії рецептів.
        List<Category> categories = new List<Category>()
        {
            // Головні категорії.
            new Category() { Name = "Перші страви" },
            new Category() { Name = "Другі страви" },
            new Category() { Name = "Закуски" },
            new Category() { Name = "Салати" },
            new Category() { Name = "Випічка" },
            new Category() { Name = "Десерти" },
        };

        // Список продуктів.
        List<Product> products = new List<Product>()
        {
            new Product() {
                Name = "Філе минтая",
                Amount = 100,
                Unit = "г",
                Calories = 72,
                Proteins = 16,
                Carbohydrate = 0,
                Fats = 1
            },
            new Product() {
                Name = "Білий хліб",
                Amount = 100,
                Unit = "г",
                Calories = 257,
                Proteins = 7.7f,
                Carbohydrate = 52,
                Fats = 1
            },
            new Product() {
                Name = "Молоко 2.5%",
                Amount = 100,
                Unit = "мл",
                Calories = 51,
                Proteins = 3,
                Carbohydrate = 4.7f,
                Fats = 2.5f
            },
            new Product() {
                Name = "Яйце куряче",
                Amount = 1,
                Weight = 55,
                Unit = "шт",
                Calories = 83,
                Proteins = 7,
                Carbohydrate = 0.5f,
                Fats = 6
            },
            new Product() {
                Name = "Цибуля ріпчаста",
                Amount = 1,
                Weight = 70,
                Unit = "шт",
                Calories = 43,
                Proteins = 1.5f,
                Carbohydrate = 9,
                Fats = 0.25f
            },
            new Product() {
                Name = "Олія соняшникова",
                Amount = 1,
                Weight = 9.3f,
                Unit = "ст. л.",
                Calories = 90,
                Proteins = 0.01f,
                Carbohydrate = 0.01f,
                Fats = 9.95f
            },
            new Product() {
                Name = "Сіль, чорний перець"
            },
            new Product() {
                Name = "Сухарі панірувальні",
                Amount = 10,
                Unit = "г",
                Calories = 24,
                Proteins = 1,
                Carbohydrate = 5,
                Fats = 0.2f
            },
            new Product() {
                Name = "Ребра свинячі",
                Amount = 100,
                Unit = "г",
                Calories = 321,
                Proteins = 29,
                Carbohydrate = 0,
                Fats = 29.3f
            },
            new Product() {
                Name = "Вода",
                Amount = 100,
                Unit = "мл",
                Calories = 0,
                Proteins = 0,
                Carbohydrate = 0,
                Fats = 0
            },
            new Product() {
                Name = "Крупа перлова",
                Amount = 100,
                Unit = "г",
                Calories = 324,
                Proteins = 9.3f,
                Carbohydrate = 73.7f,
                Fats = 1.1f
            },
            new Product() {
                Name = "Картопля",
                Amount = 1,
                Unit = "шт",
                Weight = 130,
                Calories = 120,
                Proteins = 2.3f,
                Carbohydrate = 24f,
                Fats = 0.1f
            },
            new Product() {
                Name = "Морква",
                Amount = 1,
                Unit = "шт",
                Weight = 85,
                Calories = 35,
                Proteins = 1,
                Carbohydrate = 7.3f,
                Fats = 0.2f
            },
            new Product() {
                Name = "Огірок солоний",
                Amount = 1,
                Unit = "шт",
                Weight = 60,
                Calories = 8,
                Proteins = 1,
                Carbohydrate = 1.2f,
                Fats = 0.05f
            },
            new Product() {
                Name = "Лавровий лист",
                Amount = 1,
                Unit = "шт",
                Weight = 5,
                Calories = 19,
                Proteins = 0.5f,
                Carbohydrate = 3.5f,
                Fats = 0.3f
            },
            new Product() {
                Name = "Кріп, петрушка"
            },
            new Product() {
                Name = "Огірковий розсіл",
                Amount = 100,
                Unit = "мл",
                Calories = 12,
                Proteins = 0.3f,
                Carbohydrate = 3,
                Fats = 0
            },
        };


        // Рецепти.
        List<Recipe> recipes = new List<Recipe>()
        {
            new Recipe() {
                PhotoName = "pizza-cheese-tomatoes-olives.jpg",
                Name = "Оливкова піца",
                Description = "Оливка піца з сиром та томатним соусом - це прекрасна комбінація смаків та ароматів, яка задовольнить ваші гастрономічні бажання. Ця піца поєднує в собі солодкий смак томатного соусу, ароматний сир та інтенсивний смак оливок.\r\n\r\nПіца приготована на тонкому або товстому тісті, в залежності від вашого вибору. Тонке тісто надає піці легкість, тоді як товсте тісто робить її більш насиченою та ситною.\r\n\r\nНа тонкому або товстому коржі рівномірно розподілено томатний соус, який надає піці основний та смачний смак. Верхню частину покриває або ковбаса моцарелли, або комбінація сирів, таких як пармезан, чеддер або гауда.\r\n\r\nОсновна ідея полягає в тому, щоб нарізати оливки на тонкі кільця та рівномірно розподілити їх по всій піці. Це додає піці не тільки смак оливок, але й гарний зовнішній вигляд.\r\n\r\nПісля того, як піца випічена, на неї можна додати свіжі зелені, такі як базилік чи рукола, для додаткового аромату та свіжості.",
                Difficulty = "Нормально",
                Views = 76,
                CookingTime = TimeSpan.FromMinutes(40),
                User = users[0],
                Category = categories[2] },

            new Recipe() {
                PhotoName = "chef-salad.jpg",
                Name = "Шеф-салат",
                Description = "Шеф-салат - це вишукана та смачна страва, яка зазвичай подається в ресторанах як підкреслення кулінарної майстерності шеф-кухаря. Одним із класичних варіантів є \"Цезар\", який являє собою комбінацію свіжих листків салату, курячого філе, гартованого яйця, гарячого бекону, пармезану та ароматного соусу.\r\n\r\nОсновні складники шеф-салату можуть варіюватися залежно від рецепту та кухні, але зазвичай вони включають в себе свіжі овочі, м'ясо або рибу, сир, гарнір та соус.",
                Difficulty = "Легко",
                Views = 43,
                CookingTime = TimeSpan.FromMinutes(25),
                User = users[0],
                Category = categories[1] },

            new Recipe() {
                PhotoName = "chocolate-cake.jpg",
                Name = "Шоколадний тортик з полуницями",
                Description = "Шоколадний торт з полуницями - це чудовий десерт, який поєднує в собі солодкість шоколадного бісквіта з ароматом свіжих полуниць.",
                Difficulty = "Складно",
                Views = 58,
                CookingTime = TimeSpan.FromMinutes(80),
                User = users[0],
                Category = categories[0] },

            new Recipe() {
                PhotoName = "rybni-kotlety.jpg",
                Name = "Рибні котлети",
                Description = "Рибні котлети – це ароматна, неймовірно смачна та дієтична страва. Вони на тривалий час насичують організм, ідеально поєднується з різноманітними гарнірами та соусами. Ніжні рибні котлети стануть чудовою альтернативою звичайній смаженій рибі.",
                Difficulty = "Нормально",
                Views = 22,
                CookingTime = TimeSpan.FromMinutes(60),
                User = users[0],
                Category = categories[3] }
        };

        // Кроки для приготування рецепту.
        List<CookingStep> cookingSteps = new List<CookingStep>()
        {
            // Для рибних котлет.
            new CookingStep() { StepIndex = 1, Title = "Для приготування рибних котлет філе минтая промиваємо холодною водою і просушуємо. Потім ріжемо на середні шматочки.",  Recipe = recipes[3] },
            new CookingStep() { StepIndex = 2, Title = "Очищаємо від лушпиння цибулю та ділимо її на дві частини.",  Recipe = recipes[3] },
            new CookingStep() { StepIndex = 3, Title = "Половину цибулі нарізаємо маленькими шматочками та смажимо до прозорості на розігрітій соняшниковій олії. Вона додасть котлетам особливої солодкості та смаку.",  Recipe = recipes[3] },
            new CookingStep() { StepIndex = 4, Title = "Минтай перекручуємо разом із другою половинкою цибулі через м'ясорубку із середньою решіткою.",  Recipe = recipes[3] },
            new CookingStep() { StepIndex = 5, Title = "Замочуємо в молоці скибочки білого хліба. Потім їх добре віджимаємо, перекручуємо на м’ясорубці та кладемо в рибний фарш.",  Recipe = recipes[3] },
            new CookingStep() { StepIndex = 6, Title = "Додаємо у фарш куряче яйце, подрібнений свіжий кріп і смажену цибулю. Посипаємо фарш для рибних котлет сіллю та чорним перцем і рукою добре замішуємо.",  Recipe = recipes[3] },
            new CookingStep() { StepIndex = 7, Title = "Щоб котлети було легше ліпити, і вони зберегли форму під час смаження, поставимо фарш у холодильник на 20-30 хвилин.", Recipe = recipes[3] },
            new CookingStep() { StepIndex = 8, Title = "Руками, змоченими в холодній воді, або за допомогою ложки, робимо з фаршу красиві однакові котлетки.",  Recipe = recipes[3] },
            new CookingStep() { StepIndex = 9, Title = "Насипаємо у мисочку панірувальні сухарі й кожну котлету обвалюємо в них.",  Recipe = recipes[3] },
            new CookingStep() { StepIndex = 10, Title = "Смажимо рибні котлети у розігрітій соняшниковій олії до отримання рум'яної скоринки з обох боків.", RecipeId = 4 },
            new CookingStep() { StepIndex = 11, Title = "Додаємо до рибних котлет на гарнір картоплю або рис і подаємо. Смачного!",  Recipe = recipes[3] },
        };


        // Cписок інгредієнтів.
        List<Ingredient> ingredients = new List<Ingredient>()
        {
            new Ingredient() {
                Amount = 500,
                Product = products[0],
                Recipe = recipes[3]
            },
            new Ingredient() {
                Amount = 100,
                Product = products[1],
                Recipe = recipes[3]
            },
            new Ingredient() {
                Amount = 100,
                Product = products[2],
                Recipe = recipes[3]
            },
            new Ingredient() {
                Amount = 1,
                Product = products[3],
                Recipe = recipes[3]
            },
            new Ingredient() {
                Amount = 1,
                Product = products[4],
                Recipe = recipes[3]
            },
            new Ingredient() {
                Amount = 2,
                Product = products[5],
                Recipe = recipes[3]
            },
            new Ingredient() {
                Amount = 50,
                Product = products[6],
                Recipe = recipes[3]
            },
            new Ingredient() {
                Product = products[7],
                Recipe = recipes[3]
            },
        };

        // Коментарі рецептів.
        List<Comment> comments = new List<Comment>()
        {
            new Comment() { UserName = "Олег", Title = "Все вийшло, рецепт дуже класний, раджу всім!", Rating = 5, Recipe = recipes[3] },
            new Comment() { UserName = "Катерина", Title = "Спочатку трішки не вийшло з чисткою риби, але потім все зробила так як треба, чоловіку дуже сподобалось:)", Rating = 4, Recipe = recipes[3] }
        };

        List<Task> tasks = new List<Task>()
        {
            db.Users.AddRangeAsync(users),
            db.Categories.AddRangeAsync(categories),
            db.Recipes.AddRangeAsync(recipes),
            db.CookingSteps.AddRangeAsync(cookingSteps),
            db.Comments.AddRangeAsync(comments),
            db.Products.AddRangeAsync(products),
            db.Ingredients.AddRangeAsync(ingredients),
        };

        Task.WaitAll(tasks.ToArray());
        db.SaveChanges();
        return db;
    }

    /// <summary>
    /// Видаляє базу даних та вивільняє ресурси.
    /// </summary>
    /// <param name="db">Екземпляр бази даних RecipeDbContext.</param>
    public static void Destroy(RecipeDbContext db)
    {
        db.Database.EnsureDeleted();
        db.Dispose();
    }
}
