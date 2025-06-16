using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.News.Entities;

/// <summary>
/// Модель сущности детальной части новости
/// </summary>
[Table("r_news_details")]
public class NewsDetail : Reestr
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности детальной части новости
    /// </summary>
    public NewsDetail() : base()
    {
        Title = string.Empty;
        Content = string.Empty;
        NewsEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности детальной части новости без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="title">Заголовок детальной части</param>
    /// <param cref="string" name="content">Содержание детальной части</param>
    /// <param cref="News" name="news">Новость</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public NewsDetail(string username, bool isSystem, string title, string content, News news, DateTime? dateDeleted = null)
        : base(username, isSystem, dateDeleted)
    {
        Title = title;
        Content = content;
        NewsEntity = news;
        NewsId = news.Id;
    }

    /// <summary>
    /// Конструктор модели сущности детальной части новости с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="title">Заголовок детальной части</param>
    /// <param cref="string" name="content">Содержание детальной части</param>
    /// <param cref="News" name="news">Новость</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public NewsDetail(long id, string username, bool isSystem, string title, string content, News news, DateTime? dateDeleted = null)
        : base(id, username, isSystem, dateDeleted)
    {
        Title = title;
        Content = content;
        NewsEntity = news;
        NewsId = news.Id;
    }
    #endregion

    #region Свойства
    /// <summary>
    /// Заголовок детальной части
    /// </summary>
    [Column("title")]
    [Comment("Заголовок детальной части")]
    public string Title { get; private set; }

    /// <summary>
    /// Содержание детальной части
    /// </summary>
    [Column("content")]
    [Comment("Содержание детальной части")]
    public string Content { get; private set; }

    /// <summary>
    /// Идентификатор новости
    /// </summary>
    [Column("news_id")]
    [Comment("Идентификатор новости")]
    public long NewsId { get; private set; }

    /// <summary>
    /// Навигационное свойство новости
    /// </summary>
    [ForeignKey("NewsId")]
    public News NewsEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи заголовка
    /// </summary>
    /// <param cref="string" name="title">Заголовок детальной части</param>
    public void SetTitle(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Метод записи содержания
    /// </summary>
    /// <param cref="string" name="content">Содержание детальной части</param>
    public void SetContent(string content)
    {
        Content = content;
    }

    /// <summary>
    /// Метод записи новости
    /// </summary>
    /// <param cref="News" name="news">Новость</param>
    public void SetNews(News news)
    {
        NewsEntity = news;
        NewsId = news.Id;
    }
    #endregion
}