using System;
using System.Collections.Generic;
using System.Linq;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DataModel;
using FileManager.LogService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Сервис для работы с командами
    /// </summary>
    internal class AppCommandService
    {
        /// <summary>
        /// Список именованных команд
        /// </summary>
        private readonly List<ICommand<NamedCommandArgs>> _commands;
        /// <summary>
        /// Список кнопочных команд
        /// </summary>
        private readonly List<ICommand<KeyCommandArgs>> _keyCommands;
        /// <summary>
        /// Массив с данными об описании команд
        /// </summary>
        public string[] CommandsHelp { get; }
        /// <summary>
        /// Сет из уникальных значений кнопок, которыми вызываются кнопочные команды
        /// </summary>
        public HashSet<ConsoleKey> CommandKeys { get; }
        /// <summary>
        /// Ссылка на сервис логгирования
        /// </summary>
        private readonly ILogService _logService;
        public AppCommandService(ILogService logService)
        {
            _commands = new List<ICommand<NamedCommandArgs>>()
            {
                new SelectCommand(),
                new UnselectCommand(),
                new OpenCommand(),
                new CopyCommand(),
                new DeleteCommand(),
                new ExitCommand(),
                new PreviousPageCommand(),
                new NextPageCommand(),
            };

            _keyCommands = new List<ICommand<KeyCommandArgs>>()
            {
                new IncreaseSelectionIndexCommand(),
                new DecreaseSelectionIndexCommand(),
                new NextUserInputGiverCommand(),
                new PreviousUserInputGiverCommand(),
                new SubmitUserInputCommand(),
                new ClearUserInputCommand(),
                new LeftDirectorySelectCommand(),
                new RightDirectorySelectCommand(),
            };

            CommandsHelp = _commands.Select(command => command.Help)
                .Concat(_keyCommands.Select(command => command.Help)).ToArray();

            CommandKeys = _keyCommands.Select(command => ((KeyCommand)command).CommandKey).Distinct().ToHashSet();

            _logService = logService;
        }
        /// <summary>
        /// Метод обработки кнопочных команд
        /// </summary>
        /// <param name="keyInfo">Значение нажатой кнопки</param>
        /// <param name="data">Данные приложения</param>
        public void ProcessKeyCommand(ConsoleKeyInfo keyInfo, ApplicationData data) =>
            CommandProcessor(_keyCommands, new KeyCommandArgs(keyInfo, data));

        /// <summary>
        /// Метод обработки именованных команд
        /// </summary>
        /// <param name="data">Данные приложения</param>
        public void ProcessCommand(ApplicationData data) =>
            CommandProcessor(_commands, new NamedCommandArgs(data),
                () => data.ChangeSystemOutput($"No such command \'{data.UserInputHistory.Last()}\' to process"));
        
        /// <summary>
        /// Базовый метод для обработки команд
        /// </summary>
        /// <param name="commandList">Список команд для работы</param>
        /// <param name="args">Аргументы команды</param>
        /// <param name="failCallback">Действие, выполняемое при отсутствии команд для выполнения</param>
        /// <typeparam name="T">Тип аргументов команды</typeparam>
        private void CommandProcessor<T>(List<ICommand<T>> commandList, T args, Action failCallback = null)
            where T : ICommandArgs
        {
            var command = commandList.FirstOrDefault(cmd => cmd.IsAvailableToHandle(args));
            if (command is null)
                failCallback?.Invoke();
            else
            {
                try
                {
                    command.Handle(args);
                }
                catch (Exception exception)
                {
                    args.Data.ChangeSystemOutput("Error during command execution, check log file for more information");
                    _logService.LogError(exception, args.Data.ChangeSystemOutput);
                }
            }
        }
    }
}
