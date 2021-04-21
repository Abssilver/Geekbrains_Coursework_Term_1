namespace FileManager.DrawService
{
    /// <summary>
    /// Класс сущности для отрисовки, зависящий от выравнивания
    /// </summary>
    public class AlignmentEntity
    {
        /// <summary>
        /// Стартовая позиция
        /// </summary>
        private Point _position;
        /// <summary>
        /// Скорректированная стартовая позиция на основе выравнивания
        /// </summary>
        public Point StartPosition
        {
            get => _position;
            private set
            {
                var newPosition = value;
                switch (this.Alignment)
                {
                    case Alignment.Left:
                        break;
                    case Alignment.Right:
                        newPosition = new Point(newPosition.X - Value.Length, newPosition.Y);
                        break;
                    case Alignment.Center:
                    default:
                        newPosition = new Point(newPosition.X - Value.Length / 2, newPosition.Y);
                        break;
                }
                _position = newPosition;
            }
        }
        /// <summary>
        /// Отступы у значения (пустые пространства вокруг значения справа и слева)
        /// </summary>
        public int Margin { get; private set; }
        /// <summary>
        /// Текстовое значение, содержащееся в сущности
        /// </summary>
        private string _value;
        /// <summary>
        /// Текстовое значение, скорректированное с учетом отступов
        /// </summary>
        public string Value
        {
            get => _value;
            private set => _value = string.Concat(new string(' ', Margin), value, new string(' ', Margin));
        }
        /// <summary>
        /// Значение выравнивания
        /// </summary>
        private Alignment Alignment { get; }
        public AlignmentEntity(Point startPosition, string value, int margin = 0, Alignment alignment = Alignment.Center)
        {
            this.Margin = margin;
            this.Value = value;
            this.Alignment = alignment;
            this.StartPosition = startPosition;
        }
        /// <summary>
        /// Изменение текстового значение
        /// </summary>
        /// <param name="newValue">Новое текстовое значение сущности</param>
        public void ChangeValue(string newValue) => Value = newValue;
        /// <summary>
        /// Изменение значения отступа
        /// </summary>
        /// <param name="newMargin">Новое значение отступа</param>
        public void ChangeMargin(int newMargin) => Margin = newMargin;
    }
}
