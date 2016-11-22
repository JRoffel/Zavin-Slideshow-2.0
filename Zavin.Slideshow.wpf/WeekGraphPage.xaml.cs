﻿using System;
using System.Collections.ObjectModel;
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
            InitializeComponent();

            var tmp = mainController.GetProduction(DateTime.Now.Year);

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
        }

        MainController mainController = new MainController();

        private void BarGraphAnimations()
        {

            // Set animation on Bar Graph upon loading of the window.

            //Animation for Production and Aanvoer.
            DoubleAnimation moveAnimation = new DoubleAnimation();
            moveAnimation.From = MainChart.TransformToAncestor(this).Transform(new Point(0, 0)).Y;
            moveAnimation.To = MainChart.ActualHeight / 1.57;
            moveAnimation.Duration = TimeSpan.FromMilliseconds(4000);
            BarSeriesProductie.BeginAnimation(Canvas.HeightProperty, moveAnimation);
            BarSeriesAanvoer.BeginAnimation(Canvas.HeightProperty, moveAnimation);

        }

        private void WeekGraphPage1_Loaded(object sender, RoutedEventArgs e)
        {
            BarGraphAnimations();

            ((ColumnSeries)MainChart.Series[0]).ItemsSource = _productionViewModel;

            ((ColumnSeries)MainChart.Series[1]).ItemsSource = mainController.GetAcaf(DateTime.Now.Year);

            int CurrentWeek = DatabaseController.GetCurrentWeek(DateTime.Now);

            LabelAfgelopenWeek.Content = "Totaal Afgelopen week: " + (mainController.GetProduction(DateTime.Now.Year)[CurrentWeek - 1].Burned);
            labelHuidigeWeek.Content = "Totaal Huidige week: " + (mainController.GetProduction(DateTime.Now.Year)[CurrentWeek].Burned);
        }

        public void UpdateCharts()
        {
            _productionViewModel.Clear();
            _production.Clear();

            var tmp = mainController.GetProduction(DateTime.Now.Year);

            foreach (var item in tmp)
            {
                _production.Add(new ProductionData(item.Week, item.Burned, item.Wasta));
                if (item.Burned >= 280)
                {
                    AxisModifier.Maximum = 350;
                }
            }

            foreach (var prod in _production)
            {
                _productionViewModel.Add(new ProductionDataViewModel(prod));
            }

            WeekGraphPage1_Loaded(this, null);
        }
       
    }
}
