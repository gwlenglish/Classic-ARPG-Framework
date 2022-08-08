using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class GameDatabaseModifyEditor: ArpgBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gamedatabase = (GWLPXL.ARPGCore.com.GameDatabase)target;
            if (GUILayout.Button("Reload Game Database"))
            {
                DatabaseHandler.ReloadDatabase(gamedatabase);
            }
            if (GUILayout.Button("Reload all databases"))
            {
                DatabaseHandler.ReloadAll(gamedatabase);
            }
        }
    }
}