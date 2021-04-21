using System;
using System.IO;
using System.Linq;
using FileManager.CommandService;
using FileManager.DataModel;
using FileManager.DrawService;
using FileManager.LoaderService;
using FileManager.LogService;

namespace FileManager
{
    static class FileService
    {
        static void Main(string[] args)
        {
            int.TryParse(Properties.Resources.WindowWidth, out var width);
            int.TryParse(Properties.Resources.WindowHeight, out var height);
            int.TryParse(Properties.Resources.WindowMargin, out var margin);
            int.TryParse(Properties.Resources.MaxElementsPerPage, out var maxElementsPerPage);
            int.TryParse(Properties.Resources.DefaultSelectedIndex, out var defaultSelectedIndex);
            int.TryParse(Properties.Resources.HeaderHeight, out var headerHeight);
            int.TryParse(Properties.Resources.ConsoleHeight, out var consoleHeight);
            int.TryParse(Properties.Resources.HelpHeight, out var helpHeight);
            int.TryParse(Properties.Resources.ContentWindowHeight, out var contentWindowHeight);


            ApplicationSettings appSettings = new()
            {
                Width = width,
                Height = height,
                Margin = margin,
                HeaderHeight = headerHeight,
                ConsoleHeight = consoleHeight,
                HelpHeight = helpHeight,
                ContentWindowHeight = contentWindowHeight,
                MaxElementsPerPage = maxElementsPerPage,
                DefaultPath = Environment.CurrentDirectory,
                DefaultSelectedItemIndex = defaultSelectedIndex,
                SelectionBackgroundColor = ConsoleColor.Gray,
                SelectionForegroundColor = ConsoleColor.Black,
                DefaultBackgroundColor = ConsoleColor.Black,
                DefaultForegroundColor = ConsoleColor.White,
            }; 
            
            var windowSettings = new WindowSettings(appSettings);

            var lastSessionData = Properties.Resources.SavePath;
            var savedDataPath = Path.Combine(Environment.CurrentDirectory, lastSessionData);
            
            var logService = new AppLogService();
            var applicationDataLoader = new ApplicationDataLoader(logService, savedDataPath, appSettings);
            var applicationData = applicationDataLoader.ApplicationData;
            var drawService = new AppDrawService(windowSettings, applicationData);
            var commandService = new AppCommandService(logService);
            DrawManager(drawService, commandService, applicationData);
            ProcessUserInput(commandService, applicationData);
        }

        /// <summary>
        /// Первичная отрисовка приложения
        /// </summary>
        /// <param name="appDrawService">Сервис отрисовки</param>
        /// <param name="appCommandService">Сервис обработки команд</param>
        /// <param name="applicationData">Данные приложения для межсервисного общения</param>
        private static void DrawManager(AppDrawService appDrawService, 
            AppCommandService appCommandService, ApplicationData applicationData)
        {
            Console.Clear();
            appDrawService.DrawOuterFrame("Ras4eska Pashi");
            appDrawService.DrawSelectedHeader(ActiveWindow.Left);
            appDrawService.DrawSelectedMain(ActiveWindow.Left);
            appDrawService.DrawSelectedHeader(ActiveWindow.Right);
            appDrawService.DrawSelectedMain(ActiveWindow.Right);
            appDrawService.DrawHelp(appCommandService.CommandsHelp);
            appDrawService.DrawUserInterface(applicationData.SystemOutput.Value,
                applicationData.UserInput.Value);

            applicationData.SystemOutput.OnValueChanged += value => 
                appDrawService.DrawUserInterface(value, applicationData.UserInput.Value);
            
            applicationData.UserInput.OnValueChanged += value => 
                appDrawService.DrawUserInterface(applicationData.SystemOutput.Value, value);

            applicationData.LDSelectedItemIndexChanged += _ =>
            {
                appDrawService.DrawSelectedMain(ActiveWindow.Left);
                appDrawService.SetCursorPositionForUserInput();
            };

            applicationData.RDSelectedItemIndexChanged += _ =>
            {
                appDrawService.DrawSelectedMain(ActiveWindow.Right);
                appDrawService.SetCursorPositionForUserInput();
            };
            
            applicationData.LDPathChanged += _ => appDrawService.DrawSelectedHeader(ActiveWindow.Left);
            applicationData.RDPathChanged += _ => appDrawService.DrawSelectedHeader(ActiveWindow.Right);
        }
        /// <summary>
        /// Обработчик ввода пользователя
        /// </summary>
        /// <param name="appCommandService">Сервис команд</param>
        /// <param name="data">Данные приложения для межсервисного общения</param>
        private static void ProcessUserInput(AppCommandService appCommandService, ApplicationData data)
        {
            data.OnSubmitCommand += _ => appCommandService.ProcessCommand(data);
            string lastCommand;
            do
            {
                var newInput = Console.ReadKey();

                if (appCommandService.CommandKeys.Contains(newInput.Key))
                {
                    appCommandService.ProcessKeyCommand(newInput, data);
                }
                else
                {
                    var userInput = data.UserInput.Value;
                    data.ChangeUserInput(string.Concat(userInput, newInput.KeyChar));
                }

                lastCommand = data.UserInputHistory.LastOrDefault();
                
            } while (lastCommand is null || !lastCommand.Equals("exit", StringComparison.InvariantCultureIgnoreCase));
            Environment.Exit(0);
        }
    }
}