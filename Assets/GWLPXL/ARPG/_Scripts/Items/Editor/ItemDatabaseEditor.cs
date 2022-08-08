#if UNITY_EDITOR
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{

    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : UnityEditor.Editor
    {
     
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ItemDatabase itemDatabase = (ItemDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(itemDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                ClearIDs(itemDatabase);
            }
            if (GUILayout.Button("Open Item Editor"))
            {
                EditorMethods.OpenDatabaseWindow(itemDatabase);

            }

            EditorGUILayout.Space(25);

            EditorGUILayout.LabelField("Export/Import");
            if (GUILayout.Button("Export Database to Text"))
            {
                string result = DatabaseHandler.GetDBCSV(itemDatabase);
                //Debug.Log(result);
            }

            if (GUILayout.Button("Import Database to Text"))
            {
                DatabaseHandler.ImportDBCSV(itemDatabase);
                //Debug.Log(result);
            }

        }

        void ClearIDs(ItemDatabase savesystem)
        {
            savesystem.SetSlots(new ItemDatabaseSlot[0]);


            EditorUtility.SetDirty(savesystem);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
       
    }
}
#endif