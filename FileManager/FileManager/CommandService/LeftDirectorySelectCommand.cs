using System;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда выбора левого окна
    /// </summary>
    public class LeftDirectorySelectCommand : KeyCommand
    {
        public override string Help { get; } = "<-";
        public LeftDirectorySelectCommand() : base(ConsoleKey.LeftArrow) 
        { }
        public override void Handle(KeyCommandArgs args)
        {
            if (args.Data.SelectedWindow.Equals(ActiveWindow.Left))
            {
                //TODO: Здесь LD вписан вручную, не надо так
                args.Data.ChangeSystemOutput("LD is already selected");
                return;
            }
            args.Data.ChangeSelectedWindow(ActiveWindow.Left);
            //TODO: Здесь LD вписан вручную, не надо так
            args.Data.ChangeSystemOutput("LD selected. Use arrow keys to select an item.");
        }
    }
}