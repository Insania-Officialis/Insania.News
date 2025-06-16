using Microsoft.Extensions.Logging;

using AutoMapper;

using Insania.Shared.Models.Responses.Base;

using Insania.News.Contracts.BusinessLogic;
using Insania.News.Contracts.DataAccess;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

using NewsEntity = Insania.News.Entities.News;
using InformationMessages = Insania.News.Messages.InformationMessages;

namespace Insania.News.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой новостей
/// </summary>
/// <param cref="ILogger{NewsBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="INewsDAO" name="NewsDAO">Сервис работы с данными новостей</param>
public class NewsBL(ILogger<NewsBL> logger, IMapper mapper, INewsDAO NewsDAO) : INewsBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<NewsBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными новостей
    /// </summary>
    private readonly INewsDAO _NewsDAO = NewsDAO;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка новостей
    /// </summary>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <remarks>Список новостей</remarks>
    /// <exception cref="Exception">Исключение</exception>

    public async Task<BaseResponseList> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListNewsMethod);

            //Получение данных
            List<NewsEntity>? data = await _NewsDAO.GetList();

            //Формирование ответа
            BaseResponseList? response = null;
            if (data == null) response = new(false, null);
            else response = new(true, data?.Select(_mapper.Map<BaseResponseListItem>).ToList());

            //Возврат ответа
            return response;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessages.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}