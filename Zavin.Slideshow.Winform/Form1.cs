using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Zavin.Slideshow.Winform
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            MainChart_EditValues();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainChart_TextChanged(object sender, EventArgs e)
        {

        }

        public void MainChart_EditValues()
        {
            MainChart.DataSource = Program.xDataControl;

            MainChart.Series.Add("Productie").YValueMembers = "Productie";
            MainChart.Series["Productie"].ChartType = SeriesChartType.Column;
            MainChart.Series["Productie"].XValueType = ChartValueType.Int32;
            MainChart.Series["Productie"].YValueType = ChartValueType.Int32;

            MainChart.Series.Add("Aanvoer").YValueMembers = "Aanvoer";
            MainChart.Series["Aanvoer"].ChartType = SeriesChartType.Column;
            MainChart.Series["Aanvoer"].XValueType = ChartValueType.Int32;
            MainChart.Series["Aanvoer"].YValueType = ChartValueType.Int32;

            MainChart.DataBind();
        }
    }
}
