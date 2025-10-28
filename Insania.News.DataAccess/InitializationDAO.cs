using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Npgsql;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;

using Insania.News.Database.Contexts;
using Insania.News.Entities;
using Insania.News.Models.Settings;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;
using InformationMessages = Insania.Shared.Messages.InformationMessages;

using NewsEntity = Insania.News.Entities.News;
using ErrorMessagesNews = Insania.News.Messages.ErrorMessages;

namespace Insania.News.DataAccess;

/// <summary>
/// Сервис инициализации данных в бд новостей
/// </summary>
/// <param cref="ILogger{InitializationDAO}" name="logger">Сервис логгирования</param>
/// <param cref="NewsContext" name="newsContext">Контекст базы данных новостей</param>
/// <param cref="LogsApiNewsContext" name="logsApiNewsContext">Контекст базы данных логов сервиса новостей</param>
/// <param cref="IOptions{InitializationDataSettings}" name="settings">Параметры инициализации данных</param>
/// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
/// <param cref="IConfiguration" name="configuration">Конфигурация приложения</param>
public class InitializationDAO(ILogger<InitializationDAO> logger, NewsContext newsContext, LogsApiNewsContext logsApiNewsContext, IOptions<InitializationDataSettings> settings, ITransliterationSL transliteration, IConfiguration configuration) : IInitializationDAO
{
    #region Поля
    /// <summary>
    /// Пользователь, вносящий изменения
    /// </summary>
    private readonly string _username = "initializer";
    #endregion

    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<InitializationDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных новостей
    /// </summary>
    private readonly NewsContext _newsContext = newsContext;

    /// <summary>
    /// Контекст базы данных логов сервиса новостей
    /// </summary>
    private readonly LogsApiNewsContext _logsApiNewsContext = logsApiNewsContext;

    /// <summary>
    /// Параметры инициализации данных
    /// </summary>
    private readonly IOptions<InitializationDataSettings> _settings = settings;

    /// <summary>
    /// Сервис транслитерации
    /// </summary>
    private readonly ITransliterationSL _transliteration = transliteration;

    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    private readonly IConfiguration _configuration = configuration;
    #endregion

    #region Методы
    /// <summary>
    /// Метод инициализации данных
    /// </summary>
    /// <exception cref="Exception">Исключение</exception>
    public async Task Initialize()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredInitializeMethod);

            //Инициализация структуры
            if (_settings.Value.InitStructure == true)
            {
                //Логгирование
                _logger.LogInformation("{text}", InformationMessages.InitializationStructure);

                //Инициализация баз данных в зависимости от параметров
                if (_settings.Value.Databases?.News == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("NewsSever") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_news_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("NewsEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_news_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }
                if (_settings.Value.Databases?.LogsApiNews == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("LogsApiNewsServer") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_logs_api_news_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("LogsApiNewsEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_logs_api_news_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }

                //Выход
                return;
            }

            //Накат миграций
            if (_newsContext.Database.IsRelational()) await _newsContext.Database.MigrateAsync();
            if (_logsApiNewsContext.Database.IsRelational()) await _logsApiNewsContext.Database.MigrateAsync();

            //Проверки
            if (string.IsNullOrWhiteSpace(_settings.Value.ScriptsPath)) throw new Exception(ErrorMessagesShared.EmptyScriptsPath);

            //Инициализация данных в зависимости от параметров
            if (_settings.Value.Tables?.NewsTypes == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _newsContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<NewsType> entities =
                    [
                        new(_transliteration, 1, _username, "Удалённый", DateTime.UtcNow),
                        new(_transliteration, 2, _username, "Системные", null),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_newsContext.NewsTypes.Any(x => x.Id == entity.Id)) await _newsContext.NewsTypes.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _newsContext.SaveChangesAsync();

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_news_types_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _newsContext);
                    }

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.News == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _newsContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "Удалённая", "", "1", DateTime.UtcNow.ToString()],
                        ["2", "Мы возвращаемся", "Спустя долгое время застоя администрация проекта возвращается в работу с новыми идеями и подходами к работе. Следите за новостями и будьте первыми, кто вступит в новый неизвестный мир Инсании.", "2", ""],
                        ["3", "Проработка авторизации", "Мы начали работы над стартовой страницей проекта - авторизацией, с которой вы сможете входить в систему, регистрировать новые аккаунты, восстанавливать пароли и оставлять обращения в техническую поддержку.", "2", ""],
                        ["4", "Начало работы над лэндингом", "Мы начали работать над лэндингом нашего проекта - сайтом, который даст ответы на начальные вопросы о мире. С помощью лэндинга будет осуществляться первоначальный вход новых людей в огромный мир Инсании. На главной странице лэндинга вы можете ознакомиться с расами, нациями, странами и фракциями прежде, чем выбирать персонажа для игры. Также на лэндинге представлена информация о последних новостях проекта и оставлены ссылки администраторов, к которым вы можете обращаться по любым непонятным моментам, связанным с проектом Инсания.", "2", ""],
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_newsContext.News.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            NewsType type = await _newsContext.NewsTypes.FirstOrDefaultAsync(x => x.Id == long.Parse(key[3])) ?? throw new Exception(ErrorMessagesNews.NotFoundNewsType);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[4])) dateDeleted = DateTime.Parse(key[4]);
                            NewsEntity entity = new(long.Parse(key[0]), _username, true, key[1], key[2], type, dateDeleted);

                            //Добавление сущности в бд
                            await _newsContext.News.AddAsync(entity);
                        }
                    }

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_news_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _newsContext);
                    }

                    //Сохранение изменений в бд
                    await _newsContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.NewsDetails == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _newsContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "Удалённая", "", "1", DateTime.UtcNow.ToString()],
                        ["2", "Приветствие", "", "2", ""],
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_newsContext.NewsDetails.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            NewsEntity news = await _newsContext.News.FirstOrDefaultAsync(x => x.Id == long.Parse(key[3])) ?? throw new Exception(ErrorMessagesNews.NotFoundNews);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[4])) dateDeleted = DateTime.Parse(key[4]);
                            NewsDetail entity = new(long.Parse(key[0]), _username, true, key[1], key[2], news, dateDeleted);

                            //Добавление сущности в бд
                            await _newsContext.NewsDetails.AddAsync(entity);
                        }
                    }

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_news_details_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _newsContext);
                    }

                    //Сохранение изменений в бд
                    await _newsContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Parameters == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _newsContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<ParameterNews> entities =
                    [
                        new(_transliteration, 10000, _username, "Удалённый", dateDeleted: DateTime.UtcNow),
                        new(_transliteration, 1, _username, "Ссылка на ВК", "https://vk.com/insania_officialis"),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_newsContext.Parameters.Any(x => x.Id == entity.Id)) await _newsContext.Parameters.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _newsContext.SaveChangesAsync();

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_parameters_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _newsContext);
                    }

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Метод создание базы данных
    /// </summary>
    /// <param name="connectionServer">Строка подключения к серверу</param>
    /// <param name="patternDatabases">Шаблон файлов создания базы данных</param>
    /// <param name="connectionDatabase">Строка подключения к базе данных</param>
    /// <param name="patternSchemes">Шаблон файлов создания схемы</param>
    /// <returns></returns>
    private async Task CreateDatabase(string connectionServer, string patternDatabases, string connectionDatabase, string patternSchemes)
    {
        //Проход по всем скриптам в директории и создание баз данных
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternDatabases)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionServer);
        }

        //Проход по всем скриптам в директории и создание схем
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternSchemes)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionDatabase);
        }
    }

    /// <summary>
    /// Метод выполнения скрипта со строкой подключения
    /// </summary>
    /// <param cref="string" name="filePath">Путь к скрипту</param>
    /// <param cref="string" name="connectionString">Строка подключения</param>
    private async Task ExecuteScript(string filePath, string connectionString)
    {
        //Логгирование
        _logger.LogInformation("{text} {params}", InformationMessages.ExecuteScript, filePath);

        try
        {
            //Создание соединения к бд
            using NpgsqlConnection connection = new(connectionString);

            //Открытие соединения
            connection.Open();

            //Считывание запроса
            string sql = File.ReadAllText(filePath);

            //Создание sql-запроса
            using NpgsqlCommand command = new(sql, connection);

            //Выполнение команды
            await command.ExecuteNonQueryAsync();

            //Логгирование
            _logger.LogInformation("{text} {params}", InformationMessages.ExecutedScript, filePath);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessagesShared.NotExecutedScript, filePath, ex);
        }
    }

    /// <summary>
    /// Метод выполнения скрипта с контекстом
    /// </summary>
    /// <param cref="string" name="filePath">Путь к скрипту</param>
    /// <param cref="DbContext" name="context">Контекст базы данных</param>
    private async Task ExecuteScript(string filePath, DbContext context)
    {
        //Логгирование
        _logger.LogInformation("{text} {params}", InformationMessages.ExecuteScript, filePath);

        try
        {
            //Считывание запроса
            string sql = File.ReadAllText(filePath);

            //Выполнение sql-команды
            await context.Database.ExecuteSqlRawAsync(sql);

            //Логгирование
            _logger.LogInformation("{text} {params}", InformationMessages.ExecutedScript, filePath);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessagesShared.NotExecutedScript, filePath, ex);
        }
    }
    #endregion
}