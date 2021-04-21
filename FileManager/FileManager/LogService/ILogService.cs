using System;

namespace FileManager.LogService
{
    /// <summary>
    /// Интерфейс сервиса логгирования
    /// </summary>
    public interface ILogService
    {
        void LogError(Exception exception, Action<string> outputCallback);
    }
}