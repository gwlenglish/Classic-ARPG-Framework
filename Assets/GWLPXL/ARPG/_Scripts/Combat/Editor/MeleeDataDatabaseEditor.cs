#if UNITY_EDITOR
using GWLPXL.ARPGCore.com;


using GWLPXL.ARPGCore.Statics.com;

using UnityEditor;
using UnityEngine;
namespace GWLPXL.ARPGCore.Combat.com
{

    [CustomEditor(typeof(MeleeDataDatabase))]
    public class MeleeDataDatabaseEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MeleeDataDatabase abilityDatabase = (MeleeDataDatabase)target;
           

            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(abilityDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                ClearIDs(abilityDatabase);
            }
            if (GUILayout.Button("Open Melee Data Editor"))
            {
                EditorMethods.OpenDatabaseWindow(abilityDatabase);
            }
            if (GUILayout.Button("Export Database to Text"))
            {
                string result = DatabaseHandler.GetDBCSV(abilityDatabase);
                //Debug.Log(result);
            }
            if (GUILayout.Button("Import Database to Text"))
            {
                DatabaseHandler.ImportDBCSV(abilityDatabase);
               // Debug.Log(result);
            }
        

        }

        void ClearIDs(MeleeDataDatabase savesystem)
        {
            savesystem.SetSlots(new MeleeDataDatabaseSlot[0]);
            EditorUtility.SetDirty(savesystem);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
       

       

       
    }
}

#endif