using FileManager.DataModel;

namespace FileManager.CommandService.CommandArgs
{
    /// <summary>
    /// Реализация интерфейса аргументов комманды для именованных команд
    /// </summary>
    public class NamedCommandArgs : ICommandArgs
    {
        /// <summary>
        /// Данные приложения для коммуникации между сервисами
        /// </summary>
        public ApplicationData Data { get; }
        public NamedCommandArgs(ApplicationData data)
        {
            Data = data;
        }
    }
}