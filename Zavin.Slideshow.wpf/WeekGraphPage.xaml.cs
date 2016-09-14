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
        public WeekGraphPage()
        {
            InitializeComponent();
        }

        MainController mainController = new MainController();

        private void WeekGraphPage1_Loaded(object sender, RoutedEventArgs e)
        {

            ((ColumnSeries)MainChart.Series[0]).ItemsSource = mainController.GetProduction();

            ((ColumnSeries)MainChart.Series[1]).ItemsSource = mainController.GetAcaf();

            WeekGraphColorAnimations();
        }

        private void WeekGraphColorAnimations()
        {
            //Color Animations for Productie
            //ColorAnimation AddColorAnimation = new ColorAnimation();
            //AddColorAnimation.To = Colors.Red;
            //AddColorAnimation.Duration = TimeSpan.FromSeconds(3);
            //MainChart.BeginAnimation(SolidColorBrush.ColorProperty, AddColorAnimation);

            //Color Animations for Aanvoer
            //DoubleAnimation moveAnimation = new DoubleAnimation();
            //AddColorAnimation.From = 0;
            //AddColorAnimation.To = ActualHeight;
            //AddColorAnimation.Duration = TimeSpan.FromSeconds(3);
            //BarSeriesProductie.BeginAnimation(Canvas.HeightProperty, moveAnimation);

        }
    }
}
