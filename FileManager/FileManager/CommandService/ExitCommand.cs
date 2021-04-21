using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда окончания работы приложения
    /// </summary>
    public class ExitCommand : NamedCommand
    {
        public override string Help { get; } = "Exit";
        public ExitCommand(): base ("exit")
        { }
        public override void Handle(NamedCommandArgs args)
        { 
            args.Data.ChangeSystemOutput("Application is finished");
        } 
    }
}