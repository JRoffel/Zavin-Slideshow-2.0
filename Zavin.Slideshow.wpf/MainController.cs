using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace Zavin.Slideshow.wpf
{
    public class MainController
    {
        const string password = "SuperSeriousAmazingPasswordOfEpicnessForMail";
        DatabaseController db = new DatabaseController();

        public List<ProductionDataModel> GetProduction(int Year)
        {
            List<ProductionDataModel> result = null;
            try
            {
                result = db.GetProductionTable(Year);
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public List<KeyValuePair<string, int>> GetAcaf(int Year)
        {
            List<KeyValuePair<string, int>> result = null;

            try
            {
                result = db.GetAcafTable(Year);
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
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
                result = db.GetWelcomePage();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public bool HasWelcomePage()
        {
            bool result = false;

            try
            {
                result = db.HasWelcomeScreen();
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
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
                result = db.ParsePieData();
            }
            catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
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
                result = db.GetLineGraph();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public List<KeyValuePair<string, int>> GetZeroLine()
        {
            List<KeyValuePair<string, int>> ZeroList = new List<KeyValuePair<string, int>>();

            ZeroList.Add(new KeyValuePair<string, int>("53", 0));
            for(int i = 1; i < 53; i++)
            {
                ZeroList.Add(new KeyValuePair<string, int>(i.ToString(), 0));
            }

            return ZeroList;
        }

        public int GetProdPie()
        {
            int result = 0;

            try
            {
                result = db.GetPieProduction();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }
            
            return result;
        }

        public int GetSlideTimer()
        {
            int result = 0;

            try
            {
                result = db.GetSlideTimerSeconds();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return (result * 1000);
        }

        public Memo GetMemo(int number)
        {
            Memo result = null;

            try
            {
                result = db.GetMemo(number);
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public int GetMemoCount()
        {
            int result = 0;

            try
            {
                result = db.GetMemoCount();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public int GetMemoConfig()
        {
            int result = 0;

            try
            {
                result = db.GetMemoConfig();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke((() => { SendErrorMessage(ex); }));
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { Application.Current.Shutdown(); }));
            }

            return result;
        }

        public static void SendErrorMessage(Exception exceptionMessage)
        {
            try
            {
                MailAddress fromAddress = new MailAddress("zavinslideshowbugreport@gmail.com");

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromAddress.Address, password)
                };

                MailMessage message = new MailMessage();

                message.From = fromAddress;
                message.To.Add("jasonroffel@hotmail.nl");
                message.To.Add("angeloroks@hotmail.com");
                message.To.Add("lucas.assink97@gmail.com");

                message.Subject = "Exception occured";
                message.Body = exceptionMessage.GetType().Name + "\n\n " + exceptionMessage.Message.ToString() + "\n\n The application ran for: " + MainWindow.CurrentRunTime.ToString() + " Minutes \n\n The error occured according to the following stacktrace: " + exceptionMessage.StackTrace.ToString() + " At:" + exceptionMessage.Source.ToString() + "\n\n Good luck solving!";

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(exceptionMessage.Message + exceptionMessage.InnerException + exceptionMessage.Source, exceptionMessage.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                var response = MessageBox.Show("The application has crashed, and was unable to send the developers an email with details of the crash. The following type of error has occured: " + ex.Message + ". Press OK to reboot the application or press Cancel to shut it down", "Application encountered an error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}