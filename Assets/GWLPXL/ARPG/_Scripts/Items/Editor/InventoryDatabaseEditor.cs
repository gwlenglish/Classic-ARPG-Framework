#if UNITY_EDITOR
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{

    [CustomEditor(typeof(InventoryDatabase))]
    public class InventoryDatabaseEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            InventoryDatabase itemDatabase = (InventoryDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(itemDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                ClearIDs(itemDatabase);
            }
            if (GUILayout.Button("Open Inventory Editor"))
            {
                EditorMethods.OpenDatabaseWindow(itemDatabase);

            }

        }

        void ClearIDs(InventoryDatabase savesystem)
        {
            savesystem.SetSlots(new InventoryDatabaseSlot[0]);


            EditorUtility.SetDirty(savesystem);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        
    }
}
#endif