using System.ComponentModel;
using System.Windows;

namespace WpfCrutches
{
    /// <summary>
    /// Encapsulates a single value which dependency properties can depend on, and which can depend on other
    /// dependency properties.
    /// </summary>
    public sealed class DependencyValue<T> : DependencyObject
    {
        /// <summary>Gets/sets the underlying value.</summary>
        public T Value
        {
            get { return (T) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>The dependency property corresponding to <see cref="Value"/>.</summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(T), typeof(DependencyValue<T>), new PropertyMetadata(default(T)));
    }

    /// <summary>
    /// Encapsulates a single value which dependency properties can depend on.
    /// </summary>
    public sealed class ObservableValue<T> : INotifyPropertyChanged
    {
        private T _value;

        /// <summary>Converts the observable value to the underlying type.</summary>
        public static implicit operator T(ObservableValue<T> value) { return value._value; }

        /// <summary>Constructor.</summary>
        public ObservableValue() { }
        /// <summary>Constructor.</summary>
        public ObservableValue(T value) { _value = value; }

        /// <summary>Gets/sets the underlying value.</summary>
        public T Value
        {
            get { return _value; }
            set { _value = value; PropertyChanged(this, new PropertyChangedEventArgs("Value")); }
        }

        /// <summary>Triggered whenever the <see cref="Value"/> is changed.</summary>
        public event PropertyChangedEventHandler PropertyChanged = (_, __) => { };
    }
}
