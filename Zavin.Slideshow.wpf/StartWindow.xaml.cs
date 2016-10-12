using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void StartWachtBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            MainWindow mainWindow = new MainWindow("wacht");
            mainWindow.Show();
            this.Close();
        }

        private void StartConfiguratieBtn_Click(object sender, RoutedEventArgs e)
        {
            var curdir = Directory.GetCurrentDirectory();
            var realdir = curdir + "..\\..\\..\\..\\Zavin.Slideshow.Configuration\\bin\\x86\\Debug\\Zavin.Slideshow.Configuration.exe";
            System.Diagnostics.Process.Start(realdir);
        }

        private void StartKantoorBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            MainWindow mainWindow = new MainWindow("kantoor");
            mainWindow.Show();
            this.Close();
        }

        private void DisableButtons()
        {
            StartWachtBtn.Visibility = Visibility.Collapsed;
            StartConfiguratieBtn.Visibility = Visibility.Collapsed;
            StartKantoorBtn.Visibility = Visibility.Collapsed;
        }
    }
}
