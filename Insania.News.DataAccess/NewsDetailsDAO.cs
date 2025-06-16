using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.News.Contracts.DataAccess;
using Insania.News.Database.Contexts;
using Insania.News.Entities;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;
using InformationMessages = Insania.News.Messages.InformationMessages;

namespace Insania.News.DataAccess;

/// <summary>
/// Сервис работы с данными детальных частей новостей
/// </summary>
/// <param cref="ILogger{NewsDetailsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="NewsContext" name="context">Контекст базы данных новостей</param>
public class NewsDetailsDAO(ILogger<NewsDetailsDAO> logger, NewsContext context) : INewsDetailsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<NewsDetailsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных новостей
    /// </summary>
    private readonly NewsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка детальных частей новостей
    /// </summary>
    /// <returns cref="List{NewsDetail}">Список детальных частей новостей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<NewsDetail>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListNewsDetailsMethod);

            //Получение данных из бд
            List<NewsDetail> data = await _context.NewsDetails.Where(x => x.DateDeleted == null).ToListAsync();

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