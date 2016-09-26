using System;
using System.Net;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls.DataVisualization.Charting;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainController mainController = new MainController();

        public MainWindow()
        {
            InitializeComponent();
            int bySizeTemp = Convert.ToInt32(headlines.ActualWidth);
            int bySize = bySizeTemp - bySizeTemp * 2;
            test.Text = bySizeTemp.ToString();

            for (int i = 0; i < headlines.Items.Count; i++)
            {

                ContentPresenter c = (ContentPresenter)headlines.ItemContainerGenerator.ContainerFromItem(headlines.Items[i]);
                TextBlock tb = c.ContentTemplate.FindName("pancake", c) as TextBlock;

                MessageBox.Show("  pancake  "
    + tb.Text);
            }

            //DoubleAnimation slide = new DoubleAnimation();
            //slide.From = 1920;
            //slide.By = bySize;
            //slide.Duration = TimeSpan.FromMilliseconds(4000);
            //headlinetitles.BeginAnimation(Canvas.WidthProperty, slide);


        }

        private void MainWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        

}
}
