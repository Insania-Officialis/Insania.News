using Microsoft.EntityFrameworkCore;

using Insania.News.Entities;

namespace Insania.News.Database.Contexts;

/// <summary>
/// Контекст базы данных логов сервиса новостей
/// </summary>
public class LogsApiNewsContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста базы данных логов сервиса новостей
    /// </summary>
    public LogsApiNewsContext() : base()
    {
    }

    /// <summary>
    /// Конструктор контекста базы данных логов сервиса новостей с опциями
    /// </summary>
    /// <param cref="DbContextOptions{LogsApiNewsContext}" name="options">Параметры подключения</param>
    public LogsApiNewsContext(DbContextOptions<LogsApiNewsContext> options) : base(options)
    {
    }
    #endregion

    #region Свойства
    /// <summary>
    /// Логи
    /// </summary>
    public virtual DbSet<LogApiNews> Logs { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод при создании моделей
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Конструктор моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы базы данных
        modelBuilder.HasDefaultSchema("insania_logs_api_news");

        //Добавление gin-индекса на поле с входными данными логов
        modelBuilder.Entity<LogApiNews>().HasIndex(x => x.DataIn).HasMethod("gin");

        //Добавление gin-индекса на поле с выходными данными логов
        modelBuilder.Entity<LogApiNews>().HasIndex(x => x.DataOut).HasMethod("gin");
    }
    #endregion
}