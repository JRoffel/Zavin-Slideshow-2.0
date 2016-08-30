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
        }
        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            mainController.xDataControl.Rows.Clear();
            mainController.xDataControl.Columns.Clear();
            MainChart.Series.Clear();
            
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
            //MainChart.DataBindTable(convertedTable, "X");
            MainChart.SetBinding(convertedTable, "X");
        }
    }
}
