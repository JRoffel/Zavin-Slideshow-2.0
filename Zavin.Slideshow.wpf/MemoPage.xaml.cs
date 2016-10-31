using System;
using System.Collections.Generic;
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
    /// Interaction logic for MemoPage.xaml
    /// </summary>
    public partial class MemoPage : Page
    {
        public MemoPage()
        {
            InitializeComponent();

            MemoWrapping();
        }

        public void MemoWrapping()
        { 
            string MemoImg = MemoPhoto.Source.ToString();

            if (MemoImg.Contains("images/slak.jpg"))
            {
                MemoText.Width = 1000;
                MemoText.Margin = new Thickness(-410, 300, 0, 0);
            }
            else
            {
                MemoText.Width = 1400;
                MemoText.Margin = new Thickness(0, 350, 0, 0);
                MemoPhoto.Visibility = Visibility.Collapsed;
            }
        }
    }
}
