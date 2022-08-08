using GWLPXL.ARPG._Scripts.Editor.com;

namespace GWLPXL.ARPG._Scripts.Editor.ReloadProcessors.com
{
    public class ReloadGameDatabaseList: IReloadProcessor
    {
        public IReloadGameDatabaseList[] ReloadList;
        public ReloadGameDatabaseList(params IReloadGameDatabaseList[] databaseLists)
        {
            ReloadList = databaseLists;
        }
        
        public void Reload()
        {
            var list = ArpgEditorHelper.GetAllGameDatabasesInProject();

            foreach (var databaseList in ReloadList)
            {
                databaseList.GameDatabases = list;
            }
        }
    }
}