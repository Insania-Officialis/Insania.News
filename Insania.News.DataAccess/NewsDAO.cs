using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.News.Contracts.DataAccess;
using Insania.News.Database.Contexts;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

using NewsEntity = Insania.News.Entities.News;
using InformationMessages = Insania.News.Messages.InformationMessages;

namespace Insania.News.DataAccess;

/// <summary>
/// Сервис работы с данными новостей
/// </summary>
/// <param cref="ILogger{NewsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="NewsContext" name="context">Контекст базы данных новостей</param>
public class NewsDAO(ILogger<NewsDAO> logger, NewsContext context) : INewsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<NewsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных новостей
    /// </summary>
    private readonly NewsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка новостей
    /// </summary>
    /// <returns cref="List{NewsEntity}">Список новостей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<NewsEntity>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListNewsMethod);

            //Получение данных из бд
            List<NewsEntity> data = await _context.News.Where(x => x.DateDeleted == null).ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование ошибки
            _logger.LogError("{text}: {error}", ErrorMessages.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}