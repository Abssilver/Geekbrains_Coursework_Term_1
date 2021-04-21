using FileManager.DrawService;

namespace FileManager.DataModel
{
    /// <summary>
    /// Интерфейс данных для работы одного окна
    /// </summary>
    public interface IWindowData
    {
        int MaxElementsPerPage { get; }
        ActiveWindow WindowType { get; }
        ChangeableProperty<string> Path { get; }
        ChangeableProperty<int> SelectedItemIndex { get; }
        DirectoryData DirectoryData { get; set; }
    }
}