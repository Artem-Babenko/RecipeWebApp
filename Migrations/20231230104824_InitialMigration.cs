using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipeCount = table.Column<int>(type: "int", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Views = table.Column<int>(type: "int", nullable: false),
                    CookingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recipes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CountOfLikes = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CookingSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StepIndex = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookingSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CookingSteps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentCategoryId", "RecipeCount" },
                values: new object[,]
                {
                    { 1, "Перші страви", null, 0 },
                    { 2, "Другі страви", null, 0 },
                    { 3, "Закуски", null, 0 },
                    { 4, "Салати", null, 0 },
                    { 5, "Випічка", null, 0 },
                    { 6, "Десерти", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "Email", "Gender", "Name", "Password", "PhoneNumber", "Surname" },
                values: new object[] { 1, null, "artem@gmail.com", null, "Артем", "12345678", null, "Бабенко" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentCategoryId", "RecipeCount" },
                values: new object[,]
                {
                    { 7, "Гарячі супи", 1, 0 },
                    { 8, "Холодні супи", 1, 0 },
                    { 9, "Бульйони", 1, 0 },
                    { 10, "Страви з м'яса", 2, 0 },
                    { 11, "Страви з риби", 2, 0 },
                    { 12, "Страви з овочів, грибів", 2, 0 },
                    { 13, "Страви з я'єць", 2, 0 },
                    { 14, "Гарнір", 2, 0 },
                    { 15, "Паста", 2, 0 },
                    { 16, "Запіканки", 2, 0 },
                    { 17, "Бутерброди", 3, 0 },
                    { 18, "Рулети", 3, 0 },
                    { 19, "Паштети", 3, 0 },
                    { 20, "Снеки", 3, 0 },
                    { 21, "Салати з м'яса", 4, 0 },
                    { 22, "Салати з риби", 4, 0 },
                    { 23, "Салити з овочів, грибів", 4, 0 },
                    { 24, "Млинці, оладки, сирники", 5, 0 },
                    { 25, "Піца", 5, 0 },
                    { 26, "Вироби з тіста", 5, 0 },
                    { 27, "Торти", 6, 0 },
                    { 28, "Печиво", 6, 0 },
                    { 29, "Тістечка", 6, 0 },
                    { 30, "Цукерки", 6, 0 },
                    { 31, "Морозиво", 6, 0 },
                    { 32, "Фруктові десерти", 6, 0 },
                    { 111, "Страви з морепродуктів", 2, 0 },
                    { 222, "Салати з морепродуктів", 4, 0 },
                    { 324, "Caлат з курятини", 21, 0 },
                    { 325, "Caлат з cвинини", 21, 0 },
                    { 326, "Caлат з яловичини", 21, 0 }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CategoryId", "CookingTime", "CreateDate", "Description", "Difficulty", "Name", "PhotoName", "Rating", "UserId", "Views" },
                values: new object[,]
                {
                    { 1, 25, new TimeSpan(0, 0, 40, 0, 0), new DateTime(2023, 12, 30, 12, 48, 24, 341, DateTimeKind.Local).AddTicks(1488), "Оливка піца з сиром та томатним соусом - це прекрасна комбінація смаків та ароматів, яка задовольнить ваші гастрономічні бажання. Ця піца поєднує в собі солодкий смак томатного соусу, ароматний сир та інтенсивний смак оливок.\r\n\r\nПіца приготована на тонкому або товстому тісті, в залежності від вашого вибору. Тонке тісто надає піці легкість, тоді як товсте тісто робить її більш насиченою та ситною.\r\n\r\nНа тонкому або товстому коржі рівномірно розподілено томатний соус, який надає піці основний та смачний смак. Верхню частину покриває або ковбаса моцарелли, або комбінація сирів, таких як пармезан, чеддер або гауда.\r\n\r\nОсновна ідея полягає в тому, щоб нарізати оливки на тонкі кільця та рівномірно розподілити їх по всій піці. Це додає піці не тільки смак оливок, але й гарний зовнішній вигляд.\r\n\r\nПісля того, як піца випічена, на неї можна додати свіжі зелені, такі як базилік чи рукола, для додаткового аромату та свіжості.", "Нормально", "Оливкова піца", "pizza-cheese-tomatoes-olives.jpg", 4.7f, 1, 76 },
                    { 3, 27, new TimeSpan(0, 1, 20, 0, 0), new DateTime(2023, 12, 30, 12, 48, 24, 341, DateTimeKind.Local).AddTicks(1498), "Шоколадний торт з полуницями - це чудовий десерт, який поєднує в собі солодкість шоколадного бісквіта з ароматом свіжих полуниць.", "Складно", "Шоколадний тортик з полуницями", "chocolate-cake.jpg", 4.3f, 1, 58 },
                    { 4, 11, new TimeSpan(0, 1, 0, 0, 0), new DateTime(2023, 12, 30, 12, 48, 24, 341, DateTimeKind.Local).AddTicks(1501), "Рибні котлети – це ароматна, неймовірно смачна та дієтична страва. Вони на тривалий час насичують організм, ідеально поєднується з різноманітними гарнірами та соусами. Ніжні рибні котлети стануть чудовою альтернативою звичайній смаженій рибі.", "Нормально", "Рибні котлети", "rybni-kotlety.jpg", 4.5f, 1, 22 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "CountOfLikes", "CreateTime", "Rating", "RecipeId", "Title", "UserName" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2023, 12, 30, 12, 48, 24, 341, DateTimeKind.Local).AddTicks(1424), 5f, 4, "Все вийшло, рецепт дуже класний, раджу всім!", "Олег" },
                    { 2, 0, new DateTime(2023, 12, 30, 12, 48, 24, 341, DateTimeKind.Local).AddTicks(1470), 4f, 4, "Спочатку трішки не вийшло з чисткою риби, але потім все зробила так як треба, чоловіку дуже сподобалось:)", "Катерина" }
                });

            migrationBuilder.InsertData(
                table: "CookingSteps",
                columns: new[] { "Id", "RecipeId", "StepIndex", "Title" },
                values: new object[,]
                {
                    { 1, 4, 1, "Для приготування рибних котлет філе минтая промиваємо холодною водою і просушуємо. Потім ріжемо на середні шматочки." },
                    { 2, 4, 2, "Очищаємо від лушпиння цибулю та ділимо її на дві частини." },
                    { 3, 4, 3, "Половину цибулі нарізаємо маленькими шматочками та смажимо до прозорості на розігрітій соняшниковій олії. Вона додасть котлетам особливої солодкості та смаку." },
                    { 4, 4, 4, "Минтай перекручуємо разом із другою половинкою цибулі через м'ясорубку із середньою решіткою." },
                    { 5, 4, 5, "Замочуємо в молоці скибочки білого хліба. Потім їх добре віджимаємо, перекручуємо на м’ясорубці та кладемо в рибний фарш." },
                    { 6, 4, 6, "Додаємо у фарш куряче яйце, подрібнений свіжий кріп і смажену цибулю. Посипаємо фарш для рибних котлет сіллю та чорним перцем і рукою добре замішуємо." },
                    { 7, 4, 7, "Щоб котлети було легше ліпити, і вони зберегли форму під час смаження, поставимо фарш у холодильник на 20-30 хвилин." },
                    { 8, 4, 8, "Руками, змоченими в холодній воді, або за допомогою ложки, робимо з фаршу красиві однакові котлетки." },
                    { 9, 4, 9, "Насипаємо у мисочку панірувальні сухарі й кожну котлету обвалюємо в них." },
                    { 10, 4, 10, "Смажимо рибні котлети у розігрітій соняшниковій олії до отримання рум'яної скоринки з обох боків." },
                    { 11, 4, 11, "Додаємо до рибних котлет на гарнір картоплю або рис і подаємо. Смачного!" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Amount", "Name", "RecipeId", "Unit" },
                values: new object[,]
                {
                    { 1, 500.0, "Філе минтая", 4, "гр" },
                    { 2, 6.0, "Білий хліб", 4, "шматочків" },
                    { 3, 100.0, "Молоко", 4, "мл" },
                    { 4, 1.0, "Яйце куряче", 4, "шт" },
                    { 5, 1.0, "Цибуля ріпчаста", 4, "шт" },
                    { 6, 2.0, "Олія соняшникова", 4, "ст. л." },
                    { 7, 0.0, "Сіль, чорний перець, кріп", 4, null },
                    { 8, 0.0, "Сухарі панірувальні", 4, null }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CategoryId", "CookingTime", "CreateDate", "Description", "Difficulty", "Name", "PhotoName", "Rating", "UserId", "Views" },
                values: new object[] { 2, 324, new TimeSpan(0, 0, 25, 0, 0), new DateTime(2023, 12, 30, 12, 48, 24, 341, DateTimeKind.Local).AddTicks(1494), "Шеф-салат - це вишукана та смачна страва, яка зазвичай подається в ресторанах як підкреслення кулінарної майстерності шеф-кухаря. Одним із класичних варіантів є \"Цезар\", який являє собою комбінацію свіжих листків салату, курячого філе, гартованого яйця, гарячого бекону, пармезану та ароматного соусу.\r\n\r\nОсновні складники шеф-салату можуть варіюватися залежно від рецепту та кухні, але зазвичай вони включають в себе свіжі овочі, м'ясо або рибу, сир, гарнір та соус.", "Легко", "Шеф-салат", "chef-salad.jpg", 3.8f, 1, 43 });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RecipeId",
                table: "Comments",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_CookingSteps_RecipeId",
                table: "CookingSteps",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserId",
                table: "Recipes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CookingSteps");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
