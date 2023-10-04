using Newtonsoft.Json;
using System.IO.Compression;
using System.Net.Http;

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

        private async void InstallMod() 
        {
            string dlUrl = await GetDownloadUrl();
            if (dlUrl != "")
            {
                LogLine("Downloading mod installation archive ...");
                using (HttpClient httpClient = new HttpClient())
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(dlUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            using (FileStream fileStream = File.Create(Path.Combine(Application.LocalUserAppDataPath, "mpxdownload.zip")))
                            {
                                await contentStream.CopyToAsync(fileStream);
                                fileStream.Close();
                            }
                        }
                    }
                }

                if(File.Exists(Path.Combine(Application.LocalUserAppDataPath, "mpxdownload.zip")))
                {
                    LogLine("Installing mod ...");
                    ZipFile.ExtractToDirectory(Path.Combine(Application.LocalUserAppDataPath, "mpxdownload.zip"), frmMain.GetGameDirectory(frmMain.selectedGame), true);
                    File.Delete(Path.Combine(Application.LocalUserAppDataPath, "mpxdownload.zip"));
                }
            }
            else
            {
                LogLine("Could not install mod, please try again later!");
            }
        }

        private async Task<string> GetDownloadUrl() 
        {
            string apiUrl = $"https://api.github.com/repos/mindphluxnet/{frmMain.selectedGame.RepoName}/releases/latest";

            try
            {
                using (HttpClient client = new())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        GithubAPIResponse apiResponse = JsonConvert.DeserializeObject<GithubAPIResponse>(jsonContent);
                        return apiResponse.assets[0].browser_download_url;
                    }
                    else
                    {
                        LogLine("Error getting Github API response!");
                        return "";
                    }
                }
            }
            catch (Exception)
            {
                LogLine("Error getting Github API response!");
                return "";
            }


        }

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
