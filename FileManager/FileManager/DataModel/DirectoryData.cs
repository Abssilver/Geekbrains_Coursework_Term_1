using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManager.DataModel
{
    /// <summary>
    /// Класс данных директории
    /// </summary>
    public class DirectoryData
    {
        /// <summary>
        /// Данные о текущей директории
        /// </summary>
        public DirectoryInfo CurrentDirectoryInfo { get; }
        /// <summary>
        /// Данные о вложенных директориях
        /// </summary>
        public DirectoryInfo[] InnerDirectories => CurrentDirectoryInfo?.GetDirectories();
        /// <summary>
        /// Данные о вложенных файлах
        /// </summary>
        public FileInfo[] InnerFiles => CurrentDirectoryInfo?.GetFiles();
        /// <summary>
        /// Количество значимых элементов в текущей директории
        /// </summary>
        public int Count => CurrentDirectoryInfo is null ? 0 : 1 + InnerDirectories?.Length + InnerFiles?.Length ?? 0;

        public DirectoryData(string path)
        {
            CurrentDirectoryInfo = new DirectoryInfo(path);
        }
        /// <summary>
        /// Метод получения имен элементов текущей директории
        /// </summary>
        /// <returns>Список значимых имен текущей директории</returns>
        public List<string> GetNames()
        {
            //TODO check InnerDirectories && InnerFiles for null
            var dirEntities = InnerDirectories.Select(dir => dir.Name);
            dirEntities = dirEntities.Prepend(CurrentDirectoryInfo?.Parent?.Name);
            return dirEntities.Concat(InnerFiles.Select(file => file.Name)).ToList();
        }
        /// <summary>
        /// Метод получения полного пути к элементу по его индексу
        /// </summary>
        /// <param name="elementIndex">Индекс элемента</param>
        /// <returns>Полный путь элемента</returns>
        public string GetElementFullPathByIndex(int elementIndex)
        {
            if (elementIndex.Equals(0))
                return CurrentDirectoryInfo?.Parent?.FullName;
            
            if (elementIndex > 0 && elementIndex <= InnerDirectories?.Length)
                return InnerDirectories[elementIndex - 1].FullName;

            if (elementIndex > 0 && elementIndex <= InnerDirectories?.Length + InnerFiles?.Length)
                return InnerFiles[elementIndex - InnerDirectories.Length - 1].FullName;

            return null;
        }
    }
}