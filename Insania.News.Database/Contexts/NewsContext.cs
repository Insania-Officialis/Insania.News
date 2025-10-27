using Microsoft.EntityFrameworkCore;

using Insania.News.Entities;

using NewsEntity = Insania.News.Entities.News;

namespace Insania.News.Database.Contexts;

/// <summary>
/// Контекст базы данных новостей
/// </summary>
public class NewsContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста базы данных новостей
    /// </summary>
    public NewsContext() : base()
    {
    }

    /// <summary>
    /// Конструктор контекста базы данных новостей с опциями
    /// </summary>
    /// <param cref="DbContextOptions{NewsContext}" name="options">Параметры подключения</param>
    public NewsContext(DbContextOptions<NewsContext> options) : base(options)
    {
    }
    #endregion

    #region Поля
    /// <summary>
    /// Типы новостей
    /// </summary>
    public virtual DbSet<NewsType> NewsTypes { get; set; }

    /// <summary>
    /// Новости
    /// </summary>
    public virtual DbSet<NewsEntity> News { get; set; }

    /// <summary>
    /// Детальные части новостей
    /// </summary>
    public virtual DbSet<NewsDetail> NewsDetails { get; set; }

    /// <summary>
    /// Параметры
    /// </summary>
    public virtual DbSet<ParameterNews> Parameters { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод конфигурации моделей при создании
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Построитель моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы базы данных
        modelBuilder.HasDefaultSchema("insania_news");

        //Ограничение уникальности для псевдонима типа новости
        modelBuilder.Entity<NewsType>().HasAlternateKey(x => x.Alias);

        //Ограничение уникальности для параметра
        modelBuilder.Entity<ParameterNews>().HasAlternateKey(x => x.Alias);
    }
    #endregion
}