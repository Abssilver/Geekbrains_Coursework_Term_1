using System;
using System.IO;

namespace FileManager.LogService
{
    /// <summary>
    /// Класс для ведения записи возникающих исключительных ситуаций в файл
    /// </summary>
    public class AppLogService: ILogService
    {
        /// <summary>
        /// Путь, по которому будет происходить сохранение
        /// </summary>
        private readonly string _logPath;

        public AppLogService()
        {
            _logPath = Path.Combine(Environment.CurrentDirectory, "errors");
        }

        public AppLogService(string savePath)
        {
            _logPath = savePath;
        }
        /// <summary>
        /// Метод для сохранения информации об ошибке
        /// </summary>
        /// <param name="exception">Возникающая ошибка</param>
        /// <param name="outputCallback">Действие по возникновению повторной ошибки</param>
        public void LogError(Exception exception, Action<string> outputCallback)
        {
            
            var date = DateTime.Now.ToLongDateString();
            var type = exception.GetType().ToString();
            var message = exception.Message;

            var directoryPath = Path.Combine(Environment.CurrentDirectory, _logPath);
            var filePath = Path.Combine(directoryPath, string.Concat(type, '_', date, ".txt"));

            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                
                if (!File.Exists(filePath))
                    File.Create(filePath).Dispose();

                using StreamWriter writer = File.AppendText(filePath);
                writer.WriteLine($"Date: {date}");
                writer.WriteLine($"Exception: {type}");
                writer.WriteLine($"Message: {message}");
            }
            catch (Exception logException)
            {
                outputCallback?.Invoke(logException.Message);
            }
        }
    }
}