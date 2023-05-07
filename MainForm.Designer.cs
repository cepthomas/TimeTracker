namespace TimeTracker
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbRecords = new ListBox();
            tbLegend = new TextBox();
            SuspendLayout();
            // 
            // lbRecords
            // 
            lbRecords.BackColor = SystemColors.Control;
            lbRecords.BorderStyle = BorderStyle.FixedSingle;
            lbRecords.Font = new Font("Consolas", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            lbRecords.FormattingEnabled = true;
            lbRecords.ItemHeight = 27;
            lbRecords.Location = new Point(12, 50);
            lbRecords.Margin = new Padding(3, 4, 3, 4);
            lbRecords.Name = "lbRecords";
            lbRecords.Size = new Size(639, 515);
            lbRecords.TabIndex = 0;
            lbRecords.MouseDoubleClick += Records_MouseDoubleClick;
            // 
            // tbLegend
            // 
            tbLegend.BorderStyle = BorderStyle.FixedSingle;
            tbLegend.Font = new Font("Consolas", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            tbLegend.Location = new Point(12, 14);
            tbLegend.Margin = new Padding(3, 4, 3, 4);
            tbLegend.Name = "tbLegend";
            tbLegend.ReadOnly = true;
            tbLegend.Size = new Size(639, 34);
            tbLegend.TabIndex = 1;
            tbLegend.Text = "Date        Mon Tue Wed Thu Fri Sat Sun Total";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(663, 575);
            Controls.Add(tbLegend);
            Controls.Add(lbRecords);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Time Tracker";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lbRecords;
        private TextBox tbLegend;
    }
}