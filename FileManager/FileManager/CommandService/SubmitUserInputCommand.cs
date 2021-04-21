using System;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда отправки пользователем команды (завершение ввода)
    /// </summary>
    public class SubmitUserInputCommand : KeyCommand
    {
        public override string Help { get; } = "ENTER";
        public SubmitUserInputCommand() : base(ConsoleKey.Enter) 
        { }
        public override void Handle(KeyCommandArgs args)
        {
            if (string.IsNullOrEmpty(args.Data.UserInput.Value))
            {
                args.Data.ChangeSystemOutput("Error! Unable to process an empty input");
                return;
            }
            args.Data.SubmitUserInput();
            args.Data.ChangeUserInput(string.Empty);
        }
    }
}