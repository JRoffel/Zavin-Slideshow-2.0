using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Threading;

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
        public static List<string> newItems = new List<string>();


        public DispatcherTimer MoveTicker = new DispatcherTimer();
        public DispatcherTimer RefreshListTimer = new DispatcherTimer();
        public bool startEdit = false;
        public int CanvasX = 1920;

        public int slideCounter = 0;
        public MainWindow()
        {
            System.Timers.Timer timer = new System.Timers.Timer(30000);
            timer.AutoReset = true;
            timer.Elapsed += (sender, e) => NextSlide();
            timer.Start();
            InitializeComponent();

            PageFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

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

            RefreshListTimer.Tick += new EventHandler(RefreshListTimer_Tick);
            RefreshListTimer.Interval = new TimeSpan(0, 0, 25);
            RefreshListTimer.Start();

            MoveTicker.Tick += new EventHandler(MoveTicker_Tick);
            MoveTicker.Interval = TimeSpan.FromMilliseconds(1);
            MoveTicker.Start();

        }
        private void RefreshListTimer_Tick(object sender, EventArgs e)
        {
            items.Clear();
            newItems.Clear();
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
        }

        async void MoveTicker_Tick(object sender, EventArgs e)
        {
            Canvas.SetLeft(test, CanvasX);
            CanvasX -= 4;
            await Task.Delay(1);
        }

        private void NextSlide()
        {
            Thread thread = new Thread(ThreadProc);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }


        private void MainWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void ThreadProc()
        {
            slideCounter += 1;
            switch (slideCounter)
            {
                case 1:
                    Dispatcher.Invoke(() => PageFrame.NavigationService.Navigate(new YearGraphPage()));
                    break;

                case 2:
                    Dispatcher.Invoke(() => PageFrame.NavigationService.Navigate(new UtilityPage()));
                    break;

                case 3:
                    Dispatcher.Invoke(() => PageFrame.NavigationService.Navigate(new WeekGraphPage()));
                    slideCounter = 0;
                    break;
            }
            
        }
    }
}
