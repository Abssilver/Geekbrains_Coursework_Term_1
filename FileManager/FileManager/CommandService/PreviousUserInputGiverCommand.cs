using System;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда передачи предыдущего ввода пользователя
    /// </summary>
    public class PreviousUserInputGiverCommand : KeyCommand
    {
        public override string Help { get; } = "↑";
        public PreviousUserInputGiverCommand() : base(ConsoleKey.UpArrow) 
        { }
        public override bool IsAvailableToHandle(KeyCommandArgs args) => 
            base.IsAvailableToHandle(args) && args.Data.SelectedWindow.Equals(ActiveWindow.None);
        public override void Handle(KeyCommandArgs args)
        {
            var previousUserInput = args.Data.GetPreviousUserCommand();
            if (previousUserInput is null)
            {
                args.Data.ChangeSystemOutput("No user history.");
                return;
            }
            args.Data.ChangeUserInput(previousUserInput);
        }
    }
}