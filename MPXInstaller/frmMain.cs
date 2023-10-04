using Microsoft.Win32;
using Newtonsoft.Json;

namespace MPXInstaller
{
    public partial class frmMain : Form
    {
        public static string steamPath = "";
        private static readonly string configFileUrl =
            "https://raw.githubusercontent.com/mindphluxnet/MPXInstaller/master/MPXInstaller/config/GameConfig.json";
        private List<GameConfig> gameConfigs = new List<GameConfig>();
        public static GameConfig selectedGame;

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
                MessageBox.Show(
                    "Steam does not seem to be installed correctly!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Application.Exit();
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

        private bool IsGameRunning(GameConfig gameConfig)
        {
            string path = @"Software\Valve\Steam\Apps\" + gameConfig.SteamID.ToString();
            RegistryKey _key = Registry.CurrentUser.OpenSubKey(path);
            if (_key != null)
            {
                return (int)_key.GetValue("Running", 0) == 1;
            }
            return false;
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
                        return new();
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
                return new();
            }
        }

        private void lbInstalledGames_Click(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem == null)
                return;
            UpdateDisplay(lb.SelectedItem.ToString());
        }

        private void UpdateDisplay(string name)
        {
            tbModFeatures.Clear();
            btnInstall.Text = IsModInstalled(name) ? "Uninstall" : "Install";
            selectedGame = GetGameConfigByName(name);
            ModConfig _cfg = GetModConfig(selectedGame);
            tbModFeatures.AppendText(_cfg.FeatureLog);
            btnUpdate.Visible = _cfg.Version < selectedGame.Version;
        }

        public static ModConfig GetModConfig(GameConfig config)
        {
            ModConfig mc = JsonConvert.DeserializeObject<ModConfig>(
                File.ReadAllText(
                    Path.Combine(
                        steamPath,
                        "steamapps",
                        "common",
                        config.InstallPath,
                        config.SubDir == null ? "" : config.SubDir,
                        "mod.json"
                    )
                )
            );
            return mc;
        }

        public static string GetGameDirectory(GameConfig config)
        {
            return Path.Combine(steamPath, "steamapps", "common", config.InstallPath, config.SubDir == null ? "" : config.SubDir);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateMod();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            if(IsModInstalled(selectedGame.Name))
            {
                UninstallMod();
            }
            else
            {
                InstallMod();
            }
        }

        private void InstallMod()
        {
            if (!IsGameRunning(selectedGame))
            {
                frmActivity frmActivity = new frmActivity();
                frmActivity.Activity = InstallerActivity.Install;
                frmActivity.ShowDialog();
            }
            else
            {
                MessageBox.Show($"Can't install mod; {selectedGame.Name} is currently running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UninstallMod()
        {
            if (!IsGameRunning(selectedGame))
            {
                frmActivity frmActivity = new frmActivity();
                frmActivity.Activity = InstallerActivity.Uninstall;
                frmActivity.ShowDialog();
                UpdateDisplay(selectedGame.Name);
            }
            else
            {
                MessageBox.Show($"Can't uninstall mod; {selectedGame.Name} is currently running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateMod()
        {
            if (!IsGameRunning(selectedGame))
            {
                frmActivity frmActivity = new frmActivity();
                frmActivity.Activity = InstallerActivity.Update;
                frmActivity.ShowDialog();
            }
            else
            {
                MessageBox.Show($"Can't update mod; {selectedGame.Name} is currently running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
