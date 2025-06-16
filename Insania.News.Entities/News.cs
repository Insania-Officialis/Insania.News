using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.News.Entities;

/// <summary>
/// Модель сущности новости
/// </summary>
[Table("r_news")]
public class News : Reestr
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности новости
    /// </summary>
    public News() : base()
    {
        Title = string.Empty;
        Description = string.Empty;
        TypeEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности новости без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="title">Заголовок новости</param>
    /// <param cref="string" name="description">Описание новости</param>
    /// <param cref="NewsType" name="type">Тип новости</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public News(string username, bool isSystem, string title, string description, NewsType type, DateTime? dateDeleted = null) : base(username, isSystem, dateDeleted)
    {
        Title = title;
        Description = description;
        TypeEntity = type;
        TypeId = type.Id;
    }

    /// <summary>
    /// Конструктор модели сущности новости с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="title">Заголовок новости</param>
    /// <param cref="string" name="description">Описание новости</param>
    /// <param cref="NewsType" name="type">Тип новости</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public News(long id, string username, bool isSystem, string title, string description, NewsType type, DateTime? dateDeleted = null) : base(id, username, isSystem, dateDeleted)
    {
        Title = title;
        Description = description;
        TypeEntity = type;
        TypeId = type.Id;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Заголовок новости
    /// </summary>
    [Column("title")]
    [Comment("Заголовок новости")]
    public string Title { get; private set; }

    /// <summary>
    /// Описание новости
    /// </summary>
    [Column("description")]
    [Comment("Описание новости")]
    public string Description { get; private set; }

    /// <summary>
    /// Тип новости
    /// </summary>
    [Column("type_id")]
    [Comment("Тип новости")]
    public long TypeId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство типа новости
    /// </summary>
    [ForeignKey("TypeId")]
    public NewsType TypeEntity { get; private set; }
    #endregion

    #region Методы

    /// <summary>
    /// Метод записи заголовка
    /// </summary>
    /// <param cref="string" name="title">Заголовок новости</param>
    public void SetTitle(string title) => Title = title;

    /// <summary>
    /// Метод записи описания
    /// </summary>
    /// <param cref="string" name="description">Описание новости</param>
    public void SetDescription(string description) => Description = description;

    /// <summary>
    /// Метод записи типа новости
    /// </summary>
    /// <param cref="NewsType" name="type">Тип новости</param>
    public void SetType(NewsType type)
    {
        TypeEntity = type;
        TypeId = type.Id;
    }
    #endregion
}