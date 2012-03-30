using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfCrutches
{
    /// <summary>A crutch that enables a sensible way to bind to a dependency property with a custom conversion.</summary>
    sealed class LambdaConverter<TSource, TResult> : IValueConverter
    {
        private Func<TSource, TResult> _lambda;
        private Func<TResult, TSource> _lambdaBack;

        public LambdaConverter(Func<TSource, TResult> lambda, Func<TResult, TSource> lambdaBack)
        {
            _lambda = lambda;
            _lambdaBack = lambdaBack;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!targetType.IsAssignableFrom(typeof(TResult)))
                throw new InvalidOperationException();
            return _lambda((TSource) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_lambdaBack == null)
                throw new NotImplementedException();
            if (!targetType.IsAssignableFrom(typeof(TSource)))
                throw new InvalidOperationException();
            return _lambdaBack((TResult) value);
        }
    }

    /// <summary>A crutch that enables a sensible way to bind to several dependency properties with a custom conversion.</summary>
    sealed class LambdaMultiConverter<T1, T2, TResult> : IMultiValueConverter
    {
        private Func<T1, T2, TResult> _lambda;

        public LambdaMultiConverter(Func<T1, T2, TResult> lambda)
        {
            _lambda = lambda;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                throw new InvalidOperationException();
            if (!targetType.IsAssignableFrom(typeof(TResult)))
                throw new InvalidOperationException();
            return _lambda((T1) values[0], (T2) values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>A crutch that enables a sensible way to bind to a dependency property with a custom conversion.</summary>
    public static class LambdaBinding
    {
        /// <summary>Returns the binding with a new converter, one that uses the specified lambda(s) to perform conversions.</summary>
        public static Binding New<TSource, TResult>(Binding binding, Func<TSource, TResult> lambda, Func<TResult, TSource> lambdaBack = null)
        {
            binding.Converter = new LambdaConverter<TSource, TResult>(lambda, lambdaBack);
            return binding;
        }

        /// <summary>Creates a new multi-binding consisting of two bindings, using the specified lambda for conversions.</summary>
        public static MultiBinding New<T1, T2, TResult>(Binding b1, Binding b2, Func<T1, T2, TResult> lambda)
        {
            var result = new MultiBinding();
            result.Bindings.Add(b1);
            result.Bindings.Add(b2);
            result.Converter = new LambdaMultiConverter<T1, T2, TResult>(lambda);
            return result;
        }
    }
}