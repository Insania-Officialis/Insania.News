using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Contracts.DataAccess;

using Insania.News.Contracts.DataAccess;
using Insania.News.Entities;
using Insania.News.Tests.Base;

using NewsEntity = Insania.News.Entities.News;

namespace Insania.News.Tests.DataAccess;

/// <summary>
/// Тесты сервиса инициализации данных в бд новостей
/// </summary>
[TestFixture]
public class InitializationDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис инициализации данных в бд новостей
    /// </summary>
    private IInitializationDAO InitializationDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными типов организаций
    /// </summary>
    private INewsTypesDAO NewsTypesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными организаций
    /// </summary>
    private INewsDAO NewsDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными стран
    /// </summary>
    private INewsDetailsDAO NewsDetailsDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        InitializationDAO = ServiceProvider.GetRequiredService<IInitializationDAO>();
        NewsTypesDAO = ServiceProvider.GetRequiredService<INewsTypesDAO>();
        NewsDAO = ServiceProvider.GetRequiredService<INewsDAO>();
        NewsDetailsDAO = ServiceProvider.GetRequiredService<INewsDetailsDAO>();
    }

    /// <summary>
    /// Метод, вызываемый после тестов
    /// </summary>
    [TearDown]
    public void TearDown()
    {

    }
    #endregion

    #region Методы тестирования
    /// <summary>
    /// Тест метода инициализации данных
    /// </summary>
    [Test]
    public async Task InitializeTest()
    {
        try
        {
            //Выполнение метода
            await InitializationDAO.Initialize();

            //Получение сущностей
            List<NewsType> newsTypes = await NewsTypesDAO.GetList();
            List<NewsEntity> news = await NewsDAO.GetList();
            List<NewsDetail> newsDetails = await NewsDetailsDAO.GetList();

            //Проверка результата
            Assert.Multiple(() =>
            {
                Assert.That(newsTypes, Is.Not.Empty);
                Assert.That(news, Is.Not.Empty);
                Assert.That(newsDetails, Is.Not.Empty);
            });
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}