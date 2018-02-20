using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace Zavin.Slideshow.wpf
{
    public class MainController
    {
        private const string Password = "SuperSeriousAmazingPasswordOfEpicnessForMail";
        private readonly DatabaseController _db = new DatabaseController();

        public List<ProductionDataModel> GetProduction(int year, bool isLacal)
        {
            List<ProductionDataModel> result = null;
            try
            {
                result = _db.GetProductionTable(year, isLacal);
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public List<KeyValuePair<string, int>> GetAcaf(int year, bool isLacal)
        {
            List<KeyValuePair<string, int>> result = null;

            try
            {
                result = _db.GetAcafTable(year, isLacal);
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public Memo GetWelcomePage()
        {
            Memo result = null;

            try
            {
                result = _db.GetWelcomePage();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public bool HasWelcomePage()
        {
            var result = false;

            try
            {
                result = _db.HasWelcomeScreen();
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public List<KeyValuePair<string, int>> GetPie()
        {
            List<KeyValuePair<string, int>> result = null;

            try
            {
                result = _db.ParsePieData();
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public List<KeyValuePair<string, int>> GetLine()
        {
            List<KeyValuePair<string, int>> result = null;

            try
            {
                result = _db.GetLineGraph();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public List<KeyValuePair<string, int>> GetZeroLine()
        {
            var zeroList = new List<KeyValuePair<string, int>> {new KeyValuePair<string, int>("53", 0)};

            for(var i = 1; i < 53; i++)
            {
                zeroList.Add(new KeyValuePair<string, int>(i.ToString(), 0));
            }

            return zeroList;
        }

        public int GetProdPie()
        {
            var result = 0;

            try
            {
                result = _db.GetPieProduction();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }
            
            return result;
        }

        public int GetSlideTimer()
        {
            var result = 0;

            try
            {
                result = _db.GetSlideTimerSeconds();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result * 1000;
        }

        public Memo GetMemo(int number)
        {
            Memo result = null;

            try
            {
                result = _db.GetMemo(number);
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public int GetMemoCount()
        {
            var result = 0;

            try
            {
                result = _db.GetMemoCount();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public int GetMemoConfig()
        {
            var result = 0;

            try
            {
                result = _db.GetMemoConfig();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { SendErrorMessage(ex); });
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public static void SendErrorMessage(Exception exceptionMessage)
        {
            try
            {
                var fromAddress = new MailAddress("zavinslideshowbugreport@gmail.com");

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromAddress.Address, Password)
                };

                var message = new MailMessage {From = fromAddress};

                message.To.Add("jasonroffel@hotmail.nl");
                message.To.Add("angeloroks@hotmail.com");
                message.To.Add("lucas.assink97@gmail.com");

                message.Subject = "Exception occured";
                message.Body = exceptionMessage.GetType().Name + "\n\n " + exceptionMessage.Message + "\n\n The application ran for: " + MainWindow.CurrentRunTime + " Minutes \n\n The error occured according to the following stacktrace: " + exceptionMessage.StackTrace + " At:" + exceptionMessage.Source + "\n\n Good luck solving!";

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(exceptionMessage.Message + exceptionMessage.InnerException + exceptionMessage.Source, exceptionMessage.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show("The application has crashed, and was unable to send the developers an email with details of the crash. The following type of error has occured: " + ex.Message + ". Press OK to reboot the application or press Cancel to shut it down", "Application encountered an error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}