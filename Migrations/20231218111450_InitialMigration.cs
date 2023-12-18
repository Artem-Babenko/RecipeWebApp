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
                    PluralName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<float>(type: "real", nullable: false),
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
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                columns: new[] { "Id", "Name", "PluralName" },
                values: new object[,]
                {
                    { 1, "Піца", "Піци" },
                    { 2, "Cалат", "Салати" },
                    { 3, "Десерт", "Десерти" },
                    { 4, "Випічка", "Випічка" },
                    { 5, "Суп", "Супи" },
                    { 6, "Каша", "Каші" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Name", "Password", "Surname" },
                values: new object[] { 1, "artem123", "Артем", "12345678", null });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CategoryId", "CookingTime", "CreateDate", "Description", "Name", "PhotoPath", "Rating", "UserId", "Views" },
                values: new object[,]
                {
                    { 1, 1, new TimeSpan(0, 0, 40, 0, 0), new DateTime(2023, 12, 18, 13, 14, 50, 837, DateTimeKind.Local).AddTicks(6808), "Оливка піца з сиром та томатним соусом - це прекрасна комбінація смаків та ароматів, яка задовольнить ваші гастрономічні бажання. Ця піца поєднує в собі солодкий смак томатного соусу, ароматний сир та інтенсивний смак оливок.\r\n\r\nПіца приготована на тонкому або товстому тісті, в залежності від вашого вибору. Тонке тісто надає піці легкість, тоді як товсте тісто робить її більш насиченою та ситною.\r\n\r\nНа тонкому або товстому коржі рівномірно розподілено томатний соус, який надає піці основний та смачний смак. Верхню частину покриває або ковбаса моцарелли, або комбінація сирів, таких як пармезан, чеддер або гауда.\r\n\r\nОсновна ідея полягає в тому, щоб нарізати оливки на тонкі кільця та рівномірно розподілити їх по всій піці. Це додає піці не тільки смак оливок, але й гарний зовнішній вигляд.\r\n\r\nПісля того, як піца випічена, на неї можна додати свіжі зелені, такі як базилік чи рукола, для додаткового аромату та свіжості.", "Оливкова піца", "pizza-cheese-tomatoes-olives.jpg", 4.7f, 1, 76 },
                    { 2, 2, new TimeSpan(0, 0, 25, 0, 0), new DateTime(2023, 12, 18, 13, 14, 50, 837, DateTimeKind.Local).AddTicks(6898), "Шеф-салат - це вишукана та смачна страва, яка зазвичай подається в ресторанах як підкреслення кулінарної майстерності шеф-кухаря. Одним із класичних варіантів є \"Цезар\", який являє собою комбінацію свіжих листків салату, курячого філе, гартованого яйця, гарячого бекону, пармезану та ароматного соусу.\r\n\r\nОсновні складники шеф-салату можуть варіюватися залежно від рецепту та кухні, але зазвичай вони включають в себе свіжі овочі, м'ясо або рибу, сир, гарнір та соус.", "Шеф-салат", "chef-salad.jpg", 3.8f, 1, 43 },
                    { 3, 3, new TimeSpan(0, 1, 20, 0, 0), new DateTime(2023, 12, 18, 13, 14, 50, 837, DateTimeKind.Local).AddTicks(6902), "Шоколадний торт з полуницями - це чудовий десерт, який поєднує в собі солодкість шоколадного бісквіта з ароматом свіжих полуниць.", "Шоколадний тортик з полуницями", "chocolate-cake.jpg", 4.3f, 1, 58 }
                });

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
