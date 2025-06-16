using NewsEntity = Insania.News.Entities.News;

namespace Insania.News.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными новостей
/// </summary>
public interface INewsDAO
{
    /// <summary>
    /// Метод получения списка новостей
    /// </summary>
    /// <returns cref="List{NewsEntity}">Список новостей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<NewsEntity>> GetList();
}