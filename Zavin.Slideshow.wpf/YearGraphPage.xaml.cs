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

            PieGraphLabel.Content = "Verbrand: " + (mainController.GetPie())[0].Value.ToString() + " ton";

            //((PieSeries)PieChart.Series[1]).ItemsSource =

            //    new KeyValuePair<string, int>[]
            //    {
            //        new KeyValuePair<string,int>("Overig",3271)
            //    };

            //((PieSeries)PieChart.Series[2]).ItemsSource =

            //    new KeyValuePair<string, int>[]
            //    {
            //        new KeyValuePair<string,int>("Test",3838)
            //    };
        }
    }
}
