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
    public class ProductionData : UserControl, INotifyPropertyChanged
    {
        public string Week { get; private set; }

        public Brush Color { get; private set; }

        public int Wasta
        {
            get { return _wasta; }
            set
            {
                _wasta = value;
                Helpers.InvokePropertyChanged(PropertyChanged, this, "Wasta");
            }
        }

        private int _wasta;

        public int Productions
        {
            get { return _production; }
            set
            {
                _production = value;
                Helpers.InvokePropertyChanged(PropertyChanged, this, "Production");
            }
        }

        private int _production;

        public ProductionData(string week, int production, int wasta)
        {
            Week = week;
            Productions = production;
            Wasta = wasta;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
