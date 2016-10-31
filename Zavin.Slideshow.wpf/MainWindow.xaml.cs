using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using System.Diagnostics;

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
        public bool MemoActive = false;
        public int ActiveMemo = 1;

        System.Timers.Timer timer = new System.Timers.Timer(mainController.GetSlideTimer());
        System.Timers.Timer updateTimer = new System.Timers.Timer(300000);


        public DispatcherTimer NextSlideTimer = new DispatcherTimer();
        public System.Timers.Timer tmr;
        public Stopwatch stopwatch;
        public double Canvas1X = 1920;
        public double Canvas2X;
        public double CanvasLogo1X;
        public double CanvasLogo2X;
        public double Canvas1Width;
        public double Canvas2Width;
        public bool update1 = false;
        public bool update2 = true;
        public int RequestWaitMain = 30;
        public int RequestWaitBackup = 30;

        public int slideCounter = 0;
        public MainWindow(string version)
        {
            Properties.Settings.Default.CurrentAppVersion = version;
            Properties.Settings.Default.Save();

            timer.AutoReset = true;
            timer.Elapsed += (sender, e) => NextSlide();
            timer.Start();

            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (sender, e) => UpdateOldTimer();
            updateTimer.Start();

            InitializeComponent();

            tmr = new System.Timers.Timer(1);
            tmr.AutoReset = true;
            tmr.Elapsed += MoveTicker_Tick;
            tmr.Start();

            stopwatch = new Stopwatch();
            stopwatch.Start();

            if (Properties.Settings.Default.CurrentAppVersion == "kantoor")
            {
                PlayBtn.Visibility = System.Windows.Visibility.Collapsed;
                PauseBtn.Visibility = System.Windows.Visibility.Collapsed;
                NextBtn.Visibility = System.Windows.Visibility.Collapsed;
            }

            PlayBtn.IsEnabled = false;

            int CurrentWeek = DatabaseController.GetCurrentWeek(DateTime.Now);
            ActueleWeekProductie.Text = "Actuele Productie: " + (mainController.GetProduction()[CurrentWeek].Burned);
            ActueleWeekAanvoer.Text = "Actuele Aanvoer: " + (mainController.GetAcaf()[CurrentWeek]).Value;

            PageFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            GetAndSetRssMain();
            GetAndSetRssBackup();

            var descriptor = DependencyPropertyDescriptor.FromProperty(ActualWidthProperty, typeof(TextBlock));
            if (descriptor != null)
                descriptor.AddValueChanged(HeadlineContainerMain, ActualWidth_ValueChanged);
            
            Canvas2X = Canvas1Width + 1920;
            Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
        }

        private void ActualWidth_ValueChanged(object a_sender, EventArgs a_e)
        {
            Canvas1Width = HeadlineContainerMain.ActualWidth;
            Canvas2Width = HeadlineContainerBackup.ActualWidth;
            Canvas2X = Canvas1Width + 1920;
            Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
        }

        private void MoveTicker_Tick(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (Canvas2X < 0 && update1 == false)
                {
                    Canvas2Width = Convert.ToInt32(HeadlineContainerBackup.ActualWidth);
                    Canvas1X = Canvas2Width;
                    Canvas.SetLeft(HeadlineContainerMain, Canvas1X);
                    Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
                    Canvas1X -= 4;
                    Canvas2X -= 4;

                    update1 = true;
                    update2 = false;
                    GetAndSetRssMain();
                }
                else if (Canvas1X < 0 && update2 == false)
                {
                    Canvas1Width = Convert.ToInt32(HeadlineContainerMain.ActualWidth);
                    Canvas2X = Canvas1Width;
                    Canvas.SetLeft(HeadlineContainerMain, Canvas1X);
                    Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
                    Canvas1X -= 4;
                    Canvas2X -= 4;

                    update2 = true;
                    update1 = false;
                    GetAndSetRssBackup();
                }
                else
                {
                    Canvas.SetLeft(HeadlineContainerMain, Canvas1X);
                    Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
                    Canvas1X -= 4;
                    Canvas2X -= 4;
                }
            });
        }

        private void GetAndSetRssMain()
        {
            try
            {
                if (RequestWaitMain == 30)
                {
                    XDocument doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");

                    items = (from x in doc.Descendants("item")
                             select x.Element("title").Value).ToList();
                    
                    RequestWaitMain = 0;
                }
                else
                {
                    RequestWaitMain++;
                }
                HeadlineContainerMain.Children.Clear();
                foreach (string item in items)
                {
                    TextBlock temptext = new TextBlock();
                    temptext.Text = "  " + item + "  ";
                    temptext.FontSize = 25;
                    temptext.Foreground = new SolidColorBrush(Colors.Navy);
                    temptext.FontWeight = FontWeights.Bold;
                    Thickness thickness = new Thickness();
                    thickness.Top = 5;
                    temptext.Margin = thickness;
                    System.Windows.Controls.Image nulogo = new System.Windows.Controls.Image();
                    nulogo.Source = new BitmapImage(new Uri(@"/images/nulogo.png", UriKind.Relative));
                    HeadlineContainerMain.Children.Add(temptext);
                    HeadlineContainerMain.Children.Add(nulogo);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                combinedString = "Could not get RSS feed, you might not have an internet connection, or nu.nl might be down, we will keep trying every 30 seconds  -  ";
                HeadlineContainerMain.Children.Clear();
                TextBlock temptext = new TextBlock();
                temptext.Text = combinedString;
                temptext.FontSize = 25;
                temptext.Foreground = new SolidColorBrush(Colors.Navy);
                temptext.FontWeight = FontWeights.Bold;
                Thickness thickness = new Thickness();
                thickness.Top = 5;
                temptext.Margin = thickness;
                temptext.Text = combinedString;
                HeadlineContainerMain.Children.Add(temptext);
            }
        }

        private void GetAndSetRssBackup()
        {
            try
            {
                if (RequestWaitBackup == 30)
                {
                    XDocument doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");
                    
                    items = (from x in doc.Descendants("item")
                             select x.Element("title").Value).ToList();
                  
                    RequestWaitBackup = 0;
                }
                else
                {
                    RequestWaitBackup++;
                }

                HeadlineContainerBackup.Children.Clear();
                foreach (string item in items)
                    {
                        TextBlock temptext = new TextBlock();
                        temptext.Text = "  " + item + "  ";
                        temptext.FontSize = 25;
                        temptext.Foreground = new SolidColorBrush(Colors.Navy);
                        temptext.FontWeight = FontWeights.Bold;
                        Thickness thickness = new Thickness();
                        thickness.Top = 5;
                        temptext.Margin = thickness;
                        System.Windows.Controls.Image nulogo = new System.Windows.Controls.Image();
                        nulogo.Source = new BitmapImage(new Uri(@"/images/nulogo.png", UriKind.Relative));
                        HeadlineContainerBackup.Children.Add(temptext);
                        HeadlineContainerBackup.Children.Add(nulogo);
                    }
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                combinedString = "Could not get RSS feed, you might not have an internet connection, or nu.nl might be down, we will keep trying every 30 seconds  -  ";
                HeadlineContainerMain.Children.Clear();
                TextBlock temptext = new TextBlock();
                temptext.Text = combinedString;
                temptext.FontSize = 25;
                temptext.Foreground = new SolidColorBrush(Colors.Navy);
                temptext.FontWeight = FontWeights.Bold;
                Thickness thickness = new Thickness();
                thickness.Top = 5;
                temptext.Margin = thickness;
                temptext.Text = combinedString;
                HeadlineContainerBackup.Children.Add(temptext);
            }
        }

        private void NextSlide()
        {
            stopwatch.Stop();
            stopwatch.Reset();
            stopwatch.Start();

            timer.Interval = mainController.GetSlideTimer();

            int CurrentWeek = DatabaseController.GetCurrentWeek(DateTime.Now);
            Dispatcher.Invoke(() => {
                ActueleWeekProductie.Text = "Actuele Productie: " + (mainController.GetProduction()[CurrentWeek].Burned);
                ActueleWeekAanvoer.Text = "Actuele Aanvoer: " + (mainController.GetAcaf()[CurrentWeek]).Value;
            });

            Thread thread = new Thread(ThreadProc);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    private void ThreadProc()
        {
            if(MemoActive == false)
            {
                slideCounter += 1;
            }

            switch (slideCounter)
            {
                case 1:
                    Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new YearGraphPage()); }));
                    break;

                case 2:
                    if(Properties.Settings.Default.CurrentAppVersion == "wacht")
                    {
                        MemoActive = true;
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new MemoPage(ActiveMemo)); }));
                        
                        if (ActiveMemo == mainController.GetMemoCount())
                        {
                            ActiveMemo = 1;
                            MemoActive = false;
                        }

                        if (ActiveMemo == mainController.GetMemoConfig())
                        {
                            MemoActive = false;
                        }

                        ActiveMemo++;
                    }
                    else
                    {
                        NextSlide();
                    }

                    break;

                case 3:
                    Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new UtilityPage()); }));
                    break;

                case 4:
                    Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WeekGraphPage()); }));
                    slideCounter = 0;
                    break;
            }
        }
        private void MainWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            NextSlide();
            if (PauseBtn.IsEnabled == true)
            {
                timer.Start();
                stopwatch.Stop();
                stopwatch.Reset();
                stopwatch.Start();
            }
            
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            stopwatch.Stop();
            timer.Stop();

            timer.Interval = mainController.GetSlideTimer() - stopwatch.ElapsedMilliseconds;

            PauseBtn.IsEnabled = false;
            PlayBtn.IsEnabled = true;
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            stopwatch.Start();
            PauseBtn.IsEnabled = true;
            PlayBtn.IsEnabled = false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            StartWindow startWindow = new StartWindow();
            startWindow.Show();
        }
        private void UpdateOldTimer()
        {
            timer.Interval = mainController.GetSlideTimer();
        }
    }
}