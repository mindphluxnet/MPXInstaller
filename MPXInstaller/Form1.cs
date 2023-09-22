using Microsoft.Win32;

namespace MPXInstaller
{
    public partial class frmMain : Form
    {
        private string steamPath = "";
        private static readonly string configFileUrl = "https://"

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            string? _steamPath = GetSteamPath();
            if (_steamPath != null && Directory.Exists(_steamPath))
            {
                steamPath = _steamPath;
            }
            else
            {
                // No steam installed, where is it?
            }
        }

        private string? GetSteamPath()
        {            
            string path = @"Software\Valve\Steam";
            RegistryKey? _key = Registry.CurrentUser.OpenSubKey(path);

            if (_key != null)
            {

                try
                {
                    string? steamPath = (string)_key.GetValue("SteamPath");
                    if (steamPath != null)
                    {
                        return steamPath;
                    }
                }
                catch {
                    return null;
                }
            }
            return null;
        }

        private void FetchSupportedGames()
        {

        }
    }
}