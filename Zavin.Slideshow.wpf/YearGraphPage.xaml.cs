using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            ((PieSeries)PieChart.Series[0]).ItemsSource = mainController.GetPie();

            int Total = mainController.GetProdPie();
            int CurrentWeek = GetCurrentWeek();

            PieGraphLabel.Content = "Verbrand: " + Total + " ton";

            LabelVerschilAfgelopenWeek.Content = "Verschil t.o.v begroting van de afgelopen week: " + (mainController.GetProduction()[CurrentWeek - 1].Burned);

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
    }
}
