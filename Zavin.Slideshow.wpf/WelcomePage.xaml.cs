using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                try
                {
                    WelcomePhoto.ImageSource = new ImageBrush(new BitmapImage(new Uri(WelcomeItem.ImagePath))).ImageSource;
                }
                catch (Exception ex) when (ex is FileNotFoundException || ex is ArgumentNullException || ex is UriFormatException)
                {
                    WelcomePhoto.Opacity = 0;
                    WelcomeMessage.Margin = new Thickness(20, 880, 0, 0);
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
                }
            }
            else
            {
                WelcomeCanvas.Visibility = Visibility.Collapsed;
                WelcomeMessage.Margin = new Thickness(20, 400, 0, 0);
            }
        }
    }
}
