using System;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда очистки ввода пользователя
    /// </summary>
    public class ClearUserInputCommand : KeyCommand
    {
        public override string Help { get; } = "BACKSPACE";
        public ClearUserInputCommand() : base(ConsoleKey.Backspace) 
        { }
        public override void Handle(KeyCommandArgs args)
        {
            var userInput = args.Data.UserInput.Value;
            
            if (string.IsNullOrEmpty(userInput))
            {
                args.Data.ChangeSystemOutput("The input is already cleared!");
                return;
            }

            var newLength = userInput.Length - 1;
            userInput = newLength > 0 ? userInput.Substring(0, newLength) : string.Empty;

            args.Data.ChangeUserInput(userInput);
        }
    }
}