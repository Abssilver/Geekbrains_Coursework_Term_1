using System;
using System.Linq;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда для выбора активного окна
    /// </summary>
    public class SelectCommand : CompositeCommand
    {
        public override string Help { get; } = $"Select {LD}\\{RD}";
        public SelectCommand() : base(indexOfCommandInInput: 0, argsInCommand: 2, commandName: "select")
        { }
        public override void Handle(NamedCommandArgs args)
        {
            string[] command = args.Data.UserInputHistory.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (!ValidateInput(out var isLD, 1, command))
            {
                args.Data.ChangeSystemOutput($"Invalid command! Try \"select {LD}\" or \"select {RD}\" instead");
                return;
            }
            args.Data.ChangeSelectedWindow(isLD ? ActiveWindow.Left : ActiveWindow.Right);
            var directory = isLD ? LD : RD;
            args.Data.ChangeSystemOutput($"{directory} selected. Use arrow keys to select an item.");
        }
    }
}