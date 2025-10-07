using Insania.News.Entities;

namespace Insania.News.Contracts.Services;

/// <summary>
/// Интерфейс сервиса фонового логгирования в бд
/// </summary>
public interface ILoggingSL
{
    /// <summary>
    /// Метод постановки лога в очередь на обработку
    /// </summary>
    /// <param cref="LogApiNews" name="log">Лог для записи</param>
    /// <returns cref="ValueTask">Задание</returns>
    ValueTask QueueLogAsync(LogApiNews log);
}