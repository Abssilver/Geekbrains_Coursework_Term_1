using System;
using System.Linq;
using FileManager.CommandService.CommandArgs;

namespace FileManager.CommandService.BaseCommands
{
    /// <summary>
    /// Реализация интерфейса выполняемой команды для именованных команд
    /// </summary>
    public abstract class NamedCommand : ICommand<NamedCommandArgs>
    {
        /// <summary>
        /// Имя команды
        /// </summary>
        protected readonly string Name;
        //TODO: Скорее всего константам тут не место
        /// <summary>
        /// Константа для именования левого окна
        /// </summary>
        protected const string LD = "LD";
        /// <summary>
        /// Констана для именования правого окна
        /// </summary>
        protected const string RD = "RD";
        /// <summary>
        /// Краткое описание команды
        /// </summary>
        public abstract string Help { get; }
        protected NamedCommand(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Флаг того, что команда может быть выполнена
        /// </summary>
        /// <param name="args">Аргументы команды</param>
        /// <returns>Значение возможности выполнения</returns>
        public virtual bool IsAvailableToHandle(NamedCommandArgs args)
        {
            var lastUserInput = args.Data.UserInputHistory.LastOrDefault(); 
            return lastUserInput?.Equals(Name, StringComparison.InvariantCultureIgnoreCase) ?? false;
        }
        /// <summary>
        /// Метод для выполнения команды
        /// </summary>
        /// <param name="args">Аргументы команды</param>
        public abstract void Handle(NamedCommandArgs args);
    }
}