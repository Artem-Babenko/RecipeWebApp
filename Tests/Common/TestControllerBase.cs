using Microsoft.Extensions.Caching.Memory;
using RecipeWebApp.Models;

namespace RecipeWebApp.Tests.Common;

/// <summary>
/// Абстрактний базовий клас для тестувальних контролерів, що забезпечує доступ до екземпляру <see cref="RecipeDbContext"/> та <see cref="IMemoryCache"/>.
/// </summary>
public abstract class TestControllerBase : IDisposable
{
    /// <summary>
    /// Екземпляр бази даних RecipeDbContext для використання у тестах.
    /// </summary>
    protected readonly RecipeDbContext DatabaseContext;

    /// <summary>
    /// Об'єкт MemoryCache для використання у тестах.
    /// </summary>
    protected readonly IMemoryCache MemoryCache;

    /// <summary>
    /// Конструктор класу TestControllerBase, що ініціалізує екземпляр RecipeDbContext та MemoryCache для використання у тестах.
    /// </summary>
    public TestControllerBase()
    {
        DatabaseContext = RecipeDbContextFactory.Create();
        MemoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    /// <summary>
    /// Вивільнює ресурси та знищує екземпляр бази даних після завершення тестів.
    /// </summary>
    public void Dispose()
    {
        RecipeDbContextFactory.Destroy(DatabaseContext);
    }
}
