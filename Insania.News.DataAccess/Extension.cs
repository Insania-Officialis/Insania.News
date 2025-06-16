using Microsoft.Extensions.DependencyInjection;

using Insania.News.Contracts.DataAccess;

namespace Insania.News.DataAccess;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с данными в зоне новостей
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с данными в зоне новостей
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddNewsDAO(this IServiceCollection services) =>
        services
            .AddScoped<INewsTypesDAO, NewsTypesDAO>() //сервис работы с данными типов новостей
            .AddScoped<INewsDAO, NewsDAO>() //сервис работы с данными новостей
            .AddScoped<INewsDetailsDAO, NewsDetailsDAO>() //сервис работы с данными детальных частей новостей
        ;
}