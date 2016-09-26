using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization.Charting;
using System.Timers;
using System.Threading;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainController mainController = new MainController();
        public int slideCounter = 0;
        public MainWindow()
        {
            System.Timers.Timer timer = new System.Timers.Timer(5000);
            timer.AutoReset = true;
            timer.Elapsed += (sender, e) => NextSlide();
            timer.Start();
            InitializeComponent();
            PageFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
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
                    Dispatcher.Invoke(() => PageFrame.NavigationService.Navigate(new WeekGraphPage()));
                    slideCounter = 0;
                    break;
            }
            
        }
    }
}
