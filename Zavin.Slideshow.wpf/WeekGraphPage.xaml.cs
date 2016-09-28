﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media.Animation;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for WeekGraphPage.xaml
    /// </summary>
    public partial class WeekGraphPage : Page
    {
        private ObservableCollection<ProductionData> _production = new ObservableCollection<ProductionData>();

        private ObservableCollection<ProductionDataViewModel> _productionViewModel = new ObservableCollection<ProductionDataViewModel>();
        public WeekGraphPage()
        {
            var tmp = mainController.GetProduction();

            foreach (var item in tmp)
            {
                _production.Add(new ProductionData (item.Week, item.Burned, item.Wasta));
                if (item.Burned >= 280)
                {
                    AxisModifier.Maximum = 350;
                }
            }

            foreach (var prod in _production)
            {
                _productionViewModel.Add(new ProductionDataViewModel(prod));
            }


            foreach (var cookie in _productionViewModel)
            {
                Console.WriteLine("HELLO: {0}, {1}, {2}, {3}", cookie.WastaColor, cookie.Production.Productions, cookie.Production.Week, cookie.Production.Wasta);
            }

            InitializeComponent();
        }

        MainController mainController = new MainController();

        private void BarGraphAnimations()
        {

            // Set animation on Bar Graph upon loading of the window.

            //Animation for Production.
            DoubleAnimation moveAnimation = new DoubleAnimation();
            moveAnimation.From = 0;
            moveAnimation.To = MainChart.ActualHeight / 2;
            moveAnimation.Duration = TimeSpan.FromMilliseconds(4000);
            BarSeriesProductie.BeginAnimation(Canvas.HeightProperty, moveAnimation);

            //Animation for Aanvoer.
            DoubleAnimation moveAnimation2 = new DoubleAnimation();
            moveAnimation2.From = 0;
            moveAnimation2.To = MainChart.ActualHeight / 2;
            moveAnimation2.Duration = TimeSpan.FromMilliseconds(4000);
            BarSeriesAanvoer.BeginAnimation(Canvas.HeightProperty, moveAnimation2);

        }

        private void WeekGraphPage1_Loaded(object sender, RoutedEventArgs e)
        {
            BarGraphAnimations();

            ((ColumnSeries)MainChart.Series[0]).ItemsSource = _productionViewModel;

            ((ColumnSeries)MainChart.Series[1]).ItemsSource = mainController.GetAcaf();

            int CurrentWeek = GetCurrentWeek();

            LabelAfgelopenWeek.Content = "Totaal Afgelopen week: " + (mainController.GetProduction()[CurrentWeek - 1].Burned);
            labelHuidigeWeek.Content = "Totaal Huidige week: " + (mainController.GetProduction()[CurrentWeek].Burned);
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
