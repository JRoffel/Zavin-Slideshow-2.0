using System.ComponentModel;
using System.Windows.Forms;

namespace Zavin.Slideshow.Configuration
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SlideTimerLabel = new System.Windows.Forms.Label();
            this.YearTargetLabel = new System.Windows.Forms.Label();
            this.MemoCountLabel = new System.Windows.Forms.Label();
            this.SlideTimerInput = new System.Windows.Forms.TextBox();
            this.YearTargetInput = new System.Windows.Forms.TextBox();
            this.MemoCountInput = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.RestoreButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SlideTimerLabel
            // 
            this.SlideTimerLabel.AutoSize = true;
            this.SlideTimerLabel.Location = new System.Drawing.Point(12, 36);
            this.SlideTimerLabel.Name = "SlideTimerLabel";
            this.SlideTimerLabel.Size = new System.Drawing.Size(222, 13);
            this.SlideTimerLabel.TabIndex = 0;
            this.SlideTimerLabel.Text = "Tijd tussen de slides in seconden (default: 30)";
            // 
            // YearTargetLabel
            // 
            this.YearTargetLabel.AutoSize = true;
            this.YearTargetLabel.Location = new System.Drawing.Point(15, 96);
            this.YearTargetLabel.Name = "YearTargetLabel";
            this.YearTargetLabel.Size = new System.Drawing.Size(255, 13);
            this.YearTargetLabel.TabIndex = 1;
            this.YearTargetLabel.Text = "Productietarget in tonnen voor dit jaar (default: 9750)";
            // 
            // MemoCountLabel
            // 
            this.MemoCountLabel.AutoSize = true;
            this.MemoCountLabel.Location = new System.Drawing.Point(18, 155);
            this.MemoCountLabel.Name = "MemoCountLabel";
            this.MemoCountLabel.Size = new System.Drawing.Size(164, 13);
            this.MemoCountLabel.TabIndex = 2;
            this.MemoCountLabel.Text = "Aantal memo\'s per run (default: 5)";
            // 
            // SlideTimerInput
            // 
            this.SlideTimerInput.Location = new System.Drawing.Point(295, 33);
            this.SlideTimerInput.Name = "SlideTimerInput";
            this.SlideTimerInput.Size = new System.Drawing.Size(151, 20);
            this.SlideTimerInput.TabIndex = 3;
            this.SlideTimerInput.TextChanged += new System.EventHandler(this.SlideTimerInput_TextChanged);
            // 
            // YearTargetInput
            // 
            this.YearTargetInput.Location = new System.Drawing.Point(295, 93);
            this.YearTargetInput.Name = "YearTargetInput";
            this.YearTargetInput.Size = new System.Drawing.Size(151, 20);
            this.YearTargetInput.TabIndex = 4;
            this.YearTargetInput.TextChanged += new System.EventHandler(this.YearTargetInput_TextChanged);
            // 
            // MemoCountInput
            // 
            this.MemoCountInput.Location = new System.Drawing.Point(295, 152);
            this.MemoCountInput.Name = "MemoCountInput";
            this.MemoCountInput.Size = new System.Drawing.Size(151, 20);
            this.MemoCountInput.TabIndex = 5;
            this.MemoCountInput.TextChanged += new System.EventHandler(this.MemoCountInput_TextChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(157, 208);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(91, 23);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(269, 208);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(88, 23);
            this.ExitButton.TabIndex = 7;
            this.ExitButton.Text = "Close";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // RestoreButton
            // 
            this.RestoreButton.Location = new System.Drawing.Point(371, 208);
            this.RestoreButton.Name = "RestoreButton";
            this.RestoreButton.Size = new System.Drawing.Size(95, 23);
            this.RestoreButton.TabIndex = 8;
            this.RestoreButton.Text = "Restore default";
            this.RestoreButton.UseVisualStyleBackColor = true;
            this.RestoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 255);
            this.Controls.Add(this.RestoreButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.MemoCountInput);
            this.Controls.Add(this.YearTargetInput);
            this.Controls.Add(this.SlideTimerInput);
            this.Controls.Add(this.MemoCountLabel);
            this.Controls.Add(this.YearTargetLabel);
            this.Controls.Add(this.SlideTimerLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label SlideTimerLabel;
        private Label YearTargetLabel;
        private Label MemoCountLabel;
        private TextBox SlideTimerInput;
        private TextBox YearTargetInput;
        private TextBox MemoCountInput;
        private Button SaveButton;
        private Button ExitButton;
        private Button RestoreButton;
    }
}

