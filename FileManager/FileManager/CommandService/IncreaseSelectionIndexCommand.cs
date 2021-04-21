using System;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда выбора следующего элемента
    /// </summary>
    public class IncreaseSelectionIndexCommand : KeyCommand
    {
        public override string Help { get; } = "Select + ↓";
        public IncreaseSelectionIndexCommand() : base(ConsoleKey.DownArrow) 
        { }
        public override bool IsAvailableToHandle(KeyCommandArgs args) => 
            base.IsAvailableToHandle(args) && !args.Data.SelectedWindow.Equals(ActiveWindow.None);
        public override void Handle(KeyCommandArgs args)
        {
            var index = args.Data.GetSelectedItemIndex(args.Data.SelectedWindow);
            if (index.Equals(-1))
            {
                args.Data.ChangeSystemOutput("Unable to increase index. No selected window");
                return;
            }
            args.Data.ChangeSelectedIndex(args.Data.SelectedWindow,index + 1);
        }
    }
}