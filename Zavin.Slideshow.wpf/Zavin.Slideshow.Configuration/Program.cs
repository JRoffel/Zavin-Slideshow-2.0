using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Zavin.Slideshow.Configuration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var docdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (File.Exists(docdir + @"\ConfigLocation.imp"))
            {
                File.Delete(docdir + @"\ConfigLocation.imp");
            }

            File.WriteAllText(docdir + @"\ConfigLocation.imp", Directory.GetCurrentDirectory());

            Application.Run(new Form1());
        }
    }
}
