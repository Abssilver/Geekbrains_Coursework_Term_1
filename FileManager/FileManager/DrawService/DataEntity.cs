namespace FileManager.DrawService
{
    /// <summary>
    /// Сущность данных для отрисовки
    /// </summary>
    public class DataEntity
    {
        /// <summary>
        /// Данные для отрисовки
        /// </summary>
        private string[] _data;
        public DataEntity(params string[] data)
        {
            _data = data; 
        }
        /// <summary>
        /// Получение данных для отрисовки
        /// </summary>
        public string[] GetData => _data;
    }
}