using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

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
            //MainChart_EditValues();
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

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Program.xDataControl.Rows.Clear();
            Program.xDataControl.Columns.Clear();
            MainChart.Series.Clear();

            Program.xDataControl.Columns.Add("X", typeof(int));
            Program.xDataControl.Columns.Add("Productie", typeof(int));
            Program.xDataControl.Columns.Add("Aanvoer", typeof(int));

            for (int i = 1; i < 54; i++)
            {
                DataRow dataRow = Program.xDataControl.NewRow();

                dataRow["X"] = i;
                dataRow["Productie"] = i * 12 - 10 + 33;
                dataRow["Aanvoer"] = i * 9 - 6 + 24;
                Program.xDataControl.Rows.Add(dataRow);
            }

            var convertedTable = (Program.xDataControl as IListSource).GetList();
            MainChart.DataBindTable(convertedTable, "X");
            MainChart.Series["Productie"].Points[33].Color = Color.Red;
        }
    }
}
