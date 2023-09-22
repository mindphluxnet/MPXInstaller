namespace MPXInstaller
{
    partial class frmMain
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
            statusStrip1 = new StatusStrip();
            tsLabel = new ToolStripStatusLabel();
            tsProgress = new ToolStripProgressBar();
            lbInstalledGames = new ListBox();
            groupBox1 = new GroupBox();
            tbModFeatures = new TextBox();
            btnInstall = new Button();
            statusStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { tsLabel, tsProgress });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // tsLabel
            // 
            tsLabel.Name = "tsLabel";
            tsLabel.Size = new Size(68, 17);
            tsLabel.Text = "Scanning ...";
            tsLabel.Visible = false;
            // 
            // tsProgress
            // 
            tsProgress.Name = "tsProgress";
            tsProgress.Size = new Size(100, 16);
            tsProgress.Visible = false;
            // 
            // lbInstalledGames
            // 
            lbInstalledGames.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lbInstalledGames.FormattingEnabled = true;
            lbInstalledGames.ItemHeight = 21;
            lbInstalledGames.Location = new Point(12, 12);
            lbInstalledGames.Name = "lbInstalledGames";
            lbInstalledGames.Size = new Size(244, 403);
            lbInstalledGames.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tbModFeatures);
            groupBox1.Location = new Point(262, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(526, 360);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Mod Features";
            // 
            // tbModFeatures
            // 
            tbModFeatures.Location = new Point(6, 22);
            tbModFeatures.Multiline = true;
            tbModFeatures.Name = "tbModFeatures";
            tbModFeatures.ReadOnly = true;
            tbModFeatures.Size = new Size(514, 332);
            tbModFeatures.TabIndex = 0;
            // 
            // btnInstall
            // 
            btnInstall.Location = new Point(678, 378);
            btnInstall.Name = "btnInstall";
            btnInstall.Size = new Size(104, 37);
            btnInstall.TabIndex = 3;
            btnInstall.Text = "Install";
            btnInstall.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnInstall);
            Controls.Add(groupBox1);
            Controls.Add(lbInstalledGames);
            Controls.Add(statusStrip1);
            MaximizeBox = false;
            Name = "frmMain";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MPX Installer";
            Shown += frmMain_Shown;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tsLabel;
        private ToolStripProgressBar tsProgress;
        private ListBox lbInstalledGames;
        private GroupBox groupBox1;
        private TextBox tbModFeatures;
        private Button btnInstall;
    }
}