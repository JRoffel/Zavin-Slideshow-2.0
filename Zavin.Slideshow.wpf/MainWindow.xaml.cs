using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;

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


        public DispatcherTimer NextSlideTimer = new DispatcherTimer();
        public System.Timers.Timer tmr;
        public System.Timers.Timer timer;
        public double Canvas1X = 1920;
        public double Canvas2X;
        public double CanvasLogo1X;
        public double CanvasLogo2X;
        public double Canvas1Width;
        public double Canvas2Width;
        public double CanvasLogoWidth = 60;
        public bool update1 = false;
        public bool update2 = true;
        
        

        public int slideCounter = 0;
        public MainWindow()
        {
            timer = new System.Timers.Timer(30000);
            timer.AutoReset = true;
            timer.Elapsed += (sender, e) => NextSlide();
            timer.Start();

            tmr = new System.Timers.Timer(1);
            tmr.AutoReset = true;
            tmr.Elapsed += MoveTicker_Tick;
            tmr.Start();

            InitializeComponent();
            
            PageFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            GetAndSetRss1();
            GetAndSetRss2();

            //test1block.Text = GetAndSetRss();
            //test2block.Text = GetAndSetRss();

            var descriptor = DependencyPropertyDescriptor.FromProperty(ActualWidthProperty, typeof(TextBlock));
            if (descriptor != null)
                descriptor.AddValueChanged(test1, ActualWidth_ValueChanged);
            
            Canvas2X = Canvas1Width + 1920 + CanvasLogoWidth;
            CanvasLogo1X = Canvas1Width + 1920;
            CanvasLogo2X = Canvas2Width + Canvas1Width + CanvasLogoWidth + 1920;
            Canvas.SetLeft(test2, Canvas2X);
            //Canvas.SetLeft(nulogo1, CanvasLogo1X);
            //Canvas.SetLeft(nulogo2, CanvasLogo2X);
        }

        private void ActualWidth_ValueChanged(object a_sender, EventArgs a_e)
        {
            Canvas1Width = test1.ActualWidth;
            Canvas2Width = test2.ActualWidth;
            Canvas2X = Canvas1Width + 1920 + CanvasLogoWidth;
            CanvasLogo1X = Canvas1Width + 1920;
            CanvasLogo2X = Canvas2Width + Canvas1Width + CanvasLogoWidth + 1920;
            Canvas.SetLeft(test2, Canvas2X);
            //Canvas.SetLeft(nulogo1, CanvasLogo1X);
            //Canvas.SetLeft(nulogo2, CanvasLogo2X);

            //canvas1Xtext.Text = Canvas1X.ToString();
            //canvas2Xtext.Text = Canvas2X.ToString();
            //canvas1Widthtext.Text = Canvas1Width.ToString();
            //canvas2Widthtext.Text = Canvas2Width.ToString();
        }

        private void MoveTicker_Tick(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (Canvas2X < 0 && update1 == false)
                {
                    Canvas2Width = Convert.ToInt32(test2.ActualWidth);
                    Canvas1X = Canvas2Width;
                    Canvas.SetLeft(test1, Canvas1X);
                    Canvas.SetLeft(test2, Canvas2X);
                    Canvas1X -= 4;
                    Canvas2X -= 4;

                    update1 = true;
                    update2 = false;
                    GetAndSetRss1();
                }
                else if (Canvas1X < 0 && update2 == false)
                {
                    Canvas1Width = Convert.ToInt32(test2.ActualWidth);
                    Canvas2X = Canvas1Width;
                    Canvas.SetLeft(test1, Canvas1X);
                    Canvas.SetLeft(test2, Canvas2X);
                    Canvas1X -= 4;
                    Canvas2X -= 4;

                    update2 = true;
                    update1 = false;
                    GetAndSetRss2();
                }
                else
                {
                    Canvas.SetLeft(test1, Canvas1X);
                    Canvas.SetLeft(test2, Canvas2X);
                    Canvas1X -= 4;
                    Canvas2X -= 4;
                }
            });
        }

        private string GetAndSetRss1()
        {
            try
            {
                XDocument doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");

                items = (from x in doc.Descendants("item")
                         select x.Element("title").Value).ToList();


                //combinedString = string.Join("  -  ", items.ToArray());

                foreach (string item in items)
                {
                    TextBlock temptext = new TextBlock();
                    temptext.Text = "  " + item + "  ";
                    temptext.FontSize = 25;
                    temptext.Foreground = new SolidColorBrush(Colors.Navy);
                    temptext.FontWeight = FontWeights.Bold;
                    temptext.Margin = ;
                    System.Windows.Controls.Image nulogo = new System.Windows.Controls.Image();
                    nulogo.Source = new BitmapImage(new Uri(@"/images/nulogo.png", UriKind.Relative));
                    test1.Children.Add(temptext);
                    test1.Children.Add(nulogo);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                combinedString = "Could not get RSS feed, you might not have an internet connection, or nu.nl might be down, we will keep trying every 30 seconds";
            }


            return combinedString;
        }

        private string GetAndSetRss2()
        {
            try
            {
                XDocument doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");

                items = (from x in doc.Descendants("item")
                         select x.Element("title").Value).ToList();


                //combinedString = string.Join("  -  ", items.ToArray());

                foreach (string item in items)
                {
                    TextBlock temptext = new TextBlock();
                    temptext.Text = "  " + item + "  ";
                    temptext.FontSize = 25;
                    temptext.Foreground = new SolidColorBrush(Colors.Navy);
                    temptext.FontWeight = FontWeights.Bold;
                    temptext.Margin = ;
                    System.Windows.Controls.Image nulogo = new System.Windows.Controls.Image();
                    nulogo.Source = new BitmapImage(new Uri(@"/images/nulogo.png", UriKind.Relative));
                    test1.Children.Add(temptext);
                    test1.Children.Add(nulogo);
                }
            }
            catch (WebException e)
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

    private void ThreadProc()
        {
            slideCounter += 1;
            switch (slideCounter)
            {
                case 1:
                    Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new YearGraphPage()); }));
                    break;

                case 2:
                    Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new UtilityPage()); }));
                    break;

                case 3:
                    Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WeekGraphPage()); }));
                    slideCounter = 0;
                    break;
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
