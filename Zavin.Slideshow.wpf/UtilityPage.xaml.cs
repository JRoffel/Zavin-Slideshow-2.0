using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Windows.Markup;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.IO;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for UtilityPage.xaml
    /// </summary>
    public partial class UtilityPage : Page
    {

        public UtilityPage()
        {
            InitializeComponent();

            string curDir = Directory.GetCurrentDirectory();
            var test = (String.Format("file:///{0}/weather.html", curDir));
            wbWeather.Address = test;
        }
    }
    

    [ValueConversion(typeof(object), typeof(string))]
    public class DataSanitizer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = (string)value;
            return strValue.Replace("Bron: Verkeerplaza.nl: ", "");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = (string)value;
            return strValue.Replace("Bron: Verkeerplaza.nl: ", "");
        }
    }

    //public abstract class BaseConverter : MarkupExtension
    //{
    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return this;
    //    }
    //}
}
