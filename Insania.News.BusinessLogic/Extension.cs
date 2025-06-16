using Microsoft.Extensions.DependencyInjection;

using Insania.News.Contracts.BusinessLogic;
using Insania.News.DataAccess;

namespace Insania.News.BusinessLogic;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с бизнес-логикой в зоне новостей
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с бизнес-логикой в зоне новостей
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddNewsBL(this IServiceCollection services) =>
        services
            .AddNewsDAO() //сервисы работы с данными в зоне новостей
            .AddScoped<INewsBL, NewsBL>() //сервис работы с бизнес-логикой новостей
        ;
}