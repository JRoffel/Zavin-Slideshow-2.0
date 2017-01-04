using System;
using System.IO;
using System.Windows.Forms;

namespace Zavin.Slideshow.Configuration
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var docdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (File.Exists(docdir + @"\ConfigLocation.imp"))
            {
                File.Delete(docdir + @"\ConfigLocation.imp");
            }

            File.WriteAllText(docdir + @"\ConfigLocation.imp", AppDomain.CurrentDomain.BaseDirectory);

            Application.Run(new Form1());
        }
    }
}
