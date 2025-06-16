using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.News.Contracts.DataAccess;
using Insania.News.Database.Contexts;
using Insania.News.Entities;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

using InformationMessages = Insania.News.Messages.InformationMessages;

namespace Insania.News.DataAccess;

/// <summary>
/// Сервис работы с данными типов новостей
/// </summary>
/// <param cref="ILogger{NewsTypesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="NewsContext" name="context">Контекст базы данных новостей</param>
public class NewsTypesDAO(ILogger<NewsTypesDAO> logger, NewsContext context) : INewsTypesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<NewsTypesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных новостей
    /// </summary>
    private readonly NewsContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка типов новостей
    /// </summary>
    /// <returns cref="List{NewsType}">Список типов новостей</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<NewsType>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListNewsTypesMethod);

            //Получение данных из бд
            List<NewsType> data = await _context.NewsTypes.Where(x => x.DateDeleted == null).ToListAsync();

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