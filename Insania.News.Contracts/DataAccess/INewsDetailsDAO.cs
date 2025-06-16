using Insania.News.Entities;

namespace Insania.News.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными детальных частей новостей
/// </summary>
public interface INewsDetailsDAO
{
    /// <summary>
    /// Метод получения списка детальных частей новостей
    /// </summary>
    /// <returns cref="List{NewsDetail}">Список детальных частей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<NewsDetail>> GetList();
}