using System;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда передачи следующего ввода пользователя
    /// </summary>
    public class NextUserInputGiverCommand : KeyCommand
    {
        public override string Help { get; } = "↓";
        public NextUserInputGiverCommand() : base(ConsoleKey.DownArrow) 
        { }
        public override bool IsAvailableToHandle(KeyCommandArgs args) => 
            base.IsAvailableToHandle(args) && args.Data.SelectedWindow.Equals(ActiveWindow.None);
        public override void Handle(KeyCommandArgs args)
        {
            var nextUserInput = args.Data.GetNextUserCommand();
            if (nextUserInput is null)
            {
                args.Data.ChangeSystemOutput("No user history.");
                return;
            }
            args.Data.ChangeUserInput(nextUserInput);
        }
    }
}