using System;
using System.IO;
using System.Linq;
using System.Text;
using FileManager.DataModel;
using FileManager.Helpers;

namespace FileManager.DrawService
{
    /// <summary>
    /// Сервис для отображения данных работы приложения в консоль
    /// </summary>
    public class AppDrawService
    {
        /// <summary>
        /// Настройки окна
        /// </summary>
        private readonly WindowSettings _windowSettings;
        /// <summary>
        /// Данные приложения
        /// </summary>
        private readonly ApplicationData _applicationData;
        /// <summary>
        /// Положение курсора ввода команды
        /// </summary>
        private Point _userInputCursorPosition = null;
        public AppDrawService(WindowSettings windowSettings, ApplicationData applicationData)
        {
            _windowSettings = windowSettings;
            _applicationData = applicationData;
        }
        /// <summary>
        /// Метод отрисовки внешней рамки приложения
        /// </summary>
        /// <param name="applicationName">Название приложения</param>
        public void DrawOuterFrame(string applicationName)
        {
            DrawArea(
                startPosition: new Point(0, 0),
                size: new Point(_windowSettings.AppSettings.Width, _windowSettings.AppSettings.Height),
                contentData: null,
                headerData:
                    new AlignmentEntity(
                    startPosition: new Point(_windowSettings.AppSettings.Width / 2, 0),
                    margin: 2,
                    value: applicationName,
                    alignment: Alignment.Center));
        }
        /// <summary>
        /// Метод отрисовки заголовка с данными о пути открытой директории
        /// </summary>
        /// <param name="selected">Выбранное для отрисовки окно</param>
        public void DrawSelectedHeader(ActiveWindow selected)
        {
            Point headerStartPosition;
            string path = _applicationData.GetSelectedPath(selected);

            switch (selected)
            {
                case ActiveWindow.Right:
                    headerStartPosition = new Point(
                _windowSettings.AppSettings.Margin + _windowSettings.AppSettings.Width / 2 - 1,
                _windowSettings.AppSettings.Margin - 1);
                    break;
                case ActiveWindow.Left:
                    headerStartPosition = new Point(
                        _windowSettings.AppSettings.Margin, 
                        _windowSettings.AppSettings.Margin - 1);
                    break;
                default:
                    _applicationData.ChangeSystemOutput("ActiveWindow is incorrect! Unable to display header");
                    return;
            }
            
            DrawHeader(headerStartPosition, path);
        }
        /// <summary>
        /// Метод для отрисовки основного окна с контентом выбранной директории
        /// </summary>
        /// <param name="selected">Выбранное для отрисовки окно</param>
        public void DrawSelectedMain(ActiveWindow selected)
        {
            Point windowStartPosition;
            DirectoryData dirData = _applicationData.GetDirectoryData(selected);
            int selectedItemIndex = _applicationData.GetSelectedItemIndex(selected);
            
            switch (selected)
            {
                case ActiveWindow.Right:

                    windowStartPosition = new Point(
                        _windowSettings.AppSettings.Margin + _windowSettings.AppSettings.Width / 2 - 1,
                        _windowSettings.AppSettings.Margin - 1 + _windowSettings.AppSettings.HeaderHeight);
                    break;

                case ActiveWindow.Left:

                    windowStartPosition = new Point(
                        _windowSettings.AppSettings.Margin, 
                        _windowSettings.AppSettings.Margin - 1 + _windowSettings.AppSettings.HeaderHeight);
                    break;

                default:

                    _applicationData.ChangeSystemOutput("ActiveWindow is incorrect! Unable to display directory content");
                    return;
            }
            DrawDirectoryContentWindow(windowStartPosition, dirData, selected, selectedItemIndex);
        }
        //TODO: draw correct path
        /// <summary>
        /// Метод отрисовки окна заголовка на основе своих координат, данных и настроек окна
        /// </summary>
        /// <param name="startPosition">Начальная точка для отрисовки</param>
        /// <param name="path">Путь данных для наполнения</param>
        private void DrawHeader(Point startPosition, string path)
        {
            Point size = new Point(
                _windowSettings.AppSettings.Width / 2 - _windowSettings.AppSettings.Margin - 1,
                _windowSettings.AppSettings.HeaderHeight);
            
            //Для отображения определены 3 заголовка: Path, Free, Space
            //TODO: возможно их следует вынести выше, а не оставлять тут
            var headerData = new []
            {
                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + 2, startPosition.Y),
                    value: "Path",
                    margin: 2,
                    alignment: Alignment.Left),

                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + size.X / 2, startPosition.Y),
                    value: "Free",
                    margin: 2,
                    alignment: Alignment.Center),

                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + size.X - 10, startPosition.Y),
                    value: "Space",
                    margin: 2,
                    alignment: Alignment.Right),
            };
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            var diskName = dirInfo.Root.FullName;
            //TODO: check for null
            var diskInfo = DriveInfo.GetDrives().FirstOrDefault(drive => drive.Name == diskName);
            var data = new []
                {
                    new DataEntity(
                        diskName,
                        ConverterService.BytesToGigabytesString(diskInfo.AvailableFreeSpace),
                        ConverterService.BytesToGigabytesString(diskInfo.TotalSize)
                        )
                };
            DrawArea(
                startPosition, 
                size, 
                contentData: new ContentEntity(headerData, data),
                headerData: headerData);
        }
        /// <summary>
        /// Метод отрисовки контента главного окна на основе своих координат, данных и настроек окна
        /// </summary>
        /// <param name="startPosition">Начальная точка для отрисовки</param>
        /// <param name="dirData">Данные для отрисовки</param>
        /// <param name="selected">Значение выбранного окна</param>
        /// <param name="selectedItemIndex">Значение выделенной сущности для отрисовки</param>
        private void DrawDirectoryContentWindow(
            Point startPosition, 
            DirectoryData dirData, 
            ActiveWindow selected, 
            int selectedItemIndex)
        {
            Point size = new Point(
                _windowSettings.AppSettings.Width / 2 - _windowSettings.AppSettings.Margin - 1,
                _windowSettings.AppSettings.ContentWindowHeight);

            //Для отображения определены 3 заголовка: Name, Type, Size
            //TODO: возможно их следует вынести выше, а не оставлять тут
            var headerData = new []
            {
                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + 2, startPosition.Y),
                    value: "Name",
                    margin: 2,
                    alignment: Alignment.Left),

                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + size.X / 2, startPosition.Y),
                    value: "Type",
                    margin: 2,
                    alignment: Alignment.Left),

                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + size.X - 10, startPosition.Y),
                    value: "Size",
                    margin: 2,
                    alignment: Alignment.Right),
            };

            var dirEntities = dirData.InnerDirectories.Select(dir => new DataEntity
            (
                dir.Name,
                "Directory",
                string.Empty
                ));
            var fileEntities = dirData.InnerFiles.Select(file => new DataEntity
            (
                file.Name,
                $"File [{file.Extension}]",
                ConverterService.BytesToMegabytesString(file.Length)
                ));
            var parentLink = new DataEntity
            (
                string.Concat("...\\",  dirData.CurrentDirectoryInfo?.Parent?.Name ?? "No Root"),
                "Root",
                string.Empty
                );
            dirEntities = dirEntities.Prepend(parentLink);
            var data = dirEntities.Concat(fileEntities).ToArray();

            var currentPage = GetNumberOfPage(selectedItemIndex, _windowSettings.AppSettings.MaxElementsPerPage);
            var totalPages = GetNumberOfPage(data.Length - 1, _windowSettings.AppSettings.MaxElementsPerPage);
            
            //Для отображения определены 3 нижних колонтитула: Значение страницы, наименование окна и команды prev/next
            //TODO: возможно их следует вынести выше, а не оставлять тут
            
            var footerData = new []
            {
                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + 2, startPosition.Y + size.Y - 1),
                    value: $"Page {currentPage} of {totalPages}",
                    margin: 2,
                    alignment: Alignment.Left),

                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + size.X / 2, startPosition.Y + size.Y - 1),
                    //TODO: do not use ternary operator
                    value: selected == ActiveWindow.Right ? "RD" : "LD",
                    margin: 2,
                    alignment: Alignment.Center),

                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + size.X - 2, startPosition.Y + size.Y - 1),
                    value: "[PREV] [NEXT]",
                    margin: 1,
                    alignment: Alignment.Right),
            };

            //Отображении порции данных, на основе ограничений настроек окна
            var portionData = GetDataPortionToDisplay(
                data, 
                currentPage, 
                _windowSettings.AppSettings.MaxElementsPerPage);

            var indexAtPortionData = selectedItemIndex % _windowSettings.AppSettings.MaxElementsPerPage;
            
            DrawArea(
                startPosition, 
                size, 
                contentData: new ContentEntity(headerData, portionData), 
                indexAtPortionData, 
                headerData.Concat(footerData).ToArray());
        }
        /// <summary>
        /// Метод формирования порции данных на основе ограничений максимального количества данных для отображения
        /// </summary>
        /// <param name="data">Исходные данные</param>
        /// <param name="selectedPage">Значений текущей страницы для отображения</param>
        /// <param name="maxPerPage">Максимальное количество элементов для отображения</param>
        /// <returns>Сформированная порция данных</returns>
        private DataEntity[] GetDataPortionToDisplay(DataEntity[] data, int selectedPage, int maxPerPage)
        {
            var startIndex = maxPerPage * (selectedPage - 1);
            var emptyStringCount = startIndex + maxPerPage <= data.Length
                ? 0
                : startIndex + maxPerPage - data.Length;
            DataEntity[] dataPortion = new DataEntity[maxPerPage];
            Array.Copy(
                data, 
                startIndex,
                dataPortion, 
                0, 
                maxPerPage - emptyStringCount);
            for (int i = maxPerPage - emptyStringCount; i < maxPerPage; i++)
            {
                var elements = dataPortion[i - 1].GetData.Length;
                var emptyData = new string[elements];
                for (int j = 0; j < elements; j++)
                {
                    emptyData[j] = string.Empty;
                }
                dataPortion[i] = new DataEntity(emptyData);
            }
            return dataPortion;
        }
        /// <summary>
        /// Метод получения значения страницы
        /// </summary>
        /// <param name="value">Текущее значение элементов</param>
        /// <param name="maxPerPage">Максимальное количество элементов для отображения</param>
        /// <returns>Числовое значение страницы</returns>
        private int GetNumberOfPage(int value, int maxPerPage)
        {
            var integers = Math.Ceiling((double)value / maxPerPage);
            var remainder = value % maxPerPage;
            return (int)integers + (remainder > 0 ? 0 : 1);
        }
        /// <summary>
        /// Метод для отрисовки окна для взаимодействия с пользователем
        /// </summary>
        /// <param name="systemMessage">Системное сообщение работы приложения</param>
        /// <param name="userInput">Значение пользовательского ввода</param>
        public void DrawUserInterface(string systemMessage,string userInput)
        {
            var startPosition = new Point(
                _windowSettings.AppSettings.Margin,
                _windowSettings.AppSettings.Margin - 1 + _windowSettings.AppSettings.HeaderHeight + 
                _windowSettings.AppSettings.ContentWindowHeight);

            Point size = new Point(_windowSettings.AppSettings.Width - 2 * _windowSettings.AppSettings.Margin,
                _windowSettings.AppSettings.ConsoleHeight);

            var headerData = new []
            {
                //Для отображения определен 1 заголовок: Console
                //TODO: возможно его следует вынести выше, а не оставлять тут
                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + 2, startPosition.Y),
                    value: "Console",
                    margin: 2,
                    alignment: Alignment.Left),

            };

            var data = new []
                {
                    new DataEntity($"System output: {systemMessage ?? string.Empty}"),
                    new DataEntity($"Command: {userInput ?? string.Empty}")
                };

            var contentData = new ContentEntity(headerData, data);

            DrawArea(
                startPosition,
                size, 
                contentData: contentData, 
                headerData: headerData);

            //После отрисовки окна выставляем позицию курсора в корректное положение, высчитав его
            var dataArray = contentData.GetData;
            var commandDataItem = dataArray[dataArray.GetLength(0) - 1, dataArray.GetLength(1) - 1];

            _userInputCursorPosition = new Point(
                commandDataItem.StartPosition.X + commandDataItem.Value.Length - commandDataItem.Margin,
                commandDataItem.StartPosition.Y);

            SetCursorPositionForUserInput(_userInputCursorPosition);
        }
        /// <summary>
        /// Установка позиции курсорав заданную точку
        /// </summary>
        /// <param name="position">Рассчитаная позиция курсора</param>
        private void SetCursorPositionForUserInput(Point position) => Console.SetCursorPosition(position.X, position.Y);
        /// <summary>
        /// Установка позиции курсора в заранее рассчитаную позицию.
        /// Этот метод - попытка устранить ограничение того, что окно для взаимодействия с пользователем
        /// должно быть отрисовано в последнюю очередь, из-за пользовательского положения курсора
        /// </summary>
        public void SetCursorPositionForUserInput()
        {
            if (_userInputCursorPosition is null)
            {
                _applicationData.ChangeSystemOutput("Unable to position the cursor. Position is null");
                return;
            }
            Console.SetCursorPosition(_userInputCursorPosition.X, _userInputCursorPosition.Y);
        }
        /// <summary>
        /// Метод отрисовки окна с помощью
        /// </summary>
        /// <param name="helpData">Данные для отрисовки</param>
        public void DrawHelp(string [] helpData)
        {
            Point size = new Point(_windowSettings.AppSettings.Width - 2 * _windowSettings.AppSettings.Margin,
                _windowSettings.AppSettings.HelpHeight);

            var startPosition = new Point(
                _windowSettings.AppSettings.Margin,
                _windowSettings.AppSettings.Height - _windowSettings.AppSettings.HelpHeight - 
                _windowSettings.AppSettings.Margin + 1);

            //Для отображения определен 1 заголовок: Help
            //TODO: возможно его следует вынести выше, а не оставлять тут
            var headerData = new []
            {
                new AlignmentEntity(
                    startPosition: new Point(startPosition.X + 2, startPosition.Y),
                    value: "Help",
                    margin: 2,
                    alignment: Alignment.Left),

            };

            var width = size.X - 5 - headerData[0].Margin;
            var settings = helpData.Select(helpInfo => $"[{helpInfo}]").ToArray();
            var offset = width;

            for (int i = 0; i < settings.Length; i++)
            {
                offset -= settings[i].Length;
            }

            offset /= (settings.Length - 1);
            var helpString = string.Join(new string(' ', offset), settings);

            var data = new []
                {
                    new DataEntity(helpString),
                };

            DrawArea(
                startPosition, 
                size, 
                contentData: new ContentEntity(headerData, data),
                headerData: headerData);
        }
        /// <summary>
        /// Базовый метод для отрисовки.
        /// Устанавливает положение в стартовую позицию и последовательно отрисовывает данные
        /// </summary>
        /// <param name="position">Стартовая позиция</param>
        /// <param name="letters">Данные для отрисовки</param>
        private void Draw((int x, int y) position, params char[] letters)
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(letters);
        }
        /// <summary>
        /// Базовый метод для отрисовки окна
        /// </summary>
        /// <param name="startPosition">Стартовая позиция окна</param>
        /// <param name="size">Размер окна</param>
        /// <param name="contentData">Данные для наполнения окна</param>
        /// <param name="selectedItem">Выбранный элемент (может быть null)</param>
        /// <param name="headerData">Данные заголовка окна</param>
        private void DrawArea(
            Point startPosition, 
            Point size, 
            ContentEntity contentData, 
            int? selectedItem = null, 
            params AlignmentEntity[] headerData)
        {
            Point end = new Point(startPosition.X + size.X, startPosition.Y + size.Y);
            //Отрисовываем рамку
            for (int i = startPosition.X; i < end.X; i++)
            {
                for (int j = startPosition.Y; j < end.Y; j++)
                {
                    if (i == startPosition.X && j == startPosition.Y) Draw((i, j), '╔');
                    else if (i == startPosition.X && j == end.Y - 1) Draw((i, j), '╚');
                    else if (i == end.X - 1 && j == startPosition.Y) Draw((i, j), '╗');
                    else if (i == end.X - 1 && j == end.Y - 1) Draw((i, j), '╝');
                    else if (i == startPosition.X || i == end.X - 1) Draw((i, j), '║');
                    else if (j == startPosition.Y || j == end.Y - 1) Draw((i, j), '═');
                }
            }
            //Отрисовываем заголовки
            if (headerData != null)
            {
                foreach (var entity in headerData)
                    DrawEntity(entity.StartPosition, entity.Value);
            }
            //Отрисовываем наполнение окна
            if (contentData != null)
            {
                var stringBuilder = new StringBuilder();

                for (int i = 0; i < contentData.GetData.GetLength(0); i++)
                {
                    stringBuilder.Clear();

                    for (int j = 0, entities = contentData.GetData.GetLength(1); j < entities; j++)
                    {
                        var entity = contentData.GetData[i, j];
                        if (j == 0)
                        {
                            stringBuilder.Append(new string(' ',
                                entity.StartPosition.X - startPosition.X - 1));
                        }

                        var nextPoint = j != entities - 1
                            ? contentData.GetData[i, j + 1].StartPosition
                            : new Point(end.X - 1, end.Y);

                        var newEntityValue = GetFitValue(
                                entity.StartPosition,
                                nextPoint,
                                entity.Value
                                );

                        var spaceLength = nextPoint.X - entity.StartPosition.X - newEntityValue.Length;

                        stringBuilder.Append(newEntityValue);

                        stringBuilder.Append(new string(' ', spaceLength));
                    }
                    DrawEntity(
                        startPosition: new Point(
                            startPosition.X + 1, 
                            contentData.GetData[i, 0]?.StartPosition.Y ?? startPosition.Y + 1 + i),
                        value: stringBuilder.ToString(),
                        selected: selectedItem is not null && i.Equals(selectedItem));
                }
            }
        }
        /// <summary>
        /// Метод для формирования строки, помещающейся в заданные значения
        /// </summary>
        /// <param name="start">Начальная точка ограничевающего пространства</param>
        /// <param name="end">Конечная точка ограничевающего пространства</param>
        /// <param name="value">Значение, требующее проверки на вместимость</param>
        /// <returns>Значение, которое вместится в заданные рамки</returns>
        private string GetFitValue(Point start, Point end, string value)
        {
            var availableLength = end.X - start.X;
            return availableLength < value.Length
                ? string.Concat(value.Substring(0, availableLength - 2), "..")
                : value;
        }
        /// <summary>
        /// Метод отрисовки выбранной сущности
        /// </summary>
        /// <param name="startPosition">Стартовая точка для отрисовки</param>
        /// <param name="value">Текстовое значение для отрисовки</param>
        /// <param name="selected">Флаг того, что сущность выбрана</param>
        private void DrawEntity(Point startPosition, string value, bool selected = false)
        {
            if (selected)
            {
                Console.BackgroundColor = _windowSettings.AppSettings.SelectionBackgroundColor;
                Console.ForegroundColor = _windowSettings.AppSettings.SelectionForegroundColor;
            }

            Draw((startPosition.X, startPosition.Y), value.ToCharArray());

            Console.BackgroundColor = _windowSettings.AppSettings.DefaultBackgroundColor;
            Console.ForegroundColor = _windowSettings.AppSettings.DefaultForegroundColor;
        }
    }
}
