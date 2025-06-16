using Microsoft.Extensions.DependencyInjection;

using Insania.News.Contracts.DataAccess;
using Insania.News.Entities;
using Insania.News.Tests.Base;

namespace Insania.News.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными детальных частей новостей
/// </summary>
[TestFixture]
public class NewsDetailsDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными детальных частей новостей
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
    /// Тест метода получения списка детальных частей новостей
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<NewsDetail>? result = await NewsDetailsDAO.GetList();

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