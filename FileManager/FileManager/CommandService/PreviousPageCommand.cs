using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда перехода на предыдущую страницу
    /// </summary>
    public class PreviousPageCommand : NamedCommand
    {
        public override string Help { get; } = "Prev";
        public PreviousPageCommand(): base ("prev")
        { }
        public override bool IsAvailableToHandle(NamedCommandArgs args) =>
            base.IsAvailableToHandle(args) && !args.Data.SelectedWindow.Equals(ActiveWindow.None);
        public override void Handle(NamedCommandArgs args)
        {
            var selectedItemIndex = args.Data.GetSelectedItemIndex(args.Data.SelectedWindow);
            var maxElements = args.Data.GetMaxElementsPerPage(args.Data.SelectedWindow);
            if (maxElements < 0 || selectedItemIndex< 0)
            {
                args.Data.ChangeSystemOutput(string.Format("{0}. {1}",
                    "Unable to handle Previous Page command",
                    "Wrong selected window"));
                return;
            }

            if (selectedItemIndex.Equals(0))
            {
                args.Data.ChangeSystemOutput(string.Format("{0}. {1}",
                    "Unable to handle Previous Page command",
                    "You are already on the first page"));
                return;
            }

            var newIndex = selectedItemIndex - maxElements;
            if (newIndex < 0)
                newIndex = 0;
            
            args.Data.ChangeSelectedIndex(args.Data.SelectedWindow, newIndex);
        }
    }
}