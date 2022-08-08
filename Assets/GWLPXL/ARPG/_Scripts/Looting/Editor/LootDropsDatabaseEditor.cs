#if UNITY_EDITOR
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPGCore.Looting.com
{


    [CustomEditor(typeof(LootDropsDatabase))]
    public class LootDropsDatabaseEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LootDropsDatabase lootdatabase = (LootDropsDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(lootdatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                lootdatabase.SetSlots(new LootDropsDatabaseSlot[0]);
                EditorUtility.SetDirty(lootdatabase);
            }
            if (GUILayout.Button("Open Aura Editor"))
            {
                EditorMethods.OpenDatabaseWindow(lootdatabase);

            }

        }

    }
}
#endif
