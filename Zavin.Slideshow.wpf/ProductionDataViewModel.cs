using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zavin.Slideshow.wpf
{
    public class ProductionDataViewModel : UserControl, INotifyPropertyChanged
    {
        public ProductionData Production { get; }

        public Brush WastaColor { get; private set; }

        public ProductionDataViewModel(ProductionData production)
        {
            Production = production;
            if (Production.Wasta == 1)
            {
                WastaColor = Brushes.Yellow;
            }
            else if (Production.Wasta == 2)
            {
               WastaColor = Brushes.Red;
            }
            else if (Production.Wasta == 3)
            {
                WastaColor = Brushes.Purple;
            }
            else
            {
                WastaColor = Brushes.Navy;
            }
            production.PropertyChanged += HandleProductionPropertyChanged;
        }

        void HandleProductionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("Wasta" == e.PropertyName)
            {
                if(Production.Wasta == 2)
                {
                    WastaColor = Brushes.Red;
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
