using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for WeekGraphPage.xaml
    /// </summary>
    public partial class WeekGraphPage : Page
    {
        private ObservableCollection<ProductionData> _production = new ObservableCollection<ProductionData>();

        private ObservableCollection<ProductionDataViewModel> _productionViewModel = new ObservableCollection<ProductionDataViewModel>();
        public WeekGraphPage()
        {
            var tmp = mainController.GetProduction();

            foreach (var item in tmp)
            {
                _production.Add(new ProductionData (item.Week, item.Burned, item.Wasta));
                if (item.Burned >= 280)
                {
                    AxisModifier.Maximum = 350;
                }
            }

            foreach (var prod in _production)
            {
                _productionViewModel.Add(new ProductionDataViewModel(prod));
            }


            foreach (var cookie in _productionViewModel)
            {
                Console.WriteLine("HELLO: {0}, {1}, {2}, {3}", cookie.WastaColor, cookie.Production.Productions, cookie.Production.Week, cookie.Production.Wasta);
            }

            InitializeComponent();
        }

        MainController mainController = new MainController();

        private void BarGraphAnimations()
        {

            // Set animation on Bar Graph upon loading of the window.

            //Animation for Production.
            DoubleAnimation moveAnimation = new DoubleAnimation();
            moveAnimation.From = -400;
            moveAnimation.To = ActualHeight;
            moveAnimation.Duration = TimeSpan.FromMilliseconds(6000);
            BarSeriesProductie.BeginAnimation(Canvas.HeightProperty, moveAnimation);

            //Animation for Aanvoer.
            DoubleAnimation moveAnimation2 = new DoubleAnimation();
            moveAnimation2.From = -400;
            moveAnimation2.To = ActualHeight;
            moveAnimation2.Duration = TimeSpan.FromMilliseconds(6000);
            BarSeriesAanvoer.BeginAnimation(Canvas.HeightProperty, moveAnimation2);


        }

        private void WeekGraphPage1_Loaded(object sender, RoutedEventArgs e)
        {
            BarGraphAnimations();

            ((ColumnSeries)MainChart.Series[0]).ItemsSource = _productionViewModel;

            ((ColumnSeries)MainChart.Series[1]).ItemsSource = mainController.GetAcaf();

            //PieGraphLabel.Content = "Verbrand: " + (mainController.GetPie())[0].Value.ToString() + " ton";

            int CurrentWeek = GetCurrentWeek();

            LabelAfgelopenWeek.Content = "Totaal Afgelopen week: " + (mainController.GetProduction()[CurrentWeek - 1].Burned);
            labelHuidigeWeek.Content = "Totaal Huidige week: " + (mainController.GetProduction()[CurrentWeek].Burned);
        }
       
        private int GetCurrentWeek()
        {
            DateTime CurrentDate = DateTime.Now;

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            System.Globalization.Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(CurrentDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }
    }
}
