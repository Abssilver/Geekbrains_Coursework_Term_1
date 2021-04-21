using System.IO;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда удаления выделенного элемента
    /// </summary>
    public class DeleteCommand : NamedCommand
    {
        public override string Help { get; } = "Delete";
        public DeleteCommand(): base ("delete")
        { }
        public override bool IsAvailableToHandle(NamedCommandArgs args) =>
            base.IsAvailableToHandle(args) && !args.Data.SelectedWindow.Equals(ActiveWindow.None);
        public override void Handle(NamedCommandArgs args)
        {
            var selectedItemIndex = args.Data.GetSelectedItemIndex(args.Data.SelectedWindow);
            var path = args.Data.GetItemPathBySelectedIndex(args.Data.SelectedWindow, selectedItemIndex);

            if (path is null || selectedItemIndex < 1)
            {
                args.Data.ChangeSystemOutput(string.Format("{0}. {1}",
                    "Unable to handle Delete command",
                    "Wrong selected window / You are trying to delete a root"));
                return;
            }
            
            var itemName = args.Data.GetItemNames(args.Data.SelectedWindow)[selectedItemIndex];

            if (File.Exists(path))
            {
                File.Delete(path);
                
                args.Data.ChangeSystemOutput($"File \"{itemName}\" successfully deleted");
                
                UpdateGraphics(args);
                return;
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true); 
                
                args.Data.ChangeSystemOutput($"Directory \"{itemName}\" successfully deleted");

                UpdateGraphics(args);
            }
        }
        /// <summary>
        /// Метод для обновления отображения окон
        /// </summary>
        /// <param name="args">Аргументы команды</param>
        private void UpdateGraphics(ICommandArgs args)
        {
            if (args.Data.GetSelectedPath(ActiveWindow.Left).Equals(args.Data.GetSelectedPath(ActiveWindow.Right)))
            {
                args.Data.ChangeSelectedIndex(ActiveWindow.Left, 0);
                args.Data.ChangeSelectedIndex(ActiveWindow.Right, 0);
            }
            else
            {
                args.Data.ChangeSelectedIndex(args.Data.SelectedWindow, 0);
            }
        }
    }
}