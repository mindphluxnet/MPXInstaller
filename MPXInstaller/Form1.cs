using Microsoft.Win32;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MPXInstaller
{
    public partial class frmMain : Form
    {
        private string steamPath = "";
        private static readonly string configFileUrl =
            "https://raw.githubusercontent.com/mindphluxnet/MPXInstaller/master/MPXInstaller/config/GameConfig.json";
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
                gameConfigs = await FetchSupportedGames();
                lbInstalledGames.Items.Clear();
                foreach (GameConfig _gc in gameConfigs)
                {
                    if (IsGameInstalled(_gc))
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

        private bool IsGameInstalled(GameConfig gameConfig)
        {
            return Directory.Exists(
                Path.Combine(
                    steamPath,
                    "steamapps",
                    "common",
                    gameConfig.InstallPath,
                    gameConfig.SubDir == null ? "" : gameConfig.SubDir
                )
            );
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
                catch
                {
                    return null;
                }
            }
            return null;
        }

        private GameConfig GetGameConfigByName(string name) =>
            gameConfigs.Find(x => x.Name == name);

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Performance",
            "CA1822:Member als statisch markieren",
            Justification = "<Ausstehend>"
        )]
        private async Task<List<GameConfig>> FetchSupportedGames()
        {
            try
            {
                using (HttpClient client = new())
                {
                    HttpResponseMessage response = await client.GetAsync(configFileUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<GameConfig>>(jsonContent);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Unable to download configuration file. Please try again later!",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        Application.Exit();
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Unable to download configuration file. Please try again later!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Application.Exit();
                return null;
            }
        }

        private void lbInstalledGames_Click(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem == null) return;
            UpdateDisplay(lb.SelectedItem.ToString());
        }

        private void UpdateDisplay(string name)
        {
            tbModFeatures.Clear();
            btnInstall.Text = IsModInstalled(name) ? "Uninstall" : "Install";
        }

        private bool IsModInstalled(string name)
        {
            GameConfig _cfg = GetGameConfigByName(name);
            if (IsGameInstalled(_cfg))
            {
                if (
                    File.Exists(
                        Path.Combine(
                            steamPath,
                            "steamapps",
                            "common",
                            _cfg.InstallPath,
                            _cfg.SubDir == null ? "" : _cfg.SubDir,
                            "mod.json"
                        )
                    )
                )
                {
                    ModConfig? mc = JsonConvert.DeserializeObject<ModConfig>(
                        File.ReadAllText(
                            Path.Combine(
                                steamPath,
                                "steamapps",
                                "common",
                                _cfg.InstallPath,
                                _cfg.SubDir == null ? "" : _cfg.SubDir,
                                "mod.json"
                            )
                        )
                    );
                    if (mc != null)
                        return true;
                }
            }
            return false;
        }
    }
}
