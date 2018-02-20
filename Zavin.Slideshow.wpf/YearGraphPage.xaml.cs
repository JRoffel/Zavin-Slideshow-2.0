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
    // ReSharper disable once RedundantExtendsListEntry
    public partial class YearGraphPage : Page
    {
        private readonly MainController _mainController = new MainController();
        public YearGraphPage()
        {
            InitializeComponent();
        }
        private void LoadPieChartData(object sender, RoutedEventArgs e)
        {
            var pieData = _mainController.GetPie();

            ((PieSeries)PieChart.Series[0]).ItemsSource = pieData;

            var currentWeek = GetCurrentWeek();

            var total = _mainController.GetProdPie();

            PieGraphLabel.Content = "Verbrand: " + total + " ton";

            LabelVerschilAfgelopenWeek.Content = "Verschil t.o.v begroting van de afgelopen week: " + _mainController.GetLine()[currentWeek - 1].Value;

            LoadLineChartData();

        }
        private void LoadLineChartData()
        {
            var lineList = _mainController.GetLine();
            ((LineSeries)mcChart.Series[0]).ItemsSource = lineList;
            ((LineSeries)mcChart.Series[1]).ItemsSource = _mainController.GetZeroLine();
        }
        private int GetCurrentWeek()
        {
            var currentDate = DateTime.Now;

            var dfi = DateTimeFormatInfo.CurrentInfo;
            if (dfi == null) return 0;
            var cal = dfi.Calendar;

            return cal.GetWeekOfYear(currentDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

        }
    }
}
