using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.IO;
using System.Windows.Threading;
using System.Net;
using System.Diagnostics;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for UtilityPage.xaml
    /// </summary>
    public partial class UtilityPage : Page
    {
        public System.Timers.Timer Traffictimer;
        public static List<string> titles;
        public static List<string> descs;
        public double TrafficMainHeight;
        public double TrafficBackupHeight;
        public double TrafficMainTop;
        public double TrafficBackupTop;
        public bool Tupdate1 = false;
        public bool Tupdate2 = true;
        Thread trafficThread;

        public UtilityPage()
        {
            Traffictimer = new System.Timers.Timer(10);
            Traffictimer.AutoReset = true;
            Traffictimer.Elapsed += MoveTicker_Tick;

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

            var test = (String.Format("http://maps.weerslag.nl/GratisRadar/1214/910/actueel?zoom=7"));
            wbWeather.Address = test;

            trafficThread = new Thread(GetAndSetTrafficRssMain);
            trafficThread.SetApartmentState(ApartmentState.STA);
            trafficThread.Start();
            
            var descriptor = DependencyPropertyDescriptor.FromProperty(ActualHeightProperty, typeof(StackPanel));
            if (descriptor != null)
                descriptor.AddValueChanged(trafficPanelMain, ActualHeight_ValueChanged);

        }

        private void ActualHeight_ValueChanged(object a_sender, EventArgs a_e)
        {
            TrafficMainHeight = trafficPanelMain.ActualHeight;
            TrafficBackupHeight = trafficPanelBackup.ActualHeight;
            TrafficBackupTop = TrafficMainHeight;
            Canvas.SetTop(trafficPanelBackup, TrafficBackupTop);
        }

        private void GetAndSetTrafficRssMain()
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    XDocument doc = XDocument.Load("http://www.verkeerplaza.nl/rssfeed");
                    titles = (from x in doc.Descendants("item")
                              select x.Element("title").Value).ToList();
                    descs = (from x in doc.Descendants("item")
                             select x.Element("description").Value).ToList();
                    if (!titles.Any())
                    {

                        TextBlock trafficTemp = new TextBlock();
                        trafficTemp.Text = "There is currently no traffic information available.";
                        trafficTemp.FontSize = 20;
                        trafficTemp.Margin = new Thickness(30, 10, 50, 0);
                        trafficPanelMain.Children.Add(trafficTemp);
                    }
                    for (int i = 0; i < titles.Count; i++)
                    {
                        TextBlock trafficTitle = new TextBlock();
                        TextBlock trafficDesc = new TextBlock();
                        string temptext = titles[i];

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
                        trafficDesc.Text = descs[i];
                        trafficDesc.FontSize = 15;
                        trafficDesc.Margin = new Thickness(90, 10, 10, 30);
                        trafficPanelMain.Children.Add(trafficTitle);
                        trafficPanelMain.Children.Add(trafficDesc);
                    }

                    if (titles.Count > 5)
                    {
                        GetAndSetTrafficRssBackup();
                    }
                }
                catch (WebException)
                {
                    TextBlock trafficTemp = new TextBlock();
                    trafficTemp.Text = "couldn't load Rss feed. Maybe check your internet connection?";
                    trafficTemp.FontSize = 20;
                    trafficTemp.Margin = new Thickness(30, 10, 50, 0);
                    trafficPanelMain.Children.Add(trafficTemp);
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
                }
            });
        }


        private void GetAndSetTrafficRssBackup()
        {
            Dispatcher.Invoke(() => {
                XDocument doc = XDocument.Load("http://www.verkeerplaza.nl/rssfeed");
                titles = (from x in doc.Descendants("item")
                          select x.Element("title").Value).ToList();
                descs = (from x in doc.Descendants("item")
                         select x.Element("description").Value).ToList();

                for (int i = 0; i < titles.Count; i++)
                {
                    TextBlock trafficTitle = new TextBlock();
                    TextBlock trafficDesc = new TextBlock();
                    if (titles[i].Contains("Bron: Verkeerplaza.nl: "))
                    {
                        var newTitle = titles[i].Replace("Bron: Verkeerplaza.nl: ", "");
                        trafficTitle.Text = newTitle;
                    }
                    else
                    {
                        trafficTitle.Text = titles[i];
                    }

                    trafficTitle.FontSize = 20;
                    trafficTitle.Margin = new Thickness(30, 10, 50, 0);
                    trafficDesc.Text = descs[i];
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
    }

    //public abstract class BaseConverter : MarkupExtension
    //{
    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return this;
    //    }
    //}
}
