using AutoMapper;

using Insania.Shared.Models.Responses.Base;

using NewsEntity = Insania.News.Entities.News;

namespace Insania.News.Models.Mapper;

/// <summary>
/// Сервис преобразования моделей
/// </summary>
public class NewsMappingProfile : Profile
{
    /// <summary>
    /// Конструктор сервиса преобразования моделей
    /// </summary>
    public NewsMappingProfile()
    {
        //Преобразование модели сущности новости в базовую модель элемента ответа списком
        CreateMap<NewsEntity, BaseResponseListItem>().ConstructUsing(x => new BaseResponseListItem(x.Id, x.Title, x.Description));
    }
}