using Insania.News.Entities;

namespace Insania.News.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными параметров
/// </summary>
public interface IParametersDAO
{
    /// <summary>
    /// Метод получения списка параметров
    /// </summary>
    /// <returns cref="List{ParameterNews}">Список параметров</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<ParameterNews>> GetList();
}