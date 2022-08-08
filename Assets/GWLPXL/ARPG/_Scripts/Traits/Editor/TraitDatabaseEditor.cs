using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPGCore.Traits.com
{


    [CustomEditor(typeof(EquipmentTraitDatabase))]
    public class TraitDatabaseEditor : UnityEditor.Editor
    {
       
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EquipmentTraitDatabase traitDatabase = (EquipmentTraitDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(traitDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                ClearIDs(traitDatabase);
            }
            if (GUILayout.Button("Open Trait Editor"))
            {
                EditorMethods.OpenDatabaseWindow(traitDatabase);

            }

            EditorGUILayout.Space(25);

            EditorGUILayout.LabelField("Export/Import");
            if (GUILayout.Button("Export Database to Text"))
            {
                string result = DatabaseHandler.GetDBCSV(traitDatabase);
                //Debug.Log(result);
            }

            if (GUILayout.Button("Import Database to Text"))
            {
                DatabaseHandler.ImportDBCSV(traitDatabase);
                //Debug.Log(result);
            }

        }

        void ClearIDs(EquipmentTraitDatabase savesystem)
        {
            savesystem.SetSlots(new TraitDatabaseSlot[0]);


            EditorUtility.SetDirty(savesystem);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
       
    }
}