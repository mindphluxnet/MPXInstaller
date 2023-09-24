using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPXInstaller
{
    public struct ModConfig
    {
        public int Version;
        public string FeatureLog;
        public UninstallFiles UninstallFiles;
    }

    public struct UninstallFiles
    {
        public List<string> Directories;
        public List<string> Files;
    }
}
