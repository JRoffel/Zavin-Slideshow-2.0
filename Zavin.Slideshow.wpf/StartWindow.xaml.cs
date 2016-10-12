using System;
using System.Collections.Generic;
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
            DisableButtons();
            throw new NotImplementedException();
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
