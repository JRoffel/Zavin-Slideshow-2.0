using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows;
using Microsoft.Win32;
using WPFCustomMessageBox;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>

    //No real interaction weirdness in here, Only thing that could actually break this class is the absence of MainWindow, which it depends upon, but catching that is useless, as we can't reboot anyways.
    // ReSharper disable once RedundantExtendsListEntry
    public partial class StartWindow : Window
    {
        public string DefaultVersion = "none";
        public Timer LaunchTimer = new Timer(60000);
        public StartWindow()
        {
            try
            {
                InitializeComponent();
                var rk = Registry.CurrentUser.OpenSubKey("ZavinSlideshow\\Zavin\\Settings", true);
                if (rk == null)
                {
                    rk = Registry.CurrentUser.CreateSubKey("ZavinSlideshow\\Zavin\\Settings");
                    var result = CustomMessageBox.ShowYesNoCancel("It seems like this is the first time running this application, please select a default version", "Select default version", "Kantoor", "Wacht", "Geen default");
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            rk?.SetValue("Version", "kantoor", RegistryValueKind.String);
                            break;
                        case MessageBoxResult.No:
                            rk?.SetValue("Version", "wacht", RegistryValueKind.String);
                            break;
                        case MessageBoxResult.None:
                            break;
                        case MessageBoxResult.OK:
                            break;
                        case MessageBoxResult.Cancel:
                            rk?.SetValue("Version", "none", RegistryValueKind.String);
                            break;
                        default:
                            rk?.SetValue("Version", "none", RegistryValueKind.String);
                            break;
                    }
                }
                else
                {
                    DefaultVersion = rk.GetValue("Version").ToString();
                    LaunchTimer.Elapsed += (sender, e) => launchTimer_Tick(sender);
                    LaunchTimer.Start();
                }
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { MainController.SendErrorMessage(ex); }));
                // ReSharper disable once AssignNullToNotNullAttribute
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

        }

        private void StartWachtBtn_Click(object sender, RoutedEventArgs e)
        {
            LaunchTimer.Stop();
            var mainWindow = new MainWindow("wacht");
            mainWindow.Show();
            Close();
        }

        private void StartConfiguratieBtn_Click(object sender, RoutedEventArgs e)
        {
            LaunchTimer.Stop();
            string[] dirlines;
            var docdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            try
            {
                dirlines = File.ReadAllLines(docdir + @"\ConfigLocation.imp");
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find file location for configuration, have you removed configlocation.imp from your documents? Please run the configuration application manually to recreate this file", "Error finding location", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var realdir = dirlines[0] + @"\Zavin.Slideshow.Configuration.exe";

            try            
            {
                Process.Start(realdir);
            }
            catch (Exception)
            {
                MessageBox.Show("Could not find configuration program on disk, if you have moved it, launch it manually once, it should fix the registery", "Error launching configuration", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void StartKantoorBtn_Click(object sender, RoutedEventArgs e)
        {
            LaunchTimer.Stop();
            var mainWindow = new MainWindow("kantoor");
            mainWindow.Show();
            Close();
        }

        private void launchTimer_Tick(object sender)
        {
            LaunchTimer.Stop();
            if(DefaultVersion == "kantoor")
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { StartKantoorBtn_Click(sender, null); }));
            }
            else if(DefaultVersion == "wacht")
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { StartWachtBtn_Click(sender, null); }));
            }
        }
    }
}
