using System;
using System.Collections.Generic;
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
        private List<KeyValuePair<string, int>> _pieData;
        public YearGraphPage()
        {
            InitializeComponent();
        }

        private void LoadPieChartData(object sender, RoutedEventArgs e)
        {
            _pieData = _mainController.GetPie();

            ((PieSeries)PieChart.Series[0]).ItemsSource = _pieData;

            var currentWeek = GetCurrentWeek();

            var total = _mainController.GetProdPie();

            PieGraphLabel.Content = "Verbrand: " + total + " ton";

            LabelVerschilAfgelopenWeek.Content = "Verschil t.o.v begroting van de afgelopen week: " + (_mainController.GetLine()[currentWeek - 1].Value);

            LoadLineChartData();
        }

        private void LoadLineChartData()
        {
            var lineList = _mainController.GetLine();
            AxisModifier.Maximum = ((GetHighestInLine(lineList) + 50) < 50) ? 50 : (GetHighestInLine(lineList) + 50);
            AxisModifier.Minimum = ((GetLowestInLine(lineList) - 50) > -50) ? -50 : (GetLowestInLine(lineList) - 50);
            ((LineSeries)mcChart.Series[0]).ItemsSource = lineList;
            ((LineSeries)mcChart.Series[1]).ItemsSource = _mainController.GetZeroLine();
        }
        private static int GetCurrentWeek()
        {
            var currentDate = DateTime.Now;

            var dfi = DateTimeFormatInfo.CurrentInfo;
            // ReSharper disable once PossibleNullReferenceException
            var cal = dfi.Calendar;

            return cal.GetWeekOfYear(currentDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        private int GetHighestInLine(List<KeyValuePair<string, int>> lineList)
        {
            var highest = 0;
            foreach (var line in lineList)
            {
                if (line.Value > highest)
                {
                    highest = line.Value;
                }
            }
            Console.WriteLine(highest);
            return highest;
        }

        private int GetLowestInLine(List<KeyValuePair<string, int>> lineList)
        {
            var lowest = 0;
            foreach (var line in lineList)
            {
                if (line.Value < lowest)
                {
                    lowest = line.Value;
                }
            }

            return lowest;
        }
    }
}
