using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using Timer = System.Timers.Timer;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for UtilityPage.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class UtilityPage : Page
    {
        public Timer Traffictimer;
        public static List<string> Titles;
        public static List<string> Descs;
        public double TrafficMainHeight;
        public double TrafficBackupHeight;
        public double TrafficMainTop;
        public double TrafficBackupTop;
        public bool Tupdate1;
        public bool Tupdate2 = true;

        public UtilityPage()
        {
            Traffictimer = new Timer(10) {AutoReset = true};
            Traffictimer.Elapsed += MoveTicker_Tick;

            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                // ReSharper disable once AssignNullToNotNullAttribute
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            const string test = ("http://maps.weerslag.nl/GratisRadar/1214/910/actueel?zoom=7");
            wbWeather.Address = test;

            var trafficThread = new Thread(GetAndSetTrafficRssMain);
            trafficThread.SetApartmentState(ApartmentState.STA);
            trafficThread.Start();
            
            var descriptor = DependencyPropertyDescriptor.FromProperty(ActualHeightProperty, typeof(StackPanel));
            descriptor?.AddValueChanged(trafficPanelMain, ActualHeight_ValueChanged);
        }

        private void ActualHeight_ValueChanged(object aSender, EventArgs aE)
        {
            TrafficMainHeight = trafficPanelMain.ActualHeight;
            TrafficBackupHeight = trafficPanelBackup.ActualHeight;
            TrafficBackupTop = TrafficMainHeight;
            Canvas.SetTop(trafficPanelBackup, TrafficBackupTop);
        }

        public void GetAndSetTrafficRssMain()
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    var doc = XDocument.Load("http://www.verkeerplaza.nl/rssfeed");
                    Titles = (from x in doc.Descendants("item") let xElement = x.Element("title") where xElement != null select xElement.Value).ToList();
                    Descs = (from x in doc.Descendants("item") let element = x.Element("description") where element != null select element.Value).ToList();
                    if (!Titles.Any())
                    {

                        var trafficTemp = new TextBlock
                        {
                            Text = "There is currently no traffic information available.",
                            FontSize = 20,
                            Margin = new Thickness(30, 10, 50, 0)
                        };
                        trafficPanelMain.Children.Add(trafficTemp);
                    }
                    for (var i = 0; i < Titles.Count; i++)
                    {
                        var trafficTitle = new TextBlock();
                        var trafficDesc = new TextBlock();
                        var temptext = Titles[i];

                        if (temptext.Contains("Bron: Verkeerplaza.nl: "))
                        {
                            var newText = temptext.Replace("Bron: Verkeerplaza.nl: ", "");
                            trafficTitle.Text = newText;
                        }
                        else
                        {
                            trafficTitle.Text = temptext;
                        }
                        trafficTitle.FontSize = 20;
                        trafficTitle.Margin = new Thickness(30, 10, 50, 0);
                        trafficDesc.Text = Descs[i];
                        trafficDesc.FontSize = 15;
                        trafficDesc.Margin = new Thickness(90, 10, 10, 30);
                        trafficPanelMain.Children.Add(trafficTitle);
                        trafficPanelMain.Children.Add(trafficDesc);
                    }

                    if (Titles.Count > 5)
                    {
                        GetAndSetTrafficRssBackup();
                    }
                }
                catch (WebException)
                {
                    var trafficTemp = new TextBlock
                    {
                        Text = "couldn't load Rss feed. Maybe check your internet connection?",
                        FontSize = 20,
                        Margin = new Thickness(30, 10, 50, 0)
                    };
                    trafficPanelMain.Children.Add(trafficTemp);
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
                }
            });
        }


        private void GetAndSetTrafficRssBackup()
        {
            Dispatcher.Invoke(() => {
                var doc = XDocument.Load("http://www.verkeerplaza.nl/rssfeed");
                Titles = (from x in doc.Descendants("item") let xElement = x.Element("title") where xElement != null select xElement.Value).ToList();
                Descs = (from x in doc.Descendants("item") let element = x.Element("description") where element != null select element.Value).ToList();

                for (var i = 0; i < Titles.Count; i++)
                {
                    var trafficTitle = new TextBlock();
                    var trafficDesc = new TextBlock();
                    if (Titles[i].Contains("Bron: Verkeerplaza.nl: "))
                    {
                        var newTitle = Titles[i].Replace("Bron: Verkeerplaza.nl: ", "");
                        trafficTitle.Text = newTitle;
                    }
                    else
                    {
                        trafficTitle.Text = Titles[i];
                    }

                    trafficTitle.FontSize = 20;
                    trafficTitle.Margin = new Thickness(30, 10, 50, 0);
                    trafficDesc.Text = Descs[i];
                    trafficDesc.FontSize = 15;
                    trafficDesc.Margin = new Thickness(90, 10, 10, 30);
                    trafficPanelBackup.Children.Add(trafficTitle);
                    trafficPanelBackup.Children.Add(trafficDesc);
                }
            });
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
                    TrafficMainTop -= 1.1;
                    TrafficBackupTop -= 1.1;
                    
                    Tupdate1 = true;
                    Tupdate2 = false;
                }
                else if (TrafficMainTop < 0 && Tupdate2 == false)
                {
                    TrafficMainHeight = Convert.ToInt32(trafficPanelMain.ActualHeight);
                    TrafficBackupTop = TrafficMainHeight;
                    Canvas.SetTop(trafficPanelMain, TrafficMainTop);
                    Canvas.SetTop(trafficPanelBackup, TrafficBackupTop);
                    TrafficMainTop -= 1.1;
                    TrafficBackupTop -= 1.1;


                    Tupdate2 = true;
                    Tupdate1 = false;
                }
                else
                {
                    Canvas.SetTop(trafficPanelMain, TrafficMainTop);
                    Canvas.SetTop(trafficPanelBackup, TrafficBackupTop);
                    TrafficMainTop -= 1.1;
                    TrafficBackupTop -= 1.1;
                }
            });
        }

        public void UpdateWeatherChart()
        {
            try
            {
                wbWeather.ReloadCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(@"Browser not loaded, assume up to date");
            }

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
