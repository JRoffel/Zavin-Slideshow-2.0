﻿using System;
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
        MainController mainController = new MainController();

        public MainWindow()
        {
            InitializeComponent();
        }

        Random random = new Random();

        public object NavigationService { get; private set; }

        //public int CookieData
        //{
        //    get { return (int)GetValue(CookieDataProperty); }
        //    set { SetValue(CookieDataProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for CookieData.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty CookieDataProperty =
        //    DependencyProperty.Register("CookieData", typeof(int), typeof(MainController), new PropertyMetadata(0));



        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            //mainController.xDataControl.Rows.Clear();
            //mainController.xDataControl.Columns.Clear();

            //mainController.xDataControl.Columns.Add("X", typeof(int));
            //mainController.xDataControl.Columns.Add("Productie", typeof(int));
            //mainController.xDataControl.Columns.Add("Aanvoer", typeof(int));

            //for (int i = 1; i < 54; i++)
            //                {
            //    DataRow dataRow = mainController.xDataControl.NewRow();

            //    dataRow["X"] = i;
            //    dataRow["Productie"] = i * 12 - 10 + 33;
            //    dataRow["Aanvoer"] = i * 9 - 6 + 24;
            //    mainController.xDataControl.Rows.Add(dataRow);
            //}

            //var convertedTable = (mainController.xDataControl as IListSource).GetList();
            //MainChart.SetBinding(CookieDataProperty, CookieData);
            ////MainChart.DataBindTable(convertedTable, "X");
            ////MainChart.SetBinding(convertedTable, "X");

            ((ColumnSeries)MainChart.Series[0]).ItemsSource =
                new KeyValuePair<string, int>[]
                {
                    new KeyValuePair<string, int>("1", random.Next(100, 300)),
                    new KeyValuePair<string, int>("2", random.Next(100, 300)),
                    new KeyValuePair<string, int>("3", random.Next(100, 300)),
                    new KeyValuePair<string, int>("4", random.Next(100, 300)),
                    new KeyValuePair<string, int>("5", random.Next(100, 300)),
                    new KeyValuePair<string, int>("6", random.Next(100, 300)),
                    new KeyValuePair<string, int>("7", random.Next(100, 300)),
                    new KeyValuePair<string, int>("8", random.Next(100, 300)),
                    new KeyValuePair<string, int>("9", random.Next(100, 300)),
                    new KeyValuePair<string, int>("10", random.Next(100, 300)),
                    new KeyValuePair<string, int>("11", random.Next(100, 300)),
                    new KeyValuePair<string, int>("12", random.Next(100, 300)),
                    new KeyValuePair<string, int>("13", random.Next(100, 300)),
                    new KeyValuePair<string, int>("14", random.Next(100, 300)),
                    new KeyValuePair<string, int>("15", random.Next(100, 300)),
                    new KeyValuePair<string, int>("16", random.Next(100, 300)),
                    new KeyValuePair<string, int>("17", random.Next(100, 300)),
                    new KeyValuePair<string, int>("18", random.Next(100, 300)),
                    new KeyValuePair<string, int>("19", random.Next(100, 300)),
                    new KeyValuePair<string, int>("20", random.Next(100, 300)),
                    new KeyValuePair<string, int>("21", random.Next(100, 300)),
                    new KeyValuePair<string, int>("22", random.Next(100, 300)),
                    new KeyValuePair<string, int>("23", random.Next(100, 300)),
                    new KeyValuePair<string, int>("24", random.Next(100, 300)),
                    new KeyValuePair<string, int>("25", random.Next(100, 300)),
                    new KeyValuePair<string, int>("26", random.Next(100, 300)),
                    new KeyValuePair<string, int>("27", random.Next(100, 300)),
                    new KeyValuePair<string, int>("28", random.Next(100, 300)),
                    new KeyValuePair<string, int>("29", random.Next(100, 300)),
                    new KeyValuePair<string, int>("30", random.Next(100, 300)),
                    new KeyValuePair<string, int>("31", random.Next(100, 300)),
                    new KeyValuePair<string, int>("32", random.Next(100, 300)),
                    new KeyValuePair<string, int>("33", random.Next(100, 300)),
                    new KeyValuePair<string, int>("34", random.Next(100, 300)),
                    new KeyValuePair<string, int>("35", random.Next(100, 300)),
                    new KeyValuePair<string, int>("36", random.Next(100, 300)),
                    new KeyValuePair<string, int>("37", random.Next(100, 300)),
                    new KeyValuePair<string, int>("38", 0),
                    new KeyValuePair<string, int>("39", 0),
                    new KeyValuePair<string, int>("40", 0),
                    new KeyValuePair<string, int>("41", 0),
                    new KeyValuePair<string, int>("42", 0),
                    new KeyValuePair<string, int>("43", 0),
                    new KeyValuePair<string, int>("44", 0),
                    new KeyValuePair<string, int>("45", 0),
                    new KeyValuePair<string, int>("46", 0),
                    new KeyValuePair<string, int>("47", 0),
                    new KeyValuePair<string, int>("48", 0),
                    new KeyValuePair<string, int>("49", 0),
                    new KeyValuePair<string, int>("50", 0),
                    new KeyValuePair<string, int>("51", 0),
                    new KeyValuePair<string, int>("52", 0),
                    new KeyValuePair<string, int>("53", 0)
                };

            ((ColumnSeries)MainChart.Series[1]).ItemsSource =
                new KeyValuePair<string, int>[]
                {
                    new KeyValuePair<string, int>("1", random.Next(100, 300)),
                    new KeyValuePair<string, int>("2", random.Next(100, 300)),
                    new KeyValuePair<string, int>("3", random.Next(100, 300)),
                    new KeyValuePair<string, int>("4", random.Next(100, 300)),
                    new KeyValuePair<string, int>("5", random.Next(100, 300)),
                    new KeyValuePair<string, int>("6", random.Next(100, 300)),
                    new KeyValuePair<string, int>("7", random.Next(100, 300)),
                    new KeyValuePair<string, int>("8", random.Next(100, 300)),
                    new KeyValuePair<string, int>("9", random.Next(100, 300)),
                    new KeyValuePair<string, int>("10", random.Next(100, 300)),
                    new KeyValuePair<string, int>("11", random.Next(100, 300)),
                    new KeyValuePair<string, int>("12", random.Next(100, 300)),
                    new KeyValuePair<string, int>("13", random.Next(100, 300)),
                    new KeyValuePair<string, int>("14", random.Next(100, 300)),
                    new KeyValuePair<string, int>("15", random.Next(100, 300)),
                    new KeyValuePair<string, int>("16", random.Next(100, 300)),
                    new KeyValuePair<string, int>("17", random.Next(100, 300)),
                    new KeyValuePair<string, int>("18", random.Next(100, 300)),
                    new KeyValuePair<string, int>("19", random.Next(100, 300)),
                    new KeyValuePair<string, int>("20", random.Next(100, 300)),
                    new KeyValuePair<string, int>("21", random.Next(100, 300)),
                    new KeyValuePair<string, int>("22", random.Next(100, 300)),
                    new KeyValuePair<string, int>("23", random.Next(100, 300)),
                    new KeyValuePair<string, int>("24", random.Next(100, 300)),
                    new KeyValuePair<string, int>("25", random.Next(100, 300)),
                    new KeyValuePair<string, int>("26", random.Next(100, 300)),
                    new KeyValuePair<string, int>("27", random.Next(100, 300)),
                    new KeyValuePair<string, int>("28", random.Next(100, 300)),
                    new KeyValuePair<string, int>("29", random.Next(100, 300)),
                    new KeyValuePair<string, int>("30", random.Next(100, 300)),
                    new KeyValuePair<string, int>("31", random.Next(100, 300)),
                    new KeyValuePair<string, int>("32", random.Next(100, 300)),
                    new KeyValuePair<string, int>("33", random.Next(100, 300)),
                    new KeyValuePair<string, int>("34", random.Next(100, 300)),
                    new KeyValuePair<string, int>("35", random.Next(100, 300)),
                    new KeyValuePair<string, int>("36", random.Next(100, 300)),
                    new KeyValuePair<string, int>("37", random.Next(100, 300)),
                    new KeyValuePair<string, int>("38", 0),
                    new KeyValuePair<string, int>("39", 0),
                    new KeyValuePair<string, int>("40", 0),
                    new KeyValuePair<string, int>("41", 0),
                    new KeyValuePair<string, int>("42", 0),
                    new KeyValuePair<string, int>("43", 0),
                    new KeyValuePair<string, int>("44", 0),
                    new KeyValuePair<string, int>("45", 0),
                    new KeyValuePair<string, int>("46", 0),
                    new KeyValuePair<string, int>("47", 0),
                    new KeyValuePair<string, int>("48", 0),
                    new KeyValuePair<string, int>("49", 0),
                    new KeyValuePair<string, int>("50", 0),
                    new KeyValuePair<string, int>("51", 0),
                    new KeyValuePair<string, int>("52", 0),
                    new KeyValuePair<string, int>("53", 0)
                };
            Graph_Resize();
        }

        private void Graph_Resize()
        {
            MainChart.Width = MainChart.ActualWidth - 50;
            MainChart.Height = MainChart.ActualHeight - 70;
            //ChartingSeries.Height = ChartingSeries.ActualHeight - 70;
           
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
