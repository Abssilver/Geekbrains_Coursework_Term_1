using System;
using System.Linq;
using FileManager.CommandService.CommandArgs;

namespace FileManager.CommandService.BaseCommands
{
    /// <summary>
    /// Расширение именованной командыдля работы с составным вводом пользователя
    /// </summary>
    public abstract class CompositeCommand : NamedCommand
    {
        /// <summary>
        /// Индекс команды в массиве ввода пользователя
        /// </summary>
        private readonly int _commandInInputIndex;
        /// <summary>
        /// Количество элементов в массиве ввода пользователя
        /// </summary>
        private readonly int _argsInCommand;
        protected CompositeCommand(int indexOfCommandInInput, int argsInCommand, string commandName) : base(commandName)
        {
            //TODO: check index is less argsInCommand
            _commandInInputIndex = indexOfCommandInInput;
            _argsInCommand = argsInCommand;
        }
        public override bool IsAvailableToHandle(NamedCommandArgs args)
        {
            var lastUserInput = args.Data.UserInputHistory.LastOrDefault();
            if (lastUserInput is null) 
                return false;
            var commandArgs = lastUserInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return commandArgs.Length.Equals(_argsInCommand) 
                   && commandArgs[_commandInInputIndex].Equals(Name, StringComparison.InvariantCultureIgnoreCase);
        }
        /// <summary>
        /// Метод для проверки корректности ввода пользователя
        /// </summary>
        /// <param name="isLD">Флаг того, что активное окно LD</param>
        /// <param name="argsIndexToValidate">Значение индекса для проверки</param>
        /// <param name="command">Массив ввода пользователя</param>
        /// <returns>Значение корректности</returns>
        protected bool ValidateInput(out bool isLD, int argsIndexToValidate, string [] command)
        {
            isLD = command[argsIndexToValidate].Equals(LD, StringComparison.InvariantCultureIgnoreCase);
            var isRD = command[argsIndexToValidate].Equals(RD, StringComparison.InvariantCultureIgnoreCase);
            return isLD || isRD;
        }
    }
}