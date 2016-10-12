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
using System.Windows.Threading;
using System.Net;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for UtilityPage.xaml
    /// </summary>
    public partial class UtilityPage : Page
    {
        public System.Timers.Timer Traffictimer;
        public static List<string> titles;
        public static List<string> descs;
        public double TrafficMainHeight;
        public double TrafficBackupHeight;
        public double TrafficMainTop;
        public double TrafficBackupTop;
        public bool Tupdate1 = false;
        public bool Tupdate2 = true;

        public UtilityPage()
        {
            Traffictimer = new System.Timers.Timer(1);
            Traffictimer.AutoReset = true;
            Traffictimer.Elapsed += MoveTicker_Tick;


            InitializeComponent();

            string curDir = Directory.GetCurrentDirectory();
            var test = (String.Format("file:///{0}/weather.html", curDir));
            wbWeather.Address = test;
            GetAndSetTrafficRssMain();


            var descriptor = DependencyPropertyDescriptor.FromProperty(ActualHeightProperty, typeof(StackPanel));
            if (descriptor != null)
                descriptor.AddValueChanged(trafficPanelMain, ActualHeight_ValueChanged);

        }

        private void ActualHeight_ValueChanged(object a_sender, EventArgs a_e)
        {
            TrafficMainHeight = trafficPanelMain.ActualHeight;
            TrafficBackupHeight = trafficPanelBackup.ActualHeight;
            TrafficBackupTop = TrafficMainHeight;
            Canvas.SetTop(trafficPanelBackup, TrafficBackupTop);
        }

        private void GetAndSetTrafficRssMain()
        {
            try
            {
                XDocument doc = XDocument.Load("http://www.verkeerplaza.nl/rssfeed");
                titles = (from x in doc.Descendants("item")
                          select x.Element("title").Value).ToList();
                descs = (from x in doc.Descendants("item")
                         select x.Element("description").Value).ToList();
                if (!titles.Any())
                {

                    TextBlock trafficTemp = new TextBlock();
                    trafficTemp.Text = "There is currently no traffic information available.";
                    trafficTemp.FontSize = 20;
                    trafficTemp.Margin = new Thickness(30, 10, 50, 0);
                    trafficPanelMain.Children.Add(trafficTemp);
                }
                for (int i = 0; i < titles.Count; i++)
                {
                    TextBlock trafficTitle = new TextBlock();
                    TextBlock trafficDesc = new TextBlock();
                    string temptext = titles[i];
                    if (temptext.Contains("Bron: Verkeerplaza.nl: "))
                    {
                        temptext.Replace("Bron: Verkeerplaza.nl: ", "");
                    }
                    trafficTitle.Text = temptext;
                    trafficTitle.FontSize = 20;
                    trafficTitle.Margin = new Thickness(30, 10, 50, 0);
                    trafficDesc.Text = descs[i];
                    trafficDesc.FontSize = 15;
                    trafficDesc.Margin = new Thickness(90, 10, 10, 30);
                    trafficPanelMain.Children.Add(trafficTitle);
                    trafficPanelMain.Children.Add(trafficDesc);
                }

                if (titles.Count > 5)
                {
                    GetAndSetTrafficRssBackup();
                }
            }
            catch (WebException)
            {
                TextBlock trafficTemp = new TextBlock();
                trafficTemp.Text = "couldn't load Rss feed. Maybe check your internet connection?";
                trafficTemp.FontSize = 20;
                trafficTemp.Margin = new Thickness(30, 10, 50, 0);
                trafficPanelMain.Children.Add(trafficTemp);
            }
        }


        private void GetAndSetTrafficRssBackup()
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
                trafficPanelBackup.Children.Add(trafficTitle);
                trafficPanelBackup.Children.Add(trafficDesc);
            }

            Traffictimer.Start();
        }

        private void MoveTicker_Tick(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (TrafficBackupTop < 0 && Tupdate1 == false)
                {
                    TrafficBackupHeight = Convert.ToInt32(trafficPanelBackup.ActualHeight);
                    TrafficMainTop = TrafficBackupHeight;
                    Canvas.SetTop(trafficPanelMain, TrafficMainTop);
                    Canvas.SetTop(trafficPanelBackup, TrafficBackupTop);
                    TrafficMainTop -= 1;
                    TrafficBackupTop -= 1;
                    
                    Tupdate1 = true;
                    Tupdate2 = false;
                }
                else if (TrafficMainTop < 0 && Tupdate2 == false)
                {
                    TrafficMainHeight = Convert.ToInt32(trafficPanelMain.ActualHeight);
                    TrafficBackupTop = TrafficMainHeight;
                    Canvas.SetTop(trafficPanelMain, TrafficMainTop);
                    Canvas.SetTop(trafficPanelBackup, TrafficBackupTop);
                    TrafficMainTop -= 1;
                    TrafficBackupTop -= 1;
                    
                    Tupdate2 = true;
                    Tupdate1 = false;
                }
                else
                {
                    Canvas.SetTop(trafficPanelMain, TrafficMainTop);
                    Canvas.SetTop(trafficPanelBackup, TrafficBackupTop);
                    TrafficMainTop -= 1;
                    TrafficBackupTop -= 1;
                }
            });
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
