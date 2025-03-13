using System.Collections.Generic;
using Prism.Mvvm;

namespace EducationTesting.Client.Helpers
{
    /// <summary>
    /// Базовый класс для обновляемых свойств, реализующих INotifyPropertyChanged
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Property<T> : BindableBase
    {
        private T _value;

        /// <summary>
        /// Значение свойства
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(value, _value)) return;
                _value = value;
                RaisePropertyChanged();
            }
        }
    }
}