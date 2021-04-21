using FileManager.DrawService;

namespace FileManager.LoaderService
{
    /// <summary>
    /// Класс данных для сериализации между сессиями
    /// </summary>
    public class PersistenceData
    {
        /// <summary>
        /// Тип активного окна
        /// </summary>
        public ActiveWindow WindowType { get; set; }
        /// <summary>
        /// Путь последней открытой директории
        /// </summary>
        public string DirectoryPath { get; set; }
        /// <summary>
        /// Индекс выбранного элемента
        /// </summary>
        public int SelectedItemIndex { get ; set ; }
    }
}