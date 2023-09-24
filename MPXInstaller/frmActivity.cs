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

            tbActivityLog.Text +=
                $"Uninstalling mod for {frmMain.selectedGame.Name} ...{Environment.NewLine}";

            foreach (string f in config.UninstallFiles.Directories)
            {
                try
                {
                    tbActivityLog.Text += $"Deleting directory {f} ...{Environment.NewLine}";
                    Directory.Delete(
                        Path.Combine(frmMain.GetGameDirectory(frmMain.selectedGame), f),
                        true
                    );
                }
                catch (Exception ex)
                {
                    tbActivityLog.Text +=
                        $"Error deleting directory: {ex.Message}{Environment.NewLine}";
                    hasErrors = true;
                }
            }

            foreach (string f in config.UninstallFiles.Files)
            {
                try
                {
                    tbActivityLog.Text += $"Deleting file {f} ...{Environment.NewLine}";
                    File.Delete(Path.Combine(frmMain.GetGameDirectory(frmMain.selectedGame), f));
                }
                catch (Exception ex)
                {
                    tbActivityLog.Text += $"Error deleting file: {ex.Message}{Environment.NewLine}";
                    hasErrors = true;
                }
            }

            try
            {
                tbActivityLog.Text += $"Deleting file mod.json ... {Environment.NewLine}";
                File.Delete(
                    Path.Combine(frmMain.GetGameDirectory(frmMain.selectedGame), "mod.json")
                );
            }
            catch (Exception ex)
            {
                tbActivityLog.Text += $"Error deleting file: {ex.Message}{Environment.NewLine}";
                hasErrors = true;
            }

            btnClose.Enabled = true;
            tbActivityLog.Text += hasErrors
                ? "Uninstall completed with errors, see above."
                : "Uninstall completed successfully.";
        }

        private void UpdateMod() { }
    }

    public enum InstallerActivity
    {
        Install,
        Uninstall,
        Update
    }
}
