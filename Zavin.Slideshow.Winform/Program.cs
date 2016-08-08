using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zavin.Slideshow.Winform
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        //public static int[] xDataControl = new int[53] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53 };

        public static DataTable xDataControl = new DataTable();
        //public static int[][] xDataControl = new int[2][53] { { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53 }, { 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 341, 562, 643 } };

        [STAThread]
        static void Main()
        {
            //foreach (int xDataNumber in xDataControl)
            //{
            //    Console.WriteLine(xDataNumber);
            //}
            //xDataControl = new int[53] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53 };
            //xDataControl[1] = new int[53] { 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 564, 341, 259, 154, 845, 341, 562, 643 };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
