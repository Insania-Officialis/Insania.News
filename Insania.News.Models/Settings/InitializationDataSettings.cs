namespace Insania.News.Models.Settings;

/// <summary>
/// Модель параметров инициализации данных
/// </summary>
public class InitializationDataSettings
{
    /// <summary>
    /// Признак инициализации структуры
    /// </summary>
    /// <remarks>
    /// Нужен для запуска миграций, при true не происходит инициализация данных
    /// </remarks>
    public bool? InitStructure { get; set; }

    /// <summary>
    /// Путь к файлам скриптов
    /// </summary>
    public string? ScriptsPath { get; set; }

    /// <summary>
    /// Включение в инициализацию таблиц
    /// </summary>
    public InitializationDataSettingsIncludeTables? Tables { get; set; }

    /// <summary>
    /// Включение в инициализацию баз данных
    /// </summary>
    public InitializationDataSettingsIncludeDatabases? Databases { get; set; }
}

/// <summary>
/// Модель параметра включения в инициализацию таблиц
/// </summary>
public class InitializationDataSettingsIncludeTables
{
    /// <summary>
    /// Типы новостей
    /// </summary>
    public bool? NewsTypes { get; set; }

    /// <summary>
    /// Новости
    /// </summary>
    public bool? News { get; set; }

    /// <summary>
    /// Детальные части новостей
    /// </summary>
    public bool? NewsDetails { get; set; }

    /// <summary>
    /// Параметры
    /// </summary>
    public bool? Parameters { get; set; }
}

/// <summary>
/// Модель параметра включения в инициализацию баз данных
/// </summary>
public class InitializationDataSettingsIncludeDatabases
{
    /// <summary>
    /// Политика
    /// </summary>
    public bool? News { get; set; }

    /// <summary>
    /// Логи сервиса политики
    /// </summary>
    public bool? LogsApiNews { get; set; }
}