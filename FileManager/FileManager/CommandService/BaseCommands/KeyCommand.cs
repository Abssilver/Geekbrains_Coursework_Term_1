using System;
using FileManager.CommandService.CommandArgs;

namespace FileManager.CommandService.BaseCommands
{
    /// <summary>
    /// Реализация интерфейса выполняемой команды для кнопочных команд
    /// </summary>
    public abstract class KeyCommand: ICommand<KeyCommandArgs>
    {
        /// <summary>
        /// Значение кнопки, соответствующеее команде
        /// </summary>
        public ConsoleKey CommandKey { get; }
        /// <summary>
        /// Краткое описание команды
        /// </summary>
        public abstract string Help { get; }
        protected KeyCommand(ConsoleKey commandKey)
        {
            CommandKey = commandKey; 
        }
        /// <summary>
        /// Флаг того, что команда может быть выполнена
        /// </summary>
        /// <param name="args">Аргументы команды</param>
        /// <returns>Значение возможности выполнения</returns>
        public virtual bool IsAvailableToHandle(KeyCommandArgs args) => args.KeyInfo.Key.Equals(CommandKey);
        /// <summary>
        /// Метод для выполнения команды
        /// </summary>
        /// <param name="args">Аргументы команды</param>
        public abstract void Handle(KeyCommandArgs args);
    }
}