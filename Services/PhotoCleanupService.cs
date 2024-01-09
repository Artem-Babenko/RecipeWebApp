using Microsoft.EntityFrameworkCore;
using RecipeWebApp.Models;
namespace RecipeWebApp.Services;

/// <summary>
/// Служба очистки тимчасових фотографій.
/// </summary>
public class PhotoCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private const int CleanupIntervalSeconds = 15;
    private const int MaxPhotoAgeSeconds = 30;

    /// <summary>
    /// Конструктор служби очистки тимчасових фотографій.
    /// </summary>
    public PhotoCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Основний метод виконання асинхронної роботи служби.
    /// </summary>
    /// <param name="stoppingToken">Маркер скасування для визначення зупинки служби.</param>
    /// <returns>Задача, представляюча виконання асинхронної роботи служби.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Отримання контексту бази даних.
                var dbContext = scope.ServiceProvider.GetRequiredService<RecipeDbContext>(); 

                // Отримання списку тимчасових фотографій з бази даних.
                var temporaryPhotos = await dbContext.TemporaryPhotos.ToListAsync();

                // Відбір фотографій для видалення на основі CreationTime.
                var photosToDelete = temporaryPhotos
                    .Where(photo => (DateTime.Now - photo.CreationTime).TotalSeconds > MaxPhotoAgeSeconds)
                    .ToList();

                foreach (var photo in photosToDelete)
                {
                    // Шлях до фото яке треба видилити.
                    string filePath = $"{Directory.GetCurrentDirectory()}/wwwroot/photos/{photo.Path}";
                    // Перевірка чи фото існує.
                    if (File.Exists(filePath))
                    {
                        // Видаляємо фотографію з файлової системи.
                        File.Delete(filePath);
                        // Видаляємо фотографію з бази даних.
                        dbContext.TemporaryPhotos.Remove(photo);
                    }
                }

                // Логування у консоль.
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("TemporaryPhotoCleanupService");
                Console.ResetColor();
                Console.WriteLine($": Check complited.\nTime: {DateTime.Now}. Deleted count: {photosToDelete.Count}/{temporaryPhotos.Count}.\n");

                // Збереження бази даних.
                await dbContext.SaveChangesAsync();
            }

            // Затримка перед наступною перевіркою
            await Task.Delay(TimeSpan.FromSeconds(CleanupIntervalSeconds), stoppingToken);
        }
    }
}