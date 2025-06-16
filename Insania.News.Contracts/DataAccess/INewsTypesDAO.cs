using Insania.News.Entities;

namespace Insania.News.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными типов новостей
/// </summary>
public interface INewsTypesDAO
{
    /// <summary>
    /// Метод получения списка типов новостей
    /// </summary>
    /// <returns cref="List{NewsType}">Список типов новостей</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<NewsType>> GetList();
}