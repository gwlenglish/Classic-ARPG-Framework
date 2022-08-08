#if UNITY_EDITOR
using GWLPXL.ARPGCore.com;


using GWLPXL.ARPGCore.Statics.com;

using UnityEditor;
using UnityEngine;
namespace GWLPXL.ARPGCore.Abilities.com
{

    [CustomEditor(typeof(AbilitiesDatabase))]
    public class AbilitiesDatabaseEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AbilitiesDatabase abilityDatabase = (AbilitiesDatabase)target;
           

            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(abilityDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                ClearIDs(abilityDatabase);
            }
            if (GUILayout.Button("Open Ability Editor"))
            {
                EditorMethods.OpenDatabaseWindow(abilityDatabase);
            }
            if (GUILayout.Button("Export Database to Text"))
            {
                string result = DatabaseHandler.GetDBCSV(abilityDatabase);
                Debug.Log(result);
            }
            if (GUILayout.Button("Import Database to Text"))
            {
                DatabaseHandler.ImportDBCSV(abilityDatabase);
               // Debug.Log(result);
            }
        

        }

        void ClearIDs(AbilitiesDatabase savesystem)
        {
            savesystem.SetSlots(new AbilityDatabaseSlot[0]);
            EditorUtility.SetDirty(savesystem);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
       

       

       
    }
}

#endif