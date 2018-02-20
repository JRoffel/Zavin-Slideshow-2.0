using System;
using System.Collections.Generic;
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
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : Window
    {
        public static MainController MainController = new MainController();

        public string CombinedString;
        public static List<string> Items;
        public static List<string> NewItems = new List<string>();
        public bool MemoActive;
        public int ActiveMemo = 1;
        public int MemoRemember;
        public bool ShowWelcome = true;
        public System.Timers.Timer PauseTimer = new System.Timers.Timer(300000);
        public Stopwatch PauseWatch = new Stopwatch();
        public System.Timers.Timer UpdatePauseTime = new System.Timers.Timer(1000);
        public static int CurrentRunTime;
        public static MainWindow MainWindowHolder;

        private readonly System.Timers.Timer _timer = new System.Timers.Timer(MainController.GetSlideTimer());
        private readonly System.Timers.Timer _updateTimer = new System.Timers.Timer(300000);
        private readonly System.Timers.Timer _updateRuntime = new System.Timers.Timer(60000);


        public DispatcherTimer NextSlideTimer = new DispatcherTimer();
        public System.Timers.Timer Tmr;
        public Stopwatch Stopwatch;
        public double Canvas1X = 1920;
        public double Canvas2X;
        public double CanvasLogo1X;
        public double CanvasLogo2X;
        public double Canvas1Width;
        public double Canvas2Width;
        public bool Update1;
        public bool Update2 = true;
        public int RequestWaitMain = 30;
        public int RequestWaitBackup = 30;

        public int SlideCounter;
        //safe
        public MainWindow(string version)
        {
            _updateRuntime.AutoReset = true;
            _updateRuntime.Elapsed += (sender, e) => Dispatcher.BeginInvoke((Action)UpdateRuntime_Tick);
            _updateRuntime.Start();

            MainWindowHolder = this;

            Properties.Settings.Default.CurrentAppVersion = version;
            Properties.Settings.Default.Save();

            PauseTimer.AutoReset = true;
            PauseTimer.Elapsed += (sender, e) => Dispatcher.BeginInvoke((Action)(() => { PlayBtn_Click(sender, null); }));

            UpdatePauseTime.AutoReset = true;
            UpdatePauseTime.Elapsed += (sender, e) => Dispatcher.BeginInvoke((Action)UpdatePauseBar);

            _timer.AutoReset = true;
            _timer.Elapsed += (sender, e) => NextSlide();
            _timer.Start();

            _updateTimer.AutoReset = true;
            _updateTimer.Elapsed += (sender, e) => UpdateOldTimer();
            _updateTimer.Start();

            //Most exceptions propagate till this point, so any uncaught ones that I didn't catch before should end up here... Hopefully...
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { MainController.SendErrorMessage(ex); });
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            Tmr = new System.Timers.Timer(10) {AutoReset = true};
            Tmr.Elapsed += MoveTicker_Tick;
            Tmr.Start();

            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            if (Properties.Settings.Default.CurrentAppVersion == "kantoor")
            {
                PlayBtn.Visibility = Visibility.Collapsed;
                PauseBtn.Visibility = Visibility.Collapsed;
                NextBtn.Visibility = Visibility.Collapsed;
            }

            var currentWeek = DatabaseController.GetCurrentWeek(DateTime.Now);
            ActueleWeekProductie.Text = "Actuele Productie: " + MainController.GetProduction(DateTime.Now.Year, false)[currentWeek].Burned;
            ActueleWeekAanvoer.Text = "Actuele Aanvoer: " + MainController.GetAcaf(DateTime.Now.Year, false)[currentWeek].Value;

            PageFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            GetAndSetRssMain();
            GetAndSetRssBackup();

            var descriptor = DependencyPropertyDescriptor.FromProperty(ActualWidthProperty, typeof(TextBlock));

            descriptor?.AddValueChanged(HeadlineContainerMain, ActualWidth_ValueChanged);

            Canvas2X = Canvas1Width + 1920;
            Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
        }

        //safe
        private void ActualWidth_ValueChanged(object aSender, EventArgs aE)
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
                if (Canvas2X < 0 && Update1 == false)
                {
                    Canvas2Width = Convert.ToInt32(HeadlineContainerBackup.ActualWidth);
                    Canvas1X = Canvas2Width;
                    Canvas.SetLeft(HeadlineContainerMain, Canvas1X);
                    Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
                    Canvas1X -= 2.8;
                    Canvas2X -= 2.8;

                    Update1 = true;
                    Update2 = false;
                    GetAndSetRssMain();
                }
                else if (Canvas1X < 0 && Update2 == false)
                {
                    Canvas1Width = Convert.ToInt32(HeadlineContainerMain.ActualWidth);
                    Canvas2X = Canvas1Width;
                    Canvas.SetLeft(HeadlineContainerMain, Canvas1X);
                    Canvas.SetLeft(HeadlineContainerBackup, Canvas2X);
                    Canvas1X -= 2.8;
                    Canvas2X -= 2.8;

                    Update2 = true;
                    Update1 = false;
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
                    var doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");

                    Items = (from x in doc.Descendants("item")
                             select x.Element("title")?.Value).ToList();
                    
                    RequestWaitMain = 0;
                }
                else
                {
                    RequestWaitMain++;
                }
                HeadlineContainerMain.Children.Clear();
                foreach (var item in Items)
                {
                    var temptext = new TextBlock
                    {
                        Text = "  " + item + "  ",
                        FontSize = 25,
                        Foreground = new SolidColorBrush(Colors.Navy),
                        FontWeight = FontWeights.Bold
                    };
                    var thickness = new Thickness {Top = 5};
                    temptext.Margin = thickness;
                    var nulogo = new Image {Source = new BitmapImage(new Uri(@"/images/nulogo.png", UriKind.Relative))};
                    HeadlineContainerMain.Children.Add(temptext);
                    HeadlineContainerMain.Children.Add(nulogo);
                }
            }
            catch (WebException)
            {
                CombinedString = "Could not get RSS feed, you might not have an internet connection, or nu.nl might be down, we will keep trying every 30 seconds  -  ";
                HeadlineContainerMain.Children.Clear();
                var temptext = new TextBlock
                {
                    Text = CombinedString,
                    FontSize = 25,
                    Foreground = new SolidColorBrush(Colors.Navy),
                    FontWeight = FontWeights.Bold
                };
                var thickness = new Thickness {Top = 5};
                temptext.Margin = thickness;
                temptext.Text = CombinedString;
                HeadlineContainerMain.Children.Add(temptext);
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { MainController.SendErrorMessage(ex); });
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
                    var doc = XDocument.Load("http://www.nu.nl/rss/Algemeen");
                    
                    Items = (from x in doc.Descendants("item")
                             select x.Element("title")?.Value).ToList();
                  
                    RequestWaitBackup = 0;
                }
                else
                {
                    RequestWaitBackup++;
                }

                HeadlineContainerBackup.Children.Clear();
                foreach (var item in Items)
                    {
                        var temptext = new TextBlock
                        {
                            Text = "  " + item + "  ",
                            FontSize = 25,
                            Foreground = new SolidColorBrush(Colors.Navy),
                            FontWeight = FontWeights.Bold
                        };
                        var thickness = new Thickness {Top = 5};
                        temptext.Margin = thickness;
                        var nulogo = new Image
                        {
                            Source = new BitmapImage(new Uri(@"/images/nulogo.png", UriKind.Relative))
                        };
                        HeadlineContainerBackup.Children.Add(temptext);
                        HeadlineContainerBackup.Children.Add(nulogo);
                    }
            }
            catch (WebException)
            {
                CombinedString = "Could not get RSS feed, you might not have an internet connection, or nu.nl might be down, we will keep trying every 30 seconds  -  ";
                HeadlineContainerMain.Children.Clear();
                var temptext = new TextBlock
                {
                    Text = CombinedString,
                    FontSize = 25,
                    Foreground = new SolidColorBrush(Colors.Navy),
                    FontWeight = FontWeights.Bold
                };
                var thickness = new Thickness {Top = 5};
                temptext.Margin = thickness;
                temptext.Text = CombinedString;
                HeadlineContainerBackup.Children.Add(temptext);
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { MainController.SendErrorMessage(ex); });
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }
        }

        private void NextSlide()
        {
            try
            {
                Stopwatch.Stop();
                Stopwatch.Reset();
                Stopwatch.Start();

                var thread = new Thread(ThreadProc);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { MainController.SendErrorMessage(ex); });
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }
        }

        //already caught
        private void ThreadProc()
        {
            if(MemoActive == false)
            {
                SlideCounter += 1;
            }

            switch (SlideCounter)
            {
                case 1:
                    var date = DateTime.Parse(DateTime.Now.Year + "-02-28T00:00:01Z");
                    if (DateTime.Now <= date)
                    {
                        if (ShowWelcome && Properties.Settings.Default.CurrentAppVersion == "kantoor" && MainController.HasWelcomePage())
                        {
                            Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WelcomePage()); }));
                            ShowWelcome = false;
                            SlideCounter--;
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
                    if (ShowWelcome && Properties.Settings.Default.CurrentAppVersion == "kantoor" && MainController.HasWelcomePage())
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WelcomePage()); }));
                        ShowWelcome = false;
                        SlideCounter--;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new YearGraphPage()); }));
                        ShowWelcome = true;
                    }
                    break;

                case 3:
                    if (ShowWelcome && Properties.Settings.Default.CurrentAppVersion == "kantoor" && MainController.HasWelcomePage())
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WelcomePage()); }));
                        ShowWelcome = false;
                        SlideCounter--;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WeekLacalGraph()); }));
                        ShowWelcome = true;
                    }
                    break;

                case 4: //Luckily, memo page does not require welcome page logic, as it only activates in the 'wacht' version of the application
                    if(Properties.Settings.Default.CurrentAppVersion == "wacht" && MainController.GetMemoCount() > 0)
                    {
                        MemoActive = true;
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new MemoPage(ActiveMemo + MemoRemember)); }));
                        
                        if (ActiveMemo + MemoRemember >= MainController.GetMemoCount())
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

                        if (ActiveMemo >= MainController.GetMemoConfig())
                        {
                            MemoActive = false;
                            //+= instead of = for increments might be a good solution to problems :/
                            if(MainController.GetMemoCount() > MainController.GetMemoConfig())
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

                case 5:
                    if(ShowWelcome && Properties.Settings.Default.CurrentAppVersion == "kantoor" && MainController.HasWelcomePage())
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WelcomePage()); }));
                        ShowWelcome = false;
                        SlideCounter--;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new UtilityPage()); }));
                        ShowWelcome = true;
                    }

                    break;

                case 6:
                    if(ShowWelcome && Properties.Settings.Default.CurrentAppVersion == "kantoor" && MainController.HasWelcomePage())
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WelcomePage()); }));
                        ShowWelcome = false;
                        SlideCounter--;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke((Action)(() => { PageFrame.NavigationService.Navigate(new WeekGraphPage()); }));
                        SlideCounter = 0;
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
                Close();
            }
        }

        //already caught
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            NextSlide();
            if (PauseBtn.IsEnabled)
            {
                _timer.Start();
                Stopwatch.Stop();
                Stopwatch.Reset();
                Stopwatch.Start();
            }
            
        }

        //safe
        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch.Stop();
            _timer.Stop();
            PauseTimer.Start();
            PauseWatch.Start();
            UpdatePauseTime.Start();

            _timer.Interval = MainController.GetSlideTimer() - Stopwatch.ElapsedMilliseconds;

            PauseBtn.Visibility = Visibility.Collapsed;
            PlayBtn.Visibility = Visibility.Visible;
            sldrProgress.Visibility = Visibility.Visible;
        }

        //safe
        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
            Stopwatch.Start();
            PauseTimer.Stop();
            PauseWatch.Stop();
            PauseWatch.Reset();
            UpdatePauseTime.Stop();
            sldrProgress.Value = 0;

            sldrProgress.Visibility = Visibility.Collapsed;
            PauseBtn.Visibility = Visibility.Visible;
            PlayBtn.Visibility = Visibility.Collapsed;
        }

        //safe
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var startWindow = new StartWindow();
            startWindow.Show();
        }

        //already caught
        private void UpdateOldTimer()
        {
            _timer.Interval = MainController.GetSlideTimer();

            var currentWeek = DatabaseController.GetCurrentWeek(DateTime.Now);
            Dispatcher.Invoke(() => {
                ActueleWeekProductie.Text = "Actuele Productie: " + MainController.GetProduction(DateTime.Now.Year, false)[currentWeek].Burned;
                ActueleWeekAanvoer.Text = "Actuele Aanvoer: " + MainController.GetAcaf(DateTime.Now.Year, false)[currentWeek].Value;
            });

            _timer.Interval = MainController.GetSlideTimer();
        }

        //safe
        private void UpdatePauseBar()
        {
            if (PauseWatch != null)
            {
                // ReSharper disable once PossibleLossOfFraction
                sldrProgress.Value = PauseWatch.ElapsedMilliseconds / 1000;
            }
        }

        //safe
        private void UpdateRuntime_Tick()
        {
            CurrentRunTime += 1;
        }
    }
}