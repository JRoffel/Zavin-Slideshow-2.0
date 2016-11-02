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
        public void Echo()
        {
            Console.WriteLine("I am awake");
        }

        public List<ProductionDataModel> GetProduction(int Year)
        {
            var ProductionData = db.GetProductionTable(Year);
            return ProductionData;
        }

        public List<KeyValuePair<string, int>> GetAcaf(int Year)
        {
            var AcafData = db.GetAcafTable(Year);
            return AcafData;
        }

        public Memo GetWelcomePage()
        {
            var WelcomeItem = db.GetWelcomePage();
            return WelcomeItem;
        }

        public bool HasWelcomePage()
        {
            return db.HasWelcomeScreen();
        }

        public List<KeyValuePair<string, int>> GetPie()
        {
            var PieData = db.ParsePieData();
            return PieData;
        }

        public List<KeyValuePair<string, int>> GetLine()
        {
            var LineData = db.GetLineGraph();
            return LineData;
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
            int total = db.GetPieProduction();
            return total;
        }

        public int GetSlideTimer()
        {
            int slideTimerSeconds = db.GetSlideTimerSeconds();
            return (slideTimerSeconds * 1000);
        }

        public Memo GetMemo(int number)
        {
            Memo memo = db.GetMemo(number);
            return memo;
        }

        public int GetMemoCount()
        {
            var memoCount = db.GetMemoCount();
            return memoCount;
        }

        public int GetMemoConfig()
        {
            var memoCount = db.GetMemoConfig();
            return memoCount;
        }

        public static void SendErrorMessage()
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

                message.Subject = "test";
                message.Body = "Test body, is this in spam?";

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                var response = MessageBox.Show("The application has crashed, and was unable to send the developers an email with details of the crash. The following type of error has occured: " + ex.InnerException + ". Press OK to reboot the application or press Cancel to shut it down", "Application encountered an error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}
