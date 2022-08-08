using System.Collections.Generic;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPG._Scripts.Editor.ReloadProcessors.com
{
    public interface IReloadGameDatabaseList
    {
        List<GameDatabase> GameDatabases { get; set; }
    }
}