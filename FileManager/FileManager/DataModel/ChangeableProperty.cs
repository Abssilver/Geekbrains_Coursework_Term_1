using System;

namespace FileManager.DataModel
{
    /// <summary>
    /// Изменяемое свойство, вызывающее ивент изменения OnValueChanged в момент смены значения
    /// </summary>
    /// <typeparam name="T">Тип свойства</typeparam>
    public class ChangeableProperty<T>
    {
        /// <summary>
        /// Значение свойства для хранения переменной
        /// </summary>
        private T _value;
        /// <summary>
        /// Значение свойства для работы ивента
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        /// <summary>
        /// Ивент изменения значения свойства
        /// </summary>
        public event Action<T> OnValueChanged;
        public ChangeableProperty()
        { }
        public ChangeableProperty(T value)
        {
            _value = value;
        }
    }
}