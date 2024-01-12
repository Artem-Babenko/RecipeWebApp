
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;
using RecipeWebApp.Services;


WebApplicationBuilder builder = WebApplication.CreateBuilder();
// Додавання служби для очиження тимчасовий фото.
builder.Services.AddHostedService<PhotoCleanupService>();
// Додавання контролерів.
builder.Services.AddControllers();
// Додавання контексту бази даних.
builder.Services.AddDbContext<RecipeDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
// Додавання стиснення відповіді.
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "text/html",
        "application/json"
    });
});
// Додавання аутентифікації.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.HttpOnly = false;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Name = "AuthenticationCookie";
    options.AccessDeniedPath = "/login";
    options.LoginPath = "/login";
});
// Додавання авторизації.
builder.Services.AddAuthorization();
// Додавання кешування.
builder.Services.AddMemoryCache();
// Додавання сервісу Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); 
app.UseAuthentication();  
app.UseAuthorization();   
app.UseResponseCompression();

app.MapControllers();

app.Run();