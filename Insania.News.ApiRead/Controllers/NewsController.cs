using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Messages;
using Insania.Shared.Models.Responses.Base;

using Insania.News.Contracts.BusinessLogic;

namespace Insania.News.ApiRead.Controllers;

/// <summary>
/// Контроллер работы с новостями
/// </summary>
/// <param name="logger">Сервис логгирования</param>
/// <param name="newsService">Сервис работы с бизнес-логикой новостей</param>
[Route("news")]
public class NewsController(ILogger<NewsController> logger, INewsBL newsService) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<NewsController> _logger = logger;

    /// <summary>
    /// Сервис работы с бизнес-логикой новостей
    /// </summary>
    private readonly INewsBL _newsService = newsService;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка новостей
    /// </summary>
    /// <returns cref="OkResult">Список новостей</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            //Получение результата
            BaseResponse? result = await _newsService.GetList();

            //Возврат ответа
            return Ok(result);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {ex}", ErrorMessages.Error, ex);

            //Возврат ошибки
            return BadRequest(new BaseResponseError(ex.Message));
        }
    }
    #endregion
}