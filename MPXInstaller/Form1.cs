using Microsoft.Win32;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MPXInstaller
{
    public partial class frmMain : Form
    {
        private string steamPath = "";
        private static readonly string configFileUrl = "https://raw.githubusercontent.com/mindphluxnet/MPXInstaller/master/MPXInstaller/config/GameConfig.json";
        private List<GameConfig> gameConfigs = new List<GameConfig>();

        public frmMain()
        {
            InitializeComponent();
        }

        private async void frmMain_Shown(object sender, EventArgs e)
        {
            string? _steamPath = GetSteamPath();
            if (_steamPath != null && Directory.Exists(_steamPath))
            {
                steamPath = _steamPath;
                await FetchSupportedGames();
                lbInstalledGames.Items.Clear();
                foreach(GameConfig _gc in gameConfigs)
                {
                    if(Directory.Exists(Path.Combine(steamPath, _gc.InstallPath)))
                    {
                        lbInstalledGames.Items.Add(_gc.Name);
                    }
                }
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

        private async Task FetchSupportedGames()
        {
            try
            {
                using(HttpClient client = new())
                {
                    HttpResponseMessage response = await client.GetAsync(configFileUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        gameConfigs = JsonConvert.DeserializeObject<List<GameConfig>>(jsonContent);
                    }
                    else
                    {
                        MessageBox.Show("Unable to download configuration file. Please try again later!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to download configuration file. Please try again later!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}