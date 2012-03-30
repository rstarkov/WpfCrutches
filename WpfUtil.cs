using System.Windows;
using System.Windows.Media;

namespace WpfCrutches
{
    /// <summary>
    /// Provides various WPF-related utility methods.
    /// </summary>
    public static class WpfUtil
    {
        /// <summary>
        /// Searches up the visual tree from the specified dependency object, until an object of
        /// type <typeparamref name="T"/> is found. Returns the object found, or null if none.
        /// </summary>
        public static T VisualUpwardSearch<T>(DependencyObject source) where T : DependencyObject
        {
            while (source != null && !(source is T))
                source = VisualTreeHelper.GetParent(source);
            return (T) source;
        }
    }
}
