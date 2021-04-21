using FileManager.CommandService.CommandArgs;

namespace FileManager.CommandService.BaseCommands
{
    /// <summary>
    /// Интерфейс выполняемой команды
    /// </summary>
    /// <typeparam name="T">Тип аргументов команды</typeparam>
    public interface ICommand<in T> where T : ICommandArgs
    {
        string Help { get; }
        bool IsAvailableToHandle(T args);
        void Handle(T args);
    }
}