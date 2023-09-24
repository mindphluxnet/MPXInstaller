using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPXInstaller
{
    public struct GameConfig
    {
        public string Name { get; set; }
        public string RepoURL { get; set; }
        public string InstallPath { get; set; }
        public string SubDir { get; set; }
        public int Version { get; set; }
    }
}
