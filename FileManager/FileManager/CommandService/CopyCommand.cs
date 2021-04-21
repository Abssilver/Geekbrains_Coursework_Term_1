using System;
using System.IO;
using System.Linq;
using FileManager.CommandService.BaseCommands;
using FileManager.CommandService.CommandArgs;
using FileManager.DrawService;

namespace FileManager.CommandService
{
    /// <summary>
    /// Команда копирования выделенного элемента
    /// </summary>
    public class CopyCommand : CompositeCommand
    {
        public override string Help { get; } = $"{LD}\\{RD} Copy {LD}\\{RD}";
        public CopyCommand() : base(indexOfCommandInInput: 1, argsInCommand: 3, commandName: "copy")
        { }

        public override void Handle(NamedCommandArgs args)
        {
            string[] command = args.Data.UserInputHistory.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (!ValidateInput(out var isFromLD, 0, command)
                || !ValidateInput(out var isToLD, 2, command))
            {
                args.Data.ChangeSystemOutput($"Invalid command! Try \"{LD} Copy {RD}\" or \"{RD} Copy {LD}\" instead");
                return;
            }

            var copyData = new CopyData
            {
                CopyFrom = isFromLD ? ActiveWindow.Left : ActiveWindow.Right,
                CopyTo = isToLD ? ActiveWindow.Left : ActiveWindow.Right,
            };

            copyData.ItemIndex = args.Data.GetSelectedItemIndex(copyData.CopyFrom);
            copyData.PathToCopy = args.Data.GetSelectedPath(copyData.CopyTo);

            if (copyData.ItemIndex.Equals(0))
            {
                args.Data.ChangeSystemOutput($"Selected item is a root! Unable to copy the root");
                return;
            }

            copyData.ItemPath = args.Data.GetItemPathBySelectedIndex(copyData.CopyFrom, copyData.ItemIndex);
            copyData.CopyName = args.Data.GetItemNames(copyData.CopyFrom)[copyData.ItemIndex];
            
            Copy(File.Exists,
                copyData,
                File.Copy,
                name =>
                {
                    args.Data.ChangeSystemOutput(
                        $"File successfully copied with name \"{name}\"");
                    
                    UpdateGraphics(args, copyData);
                    
                }, out bool isCopyDone);
            
            if (isCopyDone)
                return;

            Copy(Directory.Exists,
                copyData,
                (source, target) => CopyDirectory(
                    new DirectoryInfo(source),
                    new DirectoryInfo(target)),
                name =>
                {
                    args.Data.ChangeSystemOutput(
                        $"Directory successfully copied with name \"{name}\"");
                    
                    UpdateGraphics(args, copyData);

                }, out _);
        }
        
        /// <summary>
        /// Метод для обновления отображения окон
        /// </summary>
        /// <param name="args">Аргументы команды</param>
        /// <param name="copyData">Данные для копирования</param>
        private void UpdateGraphics(ICommandArgs args, CopyData copyData)
        {
            args.Data.ChangeSelectedIndex(copyData.CopyTo, 0);
                    
            if (args.Data.GetSelectedPath(copyData.CopyTo).Equals(args.Data.GetSelectedPath(copyData.CopyFrom)))
                args.Data.ChangeSelectedIndex(copyData.CopyFrom, 0);
        }
        /// <summary>
        /// Метод для копирования выбранного элемента
        /// </summary>
        /// <param name="predicate">Условие выполения метода</param>
        /// <param name="data">Данные для копирования</param>
        /// <param name="copyAction">Действие копирования</param>
        /// <param name="callback">Действие по завершению копирования</param>
        /// <param name="success">Флаг того, что копирование успешно завершено</param>
        private void Copy(
            Func<string, bool> predicate, 
            CopyData data, 
            Action<string, string> copyAction, 
            Action<string> callback,
            out bool success)
        {
            success = false;
            if (predicate(data.ItemPath))
            {
                var prefix = string.Empty;
                var copyFullPath = Path.Combine(data.PathToCopy, data.CopyName);
                
                for (int i = 1; predicate(copyFullPath); i++)
                {
                    prefix = $"Copy({i})_";
                    copyFullPath = Path.Combine(data.PathToCopy, string.Concat(prefix, data.CopyName));
                }

                copyAction(data.ItemPath, copyFullPath);

                callback?.Invoke(string.Concat(prefix, data.CopyName));
                success = true;
            } 
        }
        /// <summary>
        /// Метод копирования директории
        /// </summary>
        /// <param name="source">Данные о директории откуда происходит копирование</param>
        /// <param name="target">Данные о директории куда происходит копирование</param>
        private void CopyDirectory(DirectoryInfo source, DirectoryInfo target) 
        { 
            Directory.CreateDirectory(target.FullName); 
            
            foreach (var file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            
            foreach (var subDirectory in source.GetDirectories()) 
            { 
                DirectoryInfo nextSubDir = target.CreateSubdirectory(subDirectory.Name); 
                CopyDirectory(subDirectory, nextSubDir); 
            }
        }
        /// <summary>
        /// Структура данных для помощи в копировании
        /// </summary>
        private struct CopyData
        {
            /// <summary>
            /// Путь к элементу, который будет скопирован
            /// </summary>
            public string ItemPath { get; set; }
            /// <summary>
            /// Индекс копируемого элемента
            /// </summary>
            public int ItemIndex { get; set; }
            /// <summary>
            /// Активное окно, откуда будет произведено копирование
            /// </summary>
            public ActiveWindow CopyFrom { get; set; }
            /// <summary>
            /// Активное окно, куда будет произведено копирование
            /// </summary>
            public ActiveWindow CopyTo { get; set; }
            /// <summary>
            /// Имя созданной копии
            /// </summary>
            public string CopyName { get; set; }
            /// <summary>
            /// Путь директории, куда будет произведено копирование
            /// </summary>
            public string PathToCopy { get; set; }
        }
    }
}