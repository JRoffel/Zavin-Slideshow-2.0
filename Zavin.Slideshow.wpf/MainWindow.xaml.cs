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
        public static List<string> items;
        public static List<string> newItems;

        public DispatcherTimer MoveTicker = new System.Windows.Threading.DispatcherTimer();
        public DispatcherTimer EditList = new System.Windows.Threading.DispatcherTimer();
        public bool startEdit = false;
        public int CanvasX = 1920;
        public MainWindow()
        {
            InitializeComponent();
            XDocument doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");

            items = (from x in doc.Descendants("item")
                                  select x.Element("title").Value).ToList();
            for (int i = 0; i < 2; i++)
            {
                foreach (var item in items)
                {
                    newItems.Add(item);
                }
            }
            
            combinedString = string.Join("  -  ", newItems.ToArray());
            test.Text = combinedString;

            EditList.Tick += new EventHandler(EditList_Tick);
            EditList.Interval = new TimeSpan(0, 0, 25);

            MoveTicker.Tick += new EventHandler(MoveTicker_Tick);
            MoveTicker.Interval = TimeSpan.FromMilliseconds(1);
            MoveTicker.Start();

        }
        private void EditList_Tick(object sender, EventArgs e)
        {
            
        }

        async void MoveTicker_Tick(object sender, EventArgs e)
        {
            if(startEdit == false)
            {
                Canvas.SetLeft(test, CanvasX);
                CanvasX--;
                if (CanvasX < 0)
                {
                    EditList.Start();
                    startEdit = true;
                    await Task.Delay(1);
                }
                }else
                {
                    Canvas.SetLeft(test, CanvasX);
                    CanvasX--;
                    await Task.Delay(1);
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
