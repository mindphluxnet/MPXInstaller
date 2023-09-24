namespace MPXInstaller
{
    partial class frmActivity
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            tbActivityLog = new TextBox();
            btnClose = new Button();
            SuspendLayout();
            // 
            // tbActivityLog
            // 
            tbActivityLog.Location = new Point(12, 12);
            tbActivityLog.Multiline = true;
            tbActivityLog.Name = "tbActivityLog";
            tbActivityLog.ReadOnly = true;
            tbActivityLog.Size = new Size(496, 229);
            tbActivityLog.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.DialogResult = DialogResult.OK;
            btnClose.Enabled = false;
            btnClose.Location = new Point(404, 247);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(104, 37);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // frmActivity
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 292);
            Controls.Add(btnClose);
            Controls.Add(tbActivityLog);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmActivity";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Activity";
            Shown += frmActivity_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbActivityLog;
        private Button btnClose;
    }
}