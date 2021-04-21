using FileManager.DataModel;

namespace FileManager.CommandService.CommandArgs
{
    /// <summary>
    /// Интерфейс аргументов комманды
    /// </summary>
    public interface ICommandArgs
    {
        ApplicationData Data { get; }
    }
}