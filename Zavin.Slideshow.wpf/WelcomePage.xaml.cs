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
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        Memo WelcomeItem;

        public WelcomePage()
        {
            MainController mainController = new MainController();
            InitializeComponent();

            WelcomeItem = mainController.GetWelcomePage();

            WelcomeTitle.Content = WelcomeItem.Title;
            WelcomeMessage.Text = WelcomeItem.Description;

            if (WelcomeItem.ImagePath != null)
            {
                WelcomePhoto.ImageSource = new ImageBrush(new BitmapImage(new Uri(WelcomeItem.ImagePath))).ImageSource;
            }
            else
            {
                WelcomePhoto.Opacity = 0;
            }
        }
    }
}
