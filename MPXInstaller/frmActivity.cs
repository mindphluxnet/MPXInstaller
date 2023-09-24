namespace MPXInstaller
{
    public partial class frmActivity : Form
    {
        public static InstallerActivity Activity { get; set; }

        public frmActivity()
        {
            InitializeComponent();
        }

        private void frmActivity_Shown(object sender, EventArgs e)
        {
            switch (Activity)
            {
                case InstallerActivity.Install:
                    InstallMod();
                    break;
                case InstallerActivity.Uninstall:
                    UninstallMod();
                    break;
                case InstallerActivity.Update:
                    UpdateMod();
                    break;
            }
        }

        private void InstallMod() { }

        private void UninstallMod()
        {
            ModConfig config = frmMain.GetModConfig(frmMain.selectedGame);
            bool hasErrors = false;

            LogLine($"Uninstalling mod for {frmMain.selectedGame.Name} ...");

            foreach (string f in config.UninstallFiles.Directories)
            {
                try
                {
                    LogLine($"Deleting directory {f} ...");
                    Directory.Delete(
                        Path.Combine(frmMain.GetGameDirectory(frmMain.selectedGame), f),
                        true
                    );
                }
                catch (Exception ex)
                {
                    LogLine($"Error deleting directory: {ex.Message}");
                    hasErrors = true;
                }
            }

            foreach (string f in config.UninstallFiles.Files)
            {
                try
                {
                    LogLine($"Deleting file {f} ...");
                    File.Delete(Path.Combine(frmMain.GetGameDirectory(frmMain.selectedGame), f));
                }
                catch (Exception ex)
                {
                    LogLine($"Error deleting file: {ex.Message}");
                    hasErrors = true;
                }
            }

            try
            {
                LogLine($"Deleting file mod.json ...");
                File.Delete(
                    Path.Combine(frmMain.GetGameDirectory(frmMain.selectedGame), "mod.json")
                );
            }
            catch (Exception ex)
            {
                LogLine($"Error deleting file: {ex.Message}");
                hasErrors = true;
            }

            btnClose.Enabled = true;
            tbActivityLog.Text += hasErrors
                ? "Uninstall completed with errors, see above."
                : "Uninstall completed successfully.";
        }

        private void UpdateMod() { }

        private void LogLine(string msg)
        {
            tbActivityLog.Text += msg + Environment.NewLine;
        }
    }

    public enum InstallerActivity
    {
        Install,
        Uninstall,
        Update
    }
}
