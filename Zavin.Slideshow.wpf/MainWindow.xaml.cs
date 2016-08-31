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

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Cookie = "10,1 20,2";

        MainController mainController = new MainController();

        public MainWindow()
        {
            InitializeComponent();
        }



        public int CookieData
        {
            get { return (int)GetValue(CookieDataProperty); }
            set { SetValue(CookieDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CookieData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CookieDataProperty =
            DependencyProperty.Register("CookieData", typeof(int), typeof(MainController), new PropertyMetadata(0));



        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            mainController.xDataControl.Rows.Clear();
            mainController.xDataControl.Columns.Clear();
            
            mainController.xDataControl.Columns.Add("X", typeof(int));
            mainController.xDataControl.Columns.Add("Productie", typeof(int));
            mainController.xDataControl.Columns.Add("Aanvoer", typeof(int));
            
            for (int i = 1; i < 54; i++)
                            {
                DataRow dataRow = mainController.xDataControl.NewRow();
                
                dataRow["X"] = i;
                dataRow["Productie"] = i * 12 - 10 + 33;
                dataRow["Aanvoer"] = i * 9 - 6 + 24;
                mainController.xDataControl.Rows.Add(dataRow);
            }

            var convertedTable = (mainController.xDataControl as IListSource).GetList();
            MainChart.SetBinding(CookieDataProperty, CookieData);
            //MainChart.DataBindTable(convertedTable, "X");
            //MainChart.SetBinding(convertedTable, "X");
        }
    }
}
