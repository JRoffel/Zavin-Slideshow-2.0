using System;
using System.Windows.Forms;

namespace Zavin.Slideshow.Configuration
{
    public partial class Form1 : Form
    {
        ConfigurationController controller = new ConfigurationController();
        bool SlideTimerChanged = false;
        bool YearTargetChanged = false;
        bool MemoCountChanged = false;
        bool AllSuccess = false;

        public Form1()
        {
            InitializeComponent();
            this.MaximumSize = new System.Drawing.Size(this.Width, this.Height);
            this.MinimumSize = this.MaximumSize;
            UpdateTextLabel();
        }

        private void UpdateTextLabel()
        {
            SlideTimerInput.Text = controller.GetSlideCount().ToString();
            YearTargetInput.Text = controller.GetYearTarget().ToString();
            MemoCountInput.Text = controller.GetMemoCount().ToString();

            SlideTimerChanged = false;
            YearTargetChanged = false;
            MemoCountChanged = false;
        }

        private void SlideTimerInput_TextChanged(object sender, System.EventArgs e)
        {
            SlideTimerChanged = true;
        }

        private void YearTargetInput_TextChanged(object sender, System.EventArgs e)
        {
            YearTargetChanged = true;
        }

        private void MemoCountInput_TextChanged(object sender, System.EventArgs e)
        {
            MemoCountChanged = true;
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            if (SlideTimerChanged == true)
            {
                int newValue = 0;
                if (SlideTimerInput.Text.Contains(",") || SlideTimerInput.Text.Contains("."))
                {
                    MessageBox.Show("Values cannot be decimal, please insert an integer", "Error checking values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateTextLabel();
                    return;
                }

                if (SlideTimerInput.Text.Contains("-"))
                {
                    MessageBox.Show("Values cannot be negative", "Error checking values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateTextLabel();
                    return;
                }

                try
                {
                    newValue = Convert.ToInt32(SlideTimerInput.Text);
                }
                catch (Exception problem)
                {
                    MessageBox.Show(problem.Message, "Error converting values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(newValue == 0)
                {
                    MessageBox.Show("A value of 0 is not allowed for the configuration of 'SlideCounter', please pick a value higher than 0", "Error with value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(newValue <= 10)
                {
                    var result = MessageBox.Show("A value of 10 or lower is not recommended for the configuration of 'SlideCounter', Please confirm that this is the value you want (Keeping this value can cause problems in the application)", "Warning checking value", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if(result == DialogResult.Cancel)
                    {
                        UpdateTextLabel();
                        return;
                    }
                }

                if(newValue >= 180)
                {
                    var result = MessageBox.Show("A value of 3 minutes or higher is not recommended for the configuration of 'SlideCounter', Please confirm that this is the value you want (Keeping this value can make the application look like it is stuck due to long slide times)", "Warning checking value", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if(result == DialogResult.Cancel)
                    {
                        UpdateTextLabel();
                        return;
                    }
                }

                bool success = controller.SetSlideCount(newValue);

                if (success == false)
                {
                    MessageBox.Show("There was an error writing to the database, make sure you have internet access, and that the server is still working", "Error connecting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AllSuccess = true;
            }

            if (YearTargetChanged == true)
            {
                if (YearTargetInput.Text.Contains(",") || YearTargetInput.Text.Contains("."))
                {
                    MessageBox.Show("Values cannot be decimal, please insert an integer", "Error checking values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateTextLabel();
                    return;
                }

                if (YearTargetInput.Text.Contains("-"))
                {
                    MessageBox.Show("Values cannot be negative", "Error checking values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateTextLabel();
                    return;
                }

                int newValue = 0;
                try
                {
                    newValue = Convert.ToInt32(YearTargetInput.Text);
                }
                catch (Exception problem)
                {
                    MessageBox.Show(problem.Message, "Error converting values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool success = controller.SetYearTarget(newValue);

                if (success == false)
                {
                    MessageBox.Show("There was an error writing to the database, make sure you have internet access, and that the server is still working", "Error connecting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AllSuccess = true;
            }

            if (MemoCountChanged == true)
            {
                if (MemoCountInput.Text.Contains(",") || MemoCountInput.Text.Contains("."))
                {
                    MessageBox.Show("Values cannot be decimal, please insert an integer", "Error checking values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateTextLabel();
                    return;
                }

                if (MemoCountInput.Text.Contains("-"))
                {
                    MessageBox.Show("Values cannot be negative", "Error checking values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateTextLabel();
                    return;
                }

                int newValue = 0;
                try
                {
                    newValue = Convert.ToInt32(MemoCountInput.Text);
                }
                catch (Exception problem)
                {
                    MessageBox.Show(problem.Message, "Error converting values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(newValue == 0)
                {
                    var result = MessageBox.Show("A value of 0 is not recommended for the configuration of 'MemoCount', Please confirm that this is the value you want (The program automatically stops showing memo's if there are none, you don't need to edit this setting for that)", "Warning checking values", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if(result == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                bool success = controller.SetMemoCount(newValue);

                if (success == false)
                {
                    MessageBox.Show("There was an error writing to the database, make sure you have internet access, and that the server is still working", "Error connecting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AllSuccess = true;
            }

            if(AllSuccess == true)
            {
                UpdateTextLabel();

                MessageBox.Show("New configuration values have been applied, it can take up to 5 minutes for these values to take effect", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            if(SlideTimerChanged == true || YearTargetChanged == true || MemoCountChanged == true)
            {
                var result = MessageBox.Show("You have unsaved changes, are you sure you want to exit?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if(result == DialogResult.No)
                {
                    return;
                }

                if(result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            else
            {
                Application.Exit();
            }
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Restoring to default values will override all your changes to the configuration, are you sure you want to do this?", "Please confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }
            else if(result == DialogResult.Yes)
            {
                var success = controller.SetSlideCount(30);

                if (!success)
                {
                    MessageBox.Show("There was an error writing to the database, make sure you have internet access, and that the server is still working", "Error connecting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                success = controller.SetYearTarget(9750);

                if (!success)
                {
                    MessageBox.Show("There was an error writing to the database, make sure you have internet access, and that the server is still working", "Error connecting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                success = controller.SetMemoCount(5);

                if (!success)
                {
                    MessageBox.Show("There was an error writing to the database, make sure you have internet access, and that the server is still working", "Error connecting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UpdateTextLabel();

                MessageBox.Show("Restored all values to default! It might take 5 minutes for these changes to take effect", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
