using System.Linq;

namespace FileManager.DrawService
{
    /// <summary>
    /// Сущность наполнения окна для отрисовки
    /// </summary>
    public class ContentEntity
    {
        /// <summary>
        /// Массив сущностей для отрисовки
        /// </summary>
        private readonly AlignmentEntity[,] _contentData;
        public ContentEntity(AlignmentEntity[] headerData, DataEntity[] contentData)
        {
            //Заполнение данными происходит на основе данных заголовков.
            //Данные используют выравнивание заголовка и его стартовую позицию
            _contentData = new AlignmentEntity[contentData.Length, contentData.First().GetData.Length];
            for (int i = 0; i < contentData.Length; i++)
            {
                for (int j = 0; j < headerData.Length; j++)
                {
                    if (j < contentData[i].GetData.Length)
                    {
                        _contentData[i, j] = new AlignmentEntity(
                                                startPosition: new Point(
                                                    headerData[j].StartPosition.X, 
                                                    headerData[j].StartPosition.Y + i + 1),
                                                value: contentData[i].GetData[j],
                                                margin: headerData[j].Margin,
                                                alignment: Alignment.Left);
                    }
                }
            }
        }
        /// <summary>
        /// Получение массива сущностей для отрисовки
        /// </summary>
        public AlignmentEntity[,] GetData => _contentData;
    }
}
