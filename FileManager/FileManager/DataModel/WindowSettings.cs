using System;

namespace FileManager.DataModel
{
    /// <summary>
    /// Класс реализующий настройки окна консоли
    /// </summary>
    public class WindowSettings
    {
        /// <summary>
        /// Настройки по-умолчанию
        /// </summary>
        private readonly ApplicationSettings _defaultSettings = new()
        {
            Width = 160,
            Height = 40,
            Margin = 5,
            HeaderHeight = 3,
            ConsoleHeight = 4,
            HelpHeight = 3,
            ContentWindowHeight = 28,
            MaxElementsPerPage = 1,
            DefaultPath = Environment.CurrentDirectory,
            DefaultSelectedItemIndex = 0,
            SelectionBackgroundColor = ConsoleColor.White,
            SelectionForegroundColor = ConsoleColor.Black,
            DefaultBackgroundColor = ConsoleColor.Black,
            DefaultForegroundColor = ConsoleColor.White,
        };

        /// <summary>
        /// Минимальные настройки приложения
        /// </summary>
        private readonly ApplicationSettings _minSettings = new()
        {
            Width = 50,
            Height = 15,
            Margin = 2,
            HeaderHeight = 3,
            ConsoleHeight = 4,
            HelpHeight = 3,
            ContentWindowHeight = 3,
            MaxElementsPerPage = 1,
            DefaultPath = Environment.CurrentDirectory,
            DefaultSelectedItemIndex = 0,
            SelectionBackgroundColor = ConsoleColor.White,
            SelectionForegroundColor = ConsoleColor.Black,
            DefaultBackgroundColor = ConsoleColor.Black,
            DefaultForegroundColor = ConsoleColor.White,
        }; 
        
        /// <summary>
        /// Текущие настройки приложения
        /// </summary>
        public readonly ApplicationSettings AppSettings;
        public WindowSettings(ApplicationSettings appSettings)
        {
            AppSettings = ValidateSettings(appSettings);

            SetWindowSize();
        }

        /// <summary>
        /// Устанавливает настройки приложения с соответствии с минимальными и допустимыми настройками
        /// </summary>
        /// <param name="current">Текущие настройки</param>
        /// <returns>Скорректированное значение настроек приложения</returns>
        private ApplicationSettings ValidateSettings(ApplicationSettings current)
        {
            var widthCheck = current.Width >= _minSettings.Width && current.Width < Console.LargestWindowWidth;
            var heightCheck = current.Height >= _minSettings.Height && current.Height < Console.LargestWindowHeight;
            
            if (!widthCheck || !heightCheck)
                return _defaultSettings;

            var headerCheck = current.HeaderHeight >= _minSettings.HeaderHeight;
            var consoleCheck = current.ConsoleHeight >= _minSettings.ConsoleHeight;
            var helpCheck = current.HelpHeight >= _minSettings.HelpHeight;
            var contentCheck = current.ContentWindowHeight == 
                               current.Height - current.HeaderHeight - current.ConsoleHeight - current.HelpHeight - 2;
            
            if (!headerCheck || !consoleCheck || !helpCheck || !contentCheck)
                return _defaultSettings;
            
            var marginCheck = current.Margin >= _minSettings.Margin;
            var maxElementCheck = current.MaxElementsPerPage <= current.ContentWindowHeight - 2;
            var pathCheck = !string.IsNullOrEmpty(current.DefaultPath);

            if (!marginCheck || !maxElementCheck || !pathCheck)
                return _defaultSettings;

            var bgCheck = !current.SelectionBackgroundColor.Equals(current.DefaultBackgroundColor);
            var foregroundCheck = !current.SelectionForegroundColor.Equals(current.DefaultForegroundColor);

            if (!bgCheck || !foregroundCheck)
                return _defaultSettings;
            
            return current;
        }
        /// <summary>
        /// Метод установки размера консоли
        /// </summary>
        private void SetWindowSize() => Console.SetWindowSize(AppSettings.Width, AppSettings.Height);
    }
}
