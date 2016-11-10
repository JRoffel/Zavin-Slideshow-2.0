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
        public int MemoRemember = 0;
        public bool ShowWelcome = true;
        public System.Timers.Timer pauseTimer = new System.Timers.Timer(300000);
        public Stopwatch pauseWatch = new Stopwatch();
        public System.Timers.Timer updatePauseTime = new System.Timers.Timer(1000);
        public static int CurrentRunTime = 0;
        public static MainWindow mainWindowHolder;

        System.Timers.Timer timer = new System.Timers.Timer(mainController.GetSlideTimer());
        System.Timers.Timer updateTimer = new System.Timers.Timer(300000);
        System.Timers.Timer UpdateRuntime = new System.Timers.Timer(60000);


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
        //safe
        public MainWindow(string version)
        {
            UpdateRuntime.AutoReset = true;
            UpdateRuntime.Elapsed += (sender, e) => Dispatcher.BeginInvoke((Action)(() => { UpdateRuntime_Tick(); }));
            UpdateRuntime.Start();

            mainWindowHolder = this;

            Properties.Settings.Default.CurrentAppVersion = version;
            Properties.Settings.Default.Save();

            pauseTimer.AutoReset = true;
            pauseTimer.Elapsed += (sender, e) => Dispatcher.BeginInvoke((Action)(() => { PlayBtn_Click(sender, null); }));

            updatePauseTime.AutoReset = true;
            updatePauseTime.Elapsed += (sender, e) => Dispatcher.BeginInvoke((Action)(() => { UpdatePauseBar(); }));

            timer.AutoReset = true;
            timer.Elapsed += (sender, e) => NextSlide();
            timer.Start();

            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (sender, e) => UpdateOldTimer();
            updateTimer.Start();

            //Most exceptions propagate till this point, so any uncaught ones that I didn't catch before should end up here... Hopefully...
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            tmr = new System.Timers.Timer(10);
            tmr.AutoReset = true;
            tmr.Elapsed += MoveTicker_Tick;
            tmr.Start();

            stopwatch = new Stopwatch();
            stopwatch.Start();

            if (Properties.Settings.Default.CurrentAppVersion == "kantoor")
            {
                PlayBtn.Visibility = Visibility.Collapsed;
                PauseBtn.Visibility = Visibility.Collapsed;
                NextBtn.Visibility = Visibility.Collapsed;
            }

            int CurrentWeek = DatabaseController.GetCurrentWeek(DateTime.Now);
            ActueleWeekProductie.Text = "Actuele Productie: " + (mainController.GetProduction(DateTime.Now.Year)[CurrentWeek].Burned);
            ActueleWeekAanvoer.Text = "Actuele Aanvoer: " + (mainController.GetAcaf(DateTime.Now.Year)[CurrentWeek]).Value;

            PageFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            GetAndSetRssMain();
            GetAndSetRssBackup();

            var descriptor = DependencyPropertyDescriptor.FromProperty(ActualWidthProperty, typeof(TextBlock));

            if (descriptor != null)
                descriptor.AddValueChanged(HeadlineContainerMain, ActualWidth_ValueChanged);
            
            Canvas2X = Canvas1Width + 1920;
            Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
        }

        //safe
        private void ActualWidth_ValueChanged(object a_sender, EventArgs a_e)
        {
            Canvas1Width = HeadlineContainerMain.ActualWidth;
            Canvas2Width = HeadlineContainerBackup.ActualWidth;
            Canvas2X = Canvas1Width + 1920;
            Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
        }

        //safe
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
                    Canvas1X -= 2.8;
                    Canvas2X -= 2.8;

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
                    Canvas1X -= 2.8;
                    Canvas2X -= 2.8;

                    update2 = true;
                    update1 = false;
                    GetAndSetRssBackup();
                }
                else
                {
                    Canvas.SetLeft(HeadlineContainerMain, Canvas1X);
                    Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
                    Canvas1X -= 2.8;
                    Canvas2X -= 2.8;
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
                    Image nulogo = new Image();
                    nulogo.Source = new BitmapImage(new Uri(@"/images/nulogo.png", UriKind.Relative));
                    HeadlineContainerMain.Children.Add(temptext);
                    HeadlineContainerMain.Children.Add(nulogo);
                }
            }
            catch (WebException)
            {
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
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
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
                        Image nulogo = new Image();
                        nulogo.Source = new BitmapImage(new Uri(@"/images/nulogo.png", UriKind.Relative));
                        HeadlineContainerBackup.Children.Add(temptext);
                        HeadlineContainerBackup.Children.Add(nulogo);
                    }
            }
            catch (WebException)
            {
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
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }
        }

        private void NextSlide()
        {
            try
            {
                stopwatch.Stop();
                stopwatch.Reset();
                stopwatch.Start();

                Thread thread = new Thread(ThreadProc);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }
        }

        //already caught
        private void ThreadProc()
        {
            if(MemoActive == false)
            {
                slideCounter += 1;
            }

            switch (slideCounter)
            {
                case 1:
                    DateTime date = DateTime.Parse(DateTime.Now.Year + "-02-28T00:00:01Z");
                    if (DateTime.Now <= date)
                    {
                        if (ShowWelcome == true && Properties.Settings.Default.CurrentAppVersion == "kantoor" && mainController.HasWelcomePage() == true)
                        {
                            Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WelcomePage()); }));
                            ShowWelcome = false;
                            slideCounter--;
                        }
                        else
                        {
                            Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new OldWeekGraphPage()); }));
                            ShowWelcome = true;
                        }
                    }
                    else
                    {
                        NextSlide();
                    }

                    break;

                case 2:
                    if (ShowWelcome == true && Properties.Settings.Default.CurrentAppVersion == "kantoor" && mainController.HasWelcomePage() == true)
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WelcomePage()); }));
                        ShowWelcome = false;
                        slideCounter--;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new YearGraphPage()); }));
                        ShowWelcome = true;
                    }
                    break;

                case 3: //Luckily, memo page does not require welcome page logic, as it only activates in the 'wacht' version of the application
                    if(Properties.Settings.Default.CurrentAppVersion == "wacht" && mainController.GetMemoCount() > 0)
                    {
                        MemoActive = true;
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new MemoPage(ActiveMemo + MemoRemember)); }));
                        
                        if (ActiveMemo + MemoRemember >= mainController.GetMemoCount())
                        {
                            ActiveMemo = 0;
                            if(MemoRemember > 0)
                            {
                                MemoRemember = 0;
                            }
                            else
                            {
                                MemoActive = false;
                            }
                            
                        }

                        if (ActiveMemo >= mainController.GetMemoConfig())
                        {
                            MemoActive = false;
                            //+= instead of = for increments might be a good solution to problems :/
                            if(mainController.GetMemoCount() > mainController.GetMemoConfig())
                            {
                                MemoRemember += ActiveMemo;
                            }
                            ActiveMemo = 0;
                        }

                        ActiveMemo++;
                    }
                    else
                    {
                        NextSlide();
                    }

                    break;

                case 4:
                    if(ShowWelcome == true && Properties.Settings.Default.CurrentAppVersion == "kantoor" && mainController.HasWelcomePage() == true)
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WelcomePage()); }));
                        ShowWelcome = false;
                        slideCounter--;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new UtilityPage()); }));
                        ShowWelcome = true;
                    }

                    break;

                case 5:
                    if(ShowWelcome == true && Properties.Settings.Default.CurrentAppVersion == "kantoor" && mainController.HasWelcomePage() == true)
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WelcomePage()); }));
                        ShowWelcome = false;
                        slideCounter--;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WeekGraphPage()); }));
                        slideCounter = 0;
                        ShowWelcome = true;
                    }
                    break;
            }
        }
        //safe
        private void MainWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        //already caught
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

        //safe
        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            stopwatch.Stop();
            timer.Stop();
            pauseTimer.Start();
            pauseWatch.Start();
            updatePauseTime.Start();

            timer.Interval = mainController.GetSlideTimer() - stopwatch.ElapsedMilliseconds;

            PauseBtn.Visibility = Visibility.Collapsed;
            PlayBtn.Visibility = Visibility.Visible;
            sldrProgress.Visibility = Visibility.Visible;
        }

        //safe
        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            stopwatch.Start();
            pauseTimer.Stop();
            pauseWatch.Stop();
            pauseWatch.Reset();
            updatePauseTime.Stop();
            sldrProgress.Value = 0;

            sldrProgress.Visibility = Visibility.Collapsed;
            PauseBtn.Visibility = Visibility.Visible;
            PlayBtn.Visibility = Visibility.Collapsed;
        }

        //safe
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            StartWindow startWindow = new StartWindow();
            startWindow.Show();
        }

        //already caught
        private void UpdateOldTimer()
        {
            timer.Interval = mainController.GetSlideTimer();

            int CurrentWeek = DatabaseController.GetCurrentWeek(DateTime.Now);
            Dispatcher.Invoke(() => {
                ActueleWeekProductie.Text = "Actuele Productie: " + (mainController.GetProduction(DateTime.Now.Year)[CurrentWeek].Burned);
                ActueleWeekAanvoer.Text = "Actuele Aanvoer: " + (mainController.GetAcaf(DateTime.Now.Year)[CurrentWeek]).Value;
            });

            timer.Interval = mainController.GetSlideTimer();
        }

        //safe
        private void UpdatePauseBar()
        {
            sldrProgress.Value = (pauseWatch.ElapsedMilliseconds / 1000);
        }

        //safe
        private void UpdateRuntime_Tick()
        {
            CurrentRunTime += 1;
        }
    }
}