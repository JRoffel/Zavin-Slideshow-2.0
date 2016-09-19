using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Zavin.Slideshow.wpf
{
    public class ProductionDataViewModel : INotifyPropertyChanged
    {
        public ProductionData Production { get; private set; }

        public Brush WastaColor { get; private set; }

        public ProductionDataViewModel(ProductionData production)
        {
            Production = production;
            production.PropertyChanged += new PropertyChangedEventHandler(HandleProductionPropertyChanged);
        }

        void HandleProductionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("Wasta" == e.PropertyName)
            {
                if (Production.Wasta == 0)
                {
                    WastaColor = new SolidColorBrush { Color = Colors.Navy };
                }
                else if(Production.Wasta == 1)
                {
                    WastaColor = new SolidColorBrush { Color = Colors.Yellow };
                }
                else if (Production.Wasta == 2)
                {
                    WastaColor = new SolidColorBrush { Color = Colors.Red };
                }
                else
                {
                    WastaColor = new SolidColorBrush { Color = Colors.Blue };
                }
                Helpers.InvokePropertyChanged(PropertyChanged, this, "WastaColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
