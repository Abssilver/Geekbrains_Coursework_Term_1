using System;

namespace FileManager.DataModel
{
    /// <summary>
    /// Структура данных настроек приложения
    /// </summary>
    public struct ApplicationSettings
    {
        /// <summary>
        /// Значение ширины окна консоли
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Значение высоты окна консоли
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Значение отступа вложенных окон
        /// </summary>
        public int Margin { get; set; }
        /// <summary>
        /// Значение высоты окна заголовка (пути директории)
        /// </summary>
        public int HeaderHeight { get; set; }
        /// <summary>
        /// Значение высоты окна пользовательского ввода
        /// </summary>
        public int ConsoleHeight { get; set; }
        /// <summary>
        /// Значение высоты окна помощи
        /// </summary>
        public int HelpHeight { get; set; }
        /// <summary>
        /// Значение высоты окна контента с данными директории
        /// </summary>
        public int ContentWindowHeight { get; set; }
        /// <summary>
        /// Максимальное значение для отображения элементов на странице
        /// </summary>
        public int MaxElementsPerPage { get; set; }
        /// <summary>
        /// Путь к директории по умолчанию
        /// </summary>
        public string DefaultPath { get; set; }
        /// <summary>
        /// Индекс выбранного элемента по-умолчанию
        /// </summary>
        public int DefaultSelectedItemIndex { get; set; }
        /// <summary>
        /// Цвет фона выделенного элемента
        /// </summary>
        public ConsoleColor SelectionBackgroundColor { get; set; }
        /// <summary>
        /// Цвет выделенного элемента
        /// </summary>
        public ConsoleColor SelectionForegroundColor { get; set; }
        /// <summary>
        /// Цвет фона по-умолчанию
        /// </summary>
        public ConsoleColor DefaultBackgroundColor { get; set; }
        /// <summary>
        /// Цвет элемента по-умолчанию
        /// </summary>
        public ConsoleColor DefaultForegroundColor { get; set; }
    }
}