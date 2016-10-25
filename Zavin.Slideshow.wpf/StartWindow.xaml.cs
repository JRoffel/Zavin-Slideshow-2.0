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
            string[] dirlines;
            var docdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            try
            {
                dirlines = File.ReadAllLines(docdir + @"\ConfigLocation.imp");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Could not find file location for configuration, have you removed configlocation.imp from your documents? Please run the configuration application manually to recreate this file", "Error finding location", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var realdir = dirlines[0] + @"\Zavin.Slideshow.Configuration.exe";

            try            
            {
                System.Diagnostics.Process.Start(realdir);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Could not find configuration program on disk, if you have moved it, launch it manually once, it should fix the registery", "Error launching configuration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
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
