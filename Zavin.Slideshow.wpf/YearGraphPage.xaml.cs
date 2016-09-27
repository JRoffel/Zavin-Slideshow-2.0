using System;
using System.Collections.Generic;
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

            PieGraphLabel.Content = "Verbrand: " + Total + " ton";

            var LineList = mainController.GetLine();

            foreach (var LineItem in LineList)
            {
                if (LineItem.Value < (AxisModifier.Minimum + 10))
                {
                    AxisModifier.Minimum = (LineItem.Value - 10);
                }
            }

            ((LineSeries)lineChart.Series[0]).ItemsSource = LineList;

            ((LineSeries)lineChart.Series[1]).ItemsSource = mainController.GetZeroLine();

        }
    }
}
