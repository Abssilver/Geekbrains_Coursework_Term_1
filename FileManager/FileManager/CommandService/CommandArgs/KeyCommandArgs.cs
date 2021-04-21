using System;
using FileManager.DataModel;

namespace FileManager.CommandService.CommandArgs
{
    /// <summary>
    /// Реализация интерфейса аргументов комманды для кнопочных команд
    /// </summary>
    public class KeyCommandArgs : NamedCommandArgs
    {
        /// <summary>
        /// Значение кнопки, соответствующее команде
        /// </summary>
        public ConsoleKeyInfo KeyInfo { get; }
        public KeyCommandArgs(ConsoleKeyInfo keyInfo, ApplicationData data) : base(data)
        {
            KeyInfo = keyInfo;
        }
    }
}