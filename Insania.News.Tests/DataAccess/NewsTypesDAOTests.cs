using Microsoft.Extensions.DependencyInjection;

using Insania.News.Contracts.DataAccess;
using Insania.News.Entities;
using Insania.News.Tests.Base;

namespace Insania.News.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными типов новостей
/// </summary>
[TestFixture]
public class NewsTypesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными типов новостей
    /// </summary>
    private INewsTypesDAO NewsTypesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        NewsTypesDAO = ServiceProvider.GetRequiredService<INewsTypesDAO>();
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
    /// Тест метода получения списка типов новостей
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<NewsType>? result = await NewsTypesDAO.GetList();

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