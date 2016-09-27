using System;
using System.Net;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls.DataVisualization.Charting;
using System.Xml.Linq;
using System.Timers;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainController mainController = new MainController();

        static string combinedString;
        public MainWindow()
        {
            InitializeComponent();
            XDocument doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");

            List<string> items = (from x in doc.Descendants("item")
                                  select x.Element("title").Value).ToList();
            combinedString = string.Join("  -  ", items.ToArray());

            DispatcherTimer ticker = new System.Windows.Threading.DispatcherTimer();
            ticker.Tick += new EventHandler(ticker_Tick);
            ticker.Interval = new TimeSpan(0, 5, 0);
            ticker.Start();

        }
        private void ticker_Tick(object sender, EventArgs e)
        {
            // note to self: try taking the first item after a while and past it after the last
        }


        private void MainWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        
    }
}
