using System.ComponentModel;

namespace Zavin.Slideshow.wpf
{
    static class Helpers
    {
        public static void InvokePropertyChanged(PropertyChangedEventHandler propertyChanged, object sender, string propertyName)
        {
            var handler = propertyChanged;
            if (null != handler)
            {
                handler.Invoke(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
