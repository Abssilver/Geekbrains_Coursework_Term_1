using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using FileManager.DataModel;
using FileManager.DrawService;
using FileManager.LogService;

namespace FileManager.LoaderService
{
    /// <summary>
    /// Класс для загрузки/сохранения данных между сессиями
    /// </summary>
    public class ApplicationDataLoader
    {
        /// <summary>
        /// Данные приложения для коммуникации между сервисами
        /// </summary>
        public ApplicationData ApplicationData { get; }
        /// <summary>
        /// Настройки приложения по-умолчанию
        /// </summary>
        private readonly ApplicationSettings _applicationSettings;
        /// <summary>
        /// Ссылка на сервис логгирования
        /// </summary>
        private readonly ILogService _logService;
        public ApplicationDataLoader(ILogService logService, string path, ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
            _logService = logService;

            ApplicationData = LoadData(path);
            ApplicationData.LDPathChanged += _ => SaveData(path, ApplicationData);
            ApplicationData.RDPathChanged += _ => SaveData(path, ApplicationData);
        }
        /// <summary>
        /// Метод загрузки данных приложения по указанному пути
        /// </summary>
        /// <param name="path">Путь для загрузки данных</param>
        /// <returns>Данные приложения для коммуникации между сервисами</returns>
        /// <exception cref="SerializationException">Ошибка сериализации из-за нарушенных данных</exception>
        private ApplicationData LoadData(string path)
        {
            ApplicationData dataToReturn;
            IWindowData leftWindow, rightWindow;
            
            if (File.Exists(path))
            {
                try
                {
                    using StreamReader reader = new StreamReader(File.OpenRead(path));
                    var loadedData = JsonSerializer.Deserialize<PersistenceData[]>(reader.ReadToEnd());
                    
                    if (loadedData is null || loadedData.Any(data => !Directory.Exists(data.DirectoryPath)))
                        throw new SerializationException(
                            "Serialization completed with null or directory no more exists");

                    if (loadedData.Length.Equals(2))
                    {
                        leftWindow = GetWindowData(loadedData[0]);
                        rightWindow = GetWindowData(loadedData[1]);
                        dataToReturn = new ApplicationData(leftWindow, rightWindow);
                        dataToReturn.ChangeSystemOutput("Data restored! Greetings!");
                        return dataToReturn;
                    }
                }
                catch (Exception exception)
                {
                    _logService.LogError(exception, null);
                }
            }
            
            leftWindow = new WindowData
            {
                WindowType = ActiveWindow.Left,
                DirectoryData = new DirectoryData(_applicationSettings.DefaultPath),
                Path = new ChangeableProperty<string>(_applicationSettings.DefaultPath),
                SelectedItemIndex = new ChangeableProperty<int>(_applicationSettings.DefaultSelectedItemIndex),
                MaxElementsPerPage = _applicationSettings.MaxElementsPerPage,
            };
            
            rightWindow = new WindowData
            {
                WindowType = ActiveWindow.Right,
                DirectoryData = new DirectoryData(_applicationSettings.DefaultPath),
                Path = new ChangeableProperty<string>(_applicationSettings.DefaultPath),
                SelectedItemIndex = new ChangeableProperty<int>(_applicationSettings.DefaultSelectedItemIndex),
                MaxElementsPerPage = _applicationSettings.MaxElementsPerPage,
            };
            
            dataToReturn = new ApplicationData(leftWindow, rightWindow);
            dataToReturn.ChangeSystemOutput("Greetings!");
            
            SaveData(path, dataToReturn);
            
            return dataToReturn;
        }
        /// <summary>
        /// Метод формирования данных окна на основе данных для сериализации между сессиями
        /// </summary>
        /// <param name="loadedData">Данные для сериализации между сессиями</param>
        /// <returns>Сформированнные данные окна</returns>
        private IWindowData GetWindowData(PersistenceData loadedData) =>
            new WindowData
            {
                WindowType = loadedData.WindowType,
                DirectoryData = new DirectoryData(loadedData.DirectoryPath),
                Path = new ChangeableProperty<string>(loadedData.DirectoryPath),
                SelectedItemIndex = new ChangeableProperty<int>(loadedData.SelectedItemIndex),
                MaxElementsPerPage = _applicationSettings.MaxElementsPerPage,
            };

        /// <summary>
        /// Метод сохранения данных приложения по указанному пути
        /// </summary>
        /// <param name="path">Путь для сохранения данных</param>
        /// <param name="data">Данные приложения для сохранения</param>
        private void SaveData(string path, ApplicationData data)
        {
            try
            {
                //На основе данных для приложения формируем данные для сохранения между сессиями
                using StreamWriter writer = new StreamWriter(File.Create(path));
                var persistenceData = data.GetWindowData.Select(windowData => new PersistenceData
                {
                    WindowType = windowData.WindowType,
                    DirectoryPath = windowData.Path.Value,
                    SelectedItemIndex = windowData.SelectedItemIndex.Value,
                }).ToArray();
                
                var json = JsonSerializer.Serialize(persistenceData); 
                writer.WriteLine(json);
            }
            catch (Exception exception)
            {
                _logService.LogError(exception, data.ChangeSystemOutput);
            }
        }
    }
}

