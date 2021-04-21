using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда перехода из директории в выбранную
    /// </summary>
    public class OpenCommand : NamedCommand
    {
        public override string Help { get; } = "Open";
        public OpenCommand(): base ("open")
        { }
        public override bool IsAvailableToHandle(NamedCommandArgs args) =>
            base.IsAvailableToHandle(args) && !args.Data.SelectedWindow.Equals(ActiveWindow.None);
        public override void Handle(NamedCommandArgs args)
        {
            var selectedItemIndex = args.Data.GetSelectedItemIndex(args.Data.SelectedWindow);
            var path = args.Data.GetItemPathBySelectedIndex(args.Data.SelectedWindow, selectedItemIndex);
            if (path is null)
            {
                args.Data.ChangeSystemOutput(string.Format("{0}. {1}",
                    "Unable to handle Open command",
                    "Wrong selected window / You try to open a null path item"));
                return;
            }
            args.Data.ChangePath(args.Data.SelectedWindow, path);
        }
    }
}