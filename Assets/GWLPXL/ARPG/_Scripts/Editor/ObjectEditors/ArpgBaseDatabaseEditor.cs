using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com
{
    public class ArpgBaseDatabaseEditor: ArpgBaseEditor
    {
        public override void OnInspectorGUI()
        {
            var database = (IDatabase) target;
            base.OnInspectorGUI();
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(database.GetMyObject());
            }

        }
    }
}