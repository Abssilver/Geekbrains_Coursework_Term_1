using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда снятия выделения активного окна
    /// </summary>
    public class UnselectCommand : NamedCommand
    {
        public override string Help { get; } = "Unselect";
        public UnselectCommand(): base ("unselect")
        { }
        public override bool IsAvailableToHandle(NamedCommandArgs args) =>
            base.IsAvailableToHandle(args) && !args.Data.SelectedWindow.Equals(ActiveWindow.None);
        public override void Handle(NamedCommandArgs args)
        {
            var isLD = args.Data.SelectedWindow.Equals(ActiveWindow.Left);
            args.Data.ChangeSelectedWindow(ActiveWindow.None);
            args.Data.ChangeSystemOutput($"{(isLD ? LD : RD)} unselected");
        }
    }
}