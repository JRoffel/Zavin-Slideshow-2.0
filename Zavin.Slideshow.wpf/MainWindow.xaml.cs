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

        public string combinedString;
        public List<string> items;

        public DispatcherTimer MoveTicker = new System.Windows.Threading.DispatcherTimer();
        public DispatcherTimer EditList = new System.Windows.Threading.DispatcherTimer();
        public bool startEdit = false;
        public int CanvasX = 770;
        public MainWindow()
        {
            InitializeComponent();
            XDocument doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");

            items = (from x in doc.Descendants("item")
                                  select x.Element("title").Value).ToList();

            combinedString = string.Join("  -  ", items.ToArray());
            test.Text = combinedString;

            EditList.Tick += new EventHandler(EditList_Tick);
            EditList.Interval = new TimeSpan(0, 0, 7);

            MoveTicker.Tick += new EventHandler(MoveTicker_Tick);
            MoveTicker.Interval = TimeSpan.FromMilliseconds(500);
            MoveTicker.Start();

        }
        private void EditList_Tick(object sender, EventArgs e)
        {
            int x = items.Count;
            string tempheadline = items[0];
            items.Remove(items[0]);
            items.Add(tempheadline);
            combinedString = string.Join("  -  ", items.ToArray());
            test.Text = combinedString;
        }

        private void MoveTicker_Tick(object sender, EventArgs e)
        {
            if(startEdit == false)
            {
                Canvas.SetLeft(test, CanvasX);
                CanvasX--;
                if (CanvasX < 0)
                {
                    EditList.Start();
                    startEdit = true;
                }
            }else
            {
                Canvas.SetLeft(test, CanvasX);
                CanvasX--;
            }
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
