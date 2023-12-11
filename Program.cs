

using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;

class Program
{
    static void Main()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<RecipeDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
            {
                "text/html",
                "text/css",
                "text/javascript",
                "application/json"
            });
        });
        WebApplication app = builder.Build();

        app.UseStaticFiles();
        app.UseResponseCompression();

        app.MapControllers();

        app.Run();
    }
}