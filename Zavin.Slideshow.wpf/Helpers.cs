using System.ComponentModel;

namespace Zavin.Slideshow.wpf
{
    internal static class Helpers
    {
        public static void InvokePropertyChanged(PropertyChangedEventHandler propertyChanged, object sender, string propertyName)
        {
            var handler = propertyChanged;
            handler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
