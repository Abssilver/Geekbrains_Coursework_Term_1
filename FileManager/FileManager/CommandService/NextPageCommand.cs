using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда перехода на следующую страницу
    /// </summary>
    public class NextPageCommand : NamedCommand
    {
        public override string Help { get; } = "Next";
        public NextPageCommand(): base ("next")
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
                    "Unable to handle Next Page command",
                    "Wrong selected window"));
                return;
            }

            var maxIndex = args.Data.GetDirectoryData(args.Data.SelectedWindow).Count - 1;
            if (selectedItemIndex.Equals(maxIndex))
            {
                args.Data.ChangeSystemOutput(string.Format("{0}. {1}",
                    "Unable to handle Next Page command",
                    "You are already on the last page"));
                return;
            }

            var newIndex = selectedItemIndex + maxElements;
            if (newIndex > maxIndex)
                newIndex = maxIndex;
            
            args.Data.ChangeSelectedIndex(args.Data.SelectedWindow, newIndex);
        }
    }
}