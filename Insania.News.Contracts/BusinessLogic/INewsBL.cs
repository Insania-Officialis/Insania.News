using Insania.Shared.Models.Responses.Base;

namespace Insania.News.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой новостей
/// </summary>
public interface INewsBL
{
    /// <summary>
    /// Метод получения списка новостей
    /// </summary>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <remarks>Список новостей</remarks>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponseList> GetList();
}