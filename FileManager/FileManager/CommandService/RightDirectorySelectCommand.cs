using System;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда выбора правого окна
    /// </summary>
    public class RightDirectorySelectCommand : KeyCommand
    {
        public override string Help { get; } = "->";
        public RightDirectorySelectCommand() : base(ConsoleKey.RightArrow) 
        { }
        public override void Handle(KeyCommandArgs args)
        {
            if (args.Data.SelectedWindow.Equals(ActiveWindow.Right))
            {
                //TODO: Здесь RD вписан вручную, не надо так
                args.Data.ChangeSystemOutput("RD is already selected");
                return;
            }
            args.Data.ChangeSelectedWindow(ActiveWindow.Right);
            //TODO: Здесь RD вписан вручную, не надо так
            args.Data.ChangeSystemOutput("RD selected. Use arrow keys to select an item.");
        }
    }
}