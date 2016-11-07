using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using WPFCustomMessageBox;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>

    //No real interaction weirdness in here, Only thing that could actually break this class is the absence of MainWindow, which it depends upon, but catching that is useless, as we can't reboot anyways.
    public partial class StartWindow : Window
    {
        public string defaultVersion = "none";
        public System.Timers.Timer launchTimer = new System.Timers.Timer(60000);
        public StartWindow()
        {
            try
            {
                InitializeComponent();
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("ZavinSlideshow\\Zavin\\Settings", true);
                if (rk == null)
                {
                    rk = Registry.CurrentUser.CreateSubKey("ZavinSlideshow\\Zavin\\Settings");
                    var result = CustomMessageBox.ShowYesNoCancel("It seems like this is the first time running this application, please select a default version", "Select default version", "Kantoor", "Wacht", "Geen default");
                    if (result == MessageBoxResult.Yes)
                    {
                        rk.SetValue("Version", "kantoor", RegistryValueKind.String);
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        rk.SetValue("Version", "wacht", RegistryValueKind.String);
                    }
                    else
                    {
                        rk.SetValue("Version", "none", RegistryValueKind.String);
                    }
                }
                else if (rk != null)
                {
                    defaultVersion = rk.GetValue("Version").ToString();
                    launchTimer.Elapsed += (sender, e) => launchTimer_Tick(sender);
                    launchTimer.Start();
                }
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

        }

        private void StartWachtBtn_Click(object sender, RoutedEventArgs e)
        {
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
            MainWindow mainWindow = new MainWindow("kantoor");
            mainWindow.Show();
            this.Close();
        }

        private void launchTimer_Tick(object sender)
        {
            launchTimer.Stop();
            if(defaultVersion == "kantoor")
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { StartKantoorBtn_Click(sender, null); }));
            }
            else if(defaultVersion == "wacht")
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { StartWachtBtn_Click(sender, null); }));
            }
        }
    }
}
