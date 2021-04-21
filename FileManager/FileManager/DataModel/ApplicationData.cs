using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileManager.DrawService;

namespace FileManager.DataModel
{
    /// <summary>
    /// Данные приложения для общения между сервисами
    /// </summary>
    public class ApplicationData
    {
        /// <summary>
        /// Массив данных по каждому окну приложения (левому/правому)
        /// </summary>
        private readonly IWindowData[] _windowData;
        /// <summary>
        /// Изменяемое свойство значения системного вывода приложения
        /// </summary>
        public ChangeableProperty<string> SystemOutput { get; } = new ChangeableProperty<string>();
        /// <summary>
        /// Изменяемое свойство значения пользовательского ввода 
        /// </summary>
        public ChangeableProperty<string> UserInput { get; } = new ChangeableProperty<string>();
        /// <summary>
        /// Значение текущего активного окна
        /// </summary>
        public ActiveWindow SelectedWindow { get; private set; }
        /// <summary>
        /// Список истории пользовательских комманд
        /// </summary>
        public List<string> UserInputHistory { get; }
        /// <summary>
        /// Индекс выбора пользовательской команды из истории пользовательских комманд
        /// </summary>
        private int _historyIndexOffset;
        /// <summary>
        /// Получение массива данных по каждому окну приложения (левому/правому)
        /// </summary>
        public IWindowData[] GetWindowData => _windowData;

        /// <summary>
        /// Ивент изменения значения пути директории в левом окне
        /// </summary>
        public event Action<string> LDPathChanged;
        /// <summary>
        /// Ивент изменения значения пути директории в правом окне
        /// </summary>
        public event Action<string> RDPathChanged;
        /// <summary>
        /// Ивент изменения индекса выбранного элемента в левом окне
        /// </summary>
        public event Action<int> LDSelectedItemIndexChanged;
        /// <summary>
        /// Ивент изменения индекса выбранного элемента в правом окне
        /// </summary>
        public event Action<int> RDSelectedItemIndexChanged;
        /// <summary>
        /// Ивент срабатывания ввода пользователем
        /// </summary>
        public event Action<string> OnSubmitCommand;
        public ApplicationData(IWindowData leftPart, IWindowData rightPart)
        {
            SelectedWindow = ActiveWindow.None;
            
            _windowData = new [] { leftPart, rightPart};

            UserInputHistory = new List<string>();
            _historyIndexOffset = 0;
            
            leftPart.Path.OnValueChanged += value =>  LDPathChanged?.Invoke(value);
            leftPart.SelectedItemIndex.OnValueChanged += value => LDSelectedItemIndexChanged?.Invoke(value);
            
            rightPart.Path.OnValueChanged += value => RDPathChanged?.Invoke(value);
            rightPart.SelectedItemIndex.OnValueChanged += value => RDSelectedItemIndexChanged?.Invoke(value);
        }
        /// <summary>
        /// Метод изменения значения активного окна
        /// </summary>
        /// <param name="newActiveWindow">Новое значение активного окна</param>
        public void ChangeSelectedWindow(ActiveWindow newActiveWindow) => SelectedWindow = newActiveWindow;
        /// <summary>
        /// Метод изменения значения индекса выбранного элемента в указанном окне
        /// </summary>
        /// <param name="activeWindow">Значение активного окна</param>
        /// <param name="newIndex">Значение нового индекса</param>
        public void ChangeSelectedIndex(ActiveWindow activeWindow, int newIndex)
        {
            var window = _windowData.FirstOrDefault(data => data.WindowType.Equals(activeWindow));
            if (window is not null)
            {
                //TODO: parent is null may do incorrect logic
                window.SelectedItemIndex.Value = UseCorrectIndex(newIndex, window.DirectoryData.Count);
            }
            else
            {
                ChangeSystemOutput("Error! Invalid active window.");
            }
        }
        /// <summary>
        /// Метод изменения значения системного вывода приложения
        /// </summary>
        /// <param name="systemMessage">Новое значение системного вывода приложения</param>
        public void ChangeSystemOutput(string systemMessage) => SystemOutput.Value = systemMessage;
        /// <summary>
        /// Метод изменения значения пользовательского ввода
        /// </summary>
        /// <param name="newUserInput">Новое значение пользовательского ввода</param>
        public void ChangeUserInput(string newUserInput) => UserInput.Value = newUserInput;
        /// <summary>
        /// Метод выполнения ввода пользователя 
        /// </summary>
        public void SubmitUserInput()
        {
            UserInputHistory.Add(UserInput.Value);
            _historyIndexOffset = 0; 
            OnSubmitCommand?.Invoke(UserInput.Value);
        }
        /// <summary>
        /// Метод изменения значения пути директории в указанном окне
        /// </summary>
        /// <param name="activeWindow">Значение активного окна</param>
        /// <param name="newPath">Новое значение пути</param>
        public void ChangePath(ActiveWindow activeWindow, string newPath)
        {
            var window = _windowData.FirstOrDefault(data => data.WindowType.Equals(activeWindow));
            
            if (window is null)
            {
                ChangeSystemOutput("Error! Invalid active window.");
                return;
            }
            
            if (!Directory.Exists(newPath))
            {
                ChangeSystemOutput("Error! Path is not valid and has not been changed. Do not try to open a file");
                return;
            }

            window.DirectoryData = new DirectoryData(newPath);
            window.SelectedItemIndex.Value = 0;
            
            window.Path.Value = newPath; 
            ChangeSystemOutput("The path successfully changed");
        }
        /// <summary>
        /// Метод для получения валидного значения индекса
        /// </summary>
        /// <param name="indexToCheck">Значение индекса которое следует проверить</param>
        /// <param name="valueToCheckWith">Максимальное значение элементов</param>
        /// <returns>Скорректированное значение индекса</returns>
        private int UseCorrectIndex(int indexToCheck, int valueToCheckWith)
        {

            if (indexToCheck < 0)
                return (valueToCheckWith + indexToCheck) % valueToCheckWith;

            if (indexToCheck >= valueToCheckWith)
                return indexToCheck % valueToCheckWith;

            return indexToCheck;
        }
        /// <summary>
        /// Метод получения индекса выбранного элемента в указанном окне
        /// </summary>
        /// <param name="activeWindow">Значение активного окна</param>
        /// <returns>Числовое значение индекса</returns>
        public int GetSelectedItemIndex(ActiveWindow activeWindow) =>
            _windowData
                .FirstOrDefault(window => window.WindowType.Equals(activeWindow))?.SelectedItemIndex.Value ?? -1;
        /// <summary>
        /// Метод получения значения пути директории в указанном окне
        /// </summary>
        /// <param name="activeWindow">Значение активного окна</param>
        /// <returns>Путь директории в указанном окне</returns>
        public string GetSelectedPath(ActiveWindow activeWindow) =>
            _windowData.FirstOrDefault(window => window.WindowType.Equals(activeWindow))?.Path.Value;
        /// <summary>
        /// Метод получения данных директории в указанном окне
        /// </summary>
        /// <param name="activeWindow">Значение активного окна</param>
        /// <returns>Данные директории</returns>
        public DirectoryData GetDirectoryData(ActiveWindow activeWindow) =>
            _windowData.FirstOrDefault(window => window.WindowType.Equals(activeWindow))?.DirectoryData;
        /// <summary>
        /// Метод получения максимального количества элементов для отображения в окне на 1 странице
        /// </summary>
        /// <param name="activeWindow">Значение активного окна</param>
        /// <returns>Количество элементов для отображения на 1 странице</returns>
        public int GetMaxElementsPerPage(ActiveWindow activeWindow) =>
            _windowData.FirstOrDefault(window => window.WindowType.Equals(activeWindow))?.MaxElementsPerPage ?? -1;
        /// <summary>
        /// Метод получения пути выбранного элемента по его индексу в указанном окне
        /// </summary>
        /// <param name="activeWindow">Значение активного окна</param>
        /// <param name="index">Значение индекса выбранного элемента</param>
        /// <returns>Полный путь выбранного элемента</returns>
        public string GetItemPathBySelectedIndex(ActiveWindow activeWindow, int index) =>
            GetDirectoryData(activeWindow)?.GetElementFullPathByIndex(index);
        /// <summary>
        /// Метод получения списка имен элементов директории
        /// </summary>
        /// <param name="activeWindow">Значение активного окна</param>
        /// <returns>Список имен (директории, файлы и предок)</returns>
        public List<string> GetItemNames(ActiveWindow activeWindow) =>
            GetDirectoryData(activeWindow)?.GetNames();
        /// <summary>
        /// Метод получения предыдущей команды пользователя
        /// </summary>
        /// <returns>Значение предыдущей команды пользователя</returns>
        public string GetPreviousUserCommand()
        {
            if (UserInputHistory.Count.Equals(0))
                return null;

            var commandIndex =
                UseCorrectIndex(UserInputHistory.Count - 1 - _historyIndexOffset, UserInputHistory.Count);
            var userCommand = UserInputHistory[commandIndex];
            _historyIndexOffset = UseCorrectIndex(_historyIndexOffset + 1, UserInputHistory.Count);
            return userCommand;
        }
        /// <summary>
        /// Метод получения следующей команды пользователя
        /// </summary>
        /// <returns>Значение следующей команды пользователя</returns>
        public string GetNextUserCommand()
        {
            if (UserInputHistory.Count.Equals(0))
                return null;
            
            var commandIndex =
                UseCorrectIndex(UserInputHistory.Count - _historyIndexOffset, UserInputHistory.Count);
            var userCommand = UserInputHistory[commandIndex];
            _historyIndexOffset = UseCorrectIndex(_historyIndexOffset - 1, UserInputHistory.Count);
            return userCommand;
        }
    }
}
