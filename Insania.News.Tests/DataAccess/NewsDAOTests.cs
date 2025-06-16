using Microsoft.Extensions.DependencyInjection;

using Insania.News.Contracts.DataAccess;
using Insania.News.Tests.Base;

using NewsEntity = Insania.News.Entities.News;

namespace Insania.News.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными новостей
/// </summary>
[TestFixture]
public class NewsDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными новостей
    /// </summary>
    private INewsDAO NewsDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        NewsDAO = ServiceProvider.GetRequiredService<INewsDAO>();
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
    /// Тест метода получения списка новостей
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<NewsEntity>? result = await NewsDAO.GetList();

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}