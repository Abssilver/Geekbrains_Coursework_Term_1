using FileManager.DrawService;

namespace FileManager.DataModel
{
    /// <summary>
    /// Реализация интерфейса для работы одного окна
    /// </summary>
    public class WindowData : IWindowData
    {
        /// <summary>
        /// Максимальное количество элементов на странице
        /// </summary>
        public int MaxElementsPerPage { get; init; }
        /// <summary>
        /// Тип текущего окна
        /// </summary>
        public ActiveWindow WindowType { get; init; }
        /// <summary>
        /// Изменяемое свойство значения пути директории текущего окна
        /// </summary>
        public ChangeableProperty<string> Path { get; init; } = new ChangeableProperty<string>();
        /// <summary>
        /// Изменяемое свойство значения индекса выбранного элемента текущего окна
        /// </summary>
        public ChangeableProperty<int> SelectedItemIndex { get ; init ; } = new ChangeableProperty<int>();
        /// <summary>
        /// Данные директории текущего окна
        /// </summary>
        public DirectoryData DirectoryData { get ; set ; }
        public WindowData()
        { }
    }
}