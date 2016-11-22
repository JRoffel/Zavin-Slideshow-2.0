using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for YearGraphPage.xaml
    /// </summary>
    public partial class YearGraphPage : Page
    {
        MainController mainController = new MainController();
        public YearGraphPage()
        {
            InitializeComponent();
        }

        private void LoadPieChartData(object sender, RoutedEventArgs e)
        {
            var PieData = mainController.GetPie();

            ((PieSeries)PieChart.Series[0]).ItemsSource = PieData;

            int CurrentWeek = GetCurrentWeek();

            int Total = mainController.GetProdPie();

            PieGraphLabel.Content = "Verbrand: " + Total + " ton";

            LabelVerschilAfgelopenWeek.Content = "Verschil t.o.v begroting van de afgelopen week: " + (mainController.GetLine()[CurrentWeek - 1].Value);

            LoadLineChartData();
        }

        private void LoadLineChartData()
        {
            var LineList = mainController.GetLine();
            ((LineSeries)mcChart.Series[0]).ItemsSource = LineList;
            ((LineSeries)mcChart.Series[1]).ItemsSource = mainController.GetZeroLine();
        }
        private int GetCurrentWeek()
        {
            DateTime CurrentDate = DateTime.Now;

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            System.Globalization.Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(CurrentDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public void UpdateCharts()
        {
            //((PieSeries)PieChart.Series[0]).ItemsSource = null;
            //Dispatcher.Invoke(() => { LoadPieChartData(YearGraphPage1, null); });
            //PieChart.UpdateLayout();

            //((PieSeries)PieChart.Series[0]).ItemsSource = mainController.GetPie();
        }
    }
}
