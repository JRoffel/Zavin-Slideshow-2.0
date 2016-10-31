using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for MemoPage.xaml
    /// </summary>
    public partial class MemoPage : Page
    {
        public MemoPage(int memo)
        {
            MainController mainController = new MainController();
            /// <remarks>
            ///     This contains the entire memo, you can get all you need out of it. Press f12 on MemoItem to see what it has :P
            /// </remarks>
            /// <example>
            ///     eg: Title is stored in MemoItem.Title & ImagePath is stored in MemoItem.ImagePath
            /// </example>
            Memo MemoItem = mainController.GetMemo(memo);

            Console.WriteLine("Desc: {0}, Title: {1}, Author: {2}", MemoItem.Description, MemoItem.Title, MemoItem.Author);

            InitializeComponent();

            //MemoWrapping();
            MemoText.Text = MemoItem.Description;
            MemoTitle.Content = MemoItem.Title;
            MemoAuthor.Content = "Author: " + MemoItem.Author;
            MemoDate.Content = "Date: " + MemoItem.PostDate;

            

            if (MemoItem.ImagePath != null)
            {
                var image = (BitmapImage)MemoPhoto.Source;
                image.UriSource = new Uri(MemoItem.ImagePath);
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

        //public void MemoWrapping()
        //{ 
        //    string MemoImg = MemoPhoto.Source.ToString();

        //    if (MemoImg.Contains("images/slak.jpg"))
        //    {
        //        MemoText.Width = 1000;
        //        MemoText.Margin = new Thickness(-410, 300, 0, 0);
        //    }
        //    else
        //    {
        //        MemoText.Width = 1400;
        //        MemoText.Margin = new Thickness(0, 350, 0, 0);
        //        MemoPhoto.Visibility = Visibility.Collapsed;
        //    }
        //}
    }
}
