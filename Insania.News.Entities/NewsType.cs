using System.ComponentModel.DataAnnotations.Schema;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.News.Entities;

/// <summary>
/// Модель сущности типа новости
/// </summary>
[Table("с_news_types")]
public class NewsType : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности типа новости
    /// </summary>
    public NewsType() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа новости без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование типа</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public NewsType(ITransliterationSL transliteration, string username, string name, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа новости с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование типа</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public NewsType(ITransliterationSL transliteration, long id, string username, string name, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {

    }
    #endregion
}