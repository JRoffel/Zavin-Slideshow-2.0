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
using System.Windows.Media.Animation;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for UtilityPage.xaml
    /// </summary>
    public partial class UtilityPage : Page
    {
        public static List<string> titles;
        public static List<string> descs;

        public UtilityPage()
        {
            InitializeComponent();

            string curDir = Directory.GetCurrentDirectory();
            var test = (String.Format("file:///{0}/weather.html", curDir));
            wbWeather.Address = test;
            GetAndSetTrafficRss();

        //DoubleAnimation TrafficAnimation = new DoubleAnimation();
        //TrafficAnimation.From = 0;
        //TrafficAnimation.To = TrafficControl.ActualHeight / 2;
        //TrafficAnimation.Duration = TimeSpan.FromMilliseconds(4000);
        //TrafficControl.BeginAnimation(Canvas.HeightProperty, TrafficAnimation);

        }

        private void GetAndSetTrafficRss()
        {
           
            XDocument doc = XDocument.Load("http://www.verkeerplaza.nl/rssfeed");
            titles = (from x in doc.Descendants("item")
                     select x.Element("title").Value).ToList();
            descs = (from x in doc.Descendants("item")
                     select x.Element("description").Value).ToList();

            for (int i = 0; i < titles.Count; i++)
            {
                TextBlock trafficTitle = new TextBlock();
                TextBlock trafficDesc = new TextBlock();
                if (titles[i].Contains("Bron: Verkeerplaza.nl: "))
                {
                    titles[i].Replace("Bron: Verkeerplaza.nl: ", "");
                }
                trafficTitle.Text = titles[i];
                trafficTitle.FontSize = 20;
                trafficTitle.Margin = new Thickness(30, 10, 50, 0);
                trafficDesc.Text = descs[i];
                trafficDesc.FontSize = 15;
                trafficDesc.Margin = new Thickness(90, 10, 10, 30);
                trafficPanel.Children.Add(trafficTitle);
                trafficPanel.Children.Add(trafficDesc);
            }
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
