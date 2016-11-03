using System;
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
    public partial class MemoPage : Page
    {
        public Memo MemoItem;

        public MemoPage(int memo)
        {
            MainController mainController = new MainController();
            /// <remarks>
            ///     This contains the entire memo, you can get all you need out of it. Press f12 on MemoItem to see what it has :P
            /// </remarks>
            /// <example>
            ///     eg: Title is stored in MemoItem.Title & ImagePath is stored in MemoItem.ImagePath
            /// </example>
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
                MemoPhoto.Source = new ImageBrush(new BitmapImage(new Uri(MemoItem.ImagePath))).ImageSource;
                MemoText.Width = 1400;
                MemoText.Margin = new Thickness(-410, 350, 0, 0);
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
