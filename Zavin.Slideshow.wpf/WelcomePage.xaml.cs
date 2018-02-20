using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            var mainController = new MainController();
            InitializeComponent();

            var welcomeItem = mainController.GetWelcomePage();

            WelcomeTitle.Content = welcomeItem.Title;
            WelcomeMessage.Text = welcomeItem.Description;

            if (welcomeItem.ImagePath != null)
            {
                try
                {
                    WelcomePhoto.ImageSource = new ImageBrush(new BitmapImage(new Uri(welcomeItem.ImagePath))).ImageSource;
                }
                catch (Exception ex) when (ex is FileNotFoundException || ex is ArgumentNullException || ex is UriFormatException)
                {
                    WelcomePhoto.Opacity = 0;
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() => { MainController.SendErrorMessage(ex); });
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
