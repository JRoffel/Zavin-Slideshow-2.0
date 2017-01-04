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
    /// Interaction logic for MemoPage.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MemoPage : Page
    {
        public Memo MemoItem;

        public MemoPage(int memo)
        {
            var mainController = new MainController();
            MemoItem = mainController.GetMemo(memo);

            InitializeComponent();

            MemoCheckImage();

            MemoText.Text = MemoItem.Description;
            MemoTitle.Content = MemoItem.Title;
            MemoAuthor.Content = "Author: " + MemoItem.Author;
            MemoDate.Content = "Date: " + MemoItem.PostDate;
        }
        public void MemoCheckImage()
        {   
            if (MemoItem.ImagePath != null)
            {
                try
                {
                    MemoPhoto.Source = new ImageBrush(new BitmapImage(new Uri(MemoItem.ImagePath))).ImageSource;
                    MemoText.Width = 1400;
                    MemoText.Margin = new Thickness(-410, 350, 0, 0);
                }
                catch (Exception ex) when (ex is FileNotFoundException || ex is ArgumentNullException)
                {
                    MemoAuthor.Foreground = new SolidColorBrush(Colors.Red);
                    MemoText.Width = 1700;
                    MemoText.Margin = new Thickness(0, 350, 0, 0);
                    MemoPhoto.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
                }

            }
            else
            {
                MemoText.Width = 1700;
                MemoText.Margin = new Thickness(0, 350, 0, 0);
                MemoPhoto.Visibility = Visibility.Collapsed;
            }

        }

    }
}
