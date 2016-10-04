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
using System.ComponentModel;
using System.Net;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainController mainController = new MainController();

        public string combinedString;
        public static List<string> items;
        public static List<string> newItems = new List<string>();

        System.Timers.Timer timer = new System.Timers.Timer(mainController.GetSlideTimer());
        System.Timers.Timer updateTimer = new System.Timers.Timer(300000);


        public DispatcherTimer MoveTicker = new DispatcherTimer();
        public double Canvas1X = 1920;
        public double Canvas2X;
        public double Canvas1Width;
        public double Canvas2Width;
        public bool update1 = false;
        public bool update2 = true;

        public int slideCounter = 0;
        public MainWindow()
        {
            timer.AutoReset = true;
            timer.Elapsed += (sender, e) => NextSlide();
            timer.Start();

            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (sender, e) => UpdateOldTimer();
            updateTimer.Start();

            InitializeComponent();
            
            PageFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            string combinedString = MoveAndGet();

            test1.Text = combinedString + "  -  ";
            test2.Text = combinedString + "  -  ";
            
            var descriptor = DependencyPropertyDescriptor.FromProperty(ActualWidthProperty, typeof(TextBlock));
            if (descriptor != null)
                descriptor.AddValueChanged(test1, ActualWidth_ValueChanged);
            
            Canvas2X = Canvas1Width + 1920;
            Canvas.SetLeft(test2, Canvas2X);

            MoveTicker.Tick += new EventHandler(MoveTicker_Tick);
            MoveTicker.Interval = TimeSpan.FromMilliseconds(1);
            MoveTicker.Start();

        }

        private void ActualWidth_ValueChanged(object a_sender, EventArgs a_e)
        {
            Canvas1Width = test1.ActualWidth;
            Canvas2Width = test2.ActualWidth;
            Canvas2X = Canvas1Width + 1920;
            Canvas.SetLeft(test2, Canvas2X);

            //canvas1Xtext.Text = Canvas1X.ToString();
            //canvas2Xtext.Text = Canvas2X.ToString();
            //canvas1Widthtext.Text = Canvas1Width.ToString();
            //canvas2Widthtext.Text = Canvas2Width.ToString();
        }

        async void MoveTicker_Tick(object sender, EventArgs e)
        {
            if (Canvas2X < 0 && update1 == false)
            {
                Canvas2Width = Convert.ToInt32(test2.ActualWidth);
                Canvas1X = Canvas2Width + 1;
                Canvas.SetLeft(test1, Canvas1X);
                Canvas.SetLeft(test2, Canvas2X);
                Canvas1X -= 4;
                Canvas2X -= 4;

                update1 = true;
                update2 = false;
                test1.Text = MoveAndGet() + "  -  ";
                await Task.Delay(1);
            }
            else if (Canvas1X < 0 && update2 == false)
            {
                Canvas1Width = Convert.ToInt32(test2.ActualWidth);
                Canvas2X = Canvas1Width + 1;
                Canvas.SetLeft(test1, Canvas1X);
                Canvas.SetLeft(test2, Canvas2X);
                Canvas1X -= 4;
                Canvas2X -= 4;

                update2 = true;
                update1 = false;
                test2.Text = MoveAndGet() + "  -  ";
                await Task.Delay(1);
            }
            else
            {
                Canvas.SetLeft(test1, Canvas1X);
                Canvas.SetLeft(test2, Canvas2X);
                Canvas1X -= 4;
                Canvas2X -= 4;
                await Task.Delay(1);
            }
        }

        private string MoveAndGet()
        {
            try
            {
                XDocument doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");

                items = (from x in doc.Descendants("item")
                         select x.Element("title").Value).ToList();

                combinedString = string.Join("  -  ", items.ToArray());
            }
            catch(WebException e)
            {
                Console.WriteLine(e);
                combinedString = "Could not get RSS feed, you might not have an internet connection, or nu.nl might be down, we will keep trying every 30 seconds";
            }


            return combinedString;
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

        private void UpdateOldTimer()
        {
            timer.Interval = mainController.GetSlideTimer();
        }
    }
}