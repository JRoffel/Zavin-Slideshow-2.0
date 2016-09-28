using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zavin.Slideshow.wpf
{
    public class ProductionDataViewModel : UserControl, INotifyPropertyChanged
    {
        public ProductionData Production { get; private set; }

        public Brush WastaColor { get; private set; }

        public ProductionDataViewModel(ProductionData production)
        {
            Production = production;
            Console.WriteLine("{0}, {1}", Production.Week, Production.Wasta);
            if (Production.Wasta == 1)
            {
                WastaColor = Brushes.Yellow;
            }
            else if (Production.Wasta == 2)
            {
                WastaColor = Brushes.Red;
            }
            else
            {
                WastaColor = Brushes.Navy;
            }
            production.PropertyChanged += new PropertyChangedEventHandler(HandleProductionPropertyChanged);
        }

        void HandleProductionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("Wasta" == e.PropertyName)
            {
                if(Production.Wasta == 2)
                {
                    WastaColor = Brushes.DarkRed;
                }
                else if(Production.Wasta == 1)
                {
                    WastaColor = Brushes.LightYellow;
                }
                else
                {
                    WastaColor = Brushes.Navy;
                }
                Helpers.InvokePropertyChanged(PropertyChanged, this, "WastaColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
