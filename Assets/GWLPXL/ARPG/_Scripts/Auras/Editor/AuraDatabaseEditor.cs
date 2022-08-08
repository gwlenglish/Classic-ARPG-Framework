#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Auras.com
{

    [CustomEditor(typeof(AuraDatabase))]
    public class AuraDatabaseEditor : UnityEditor.Editor
    {
    
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AuraDatabase auraDatabase = (AuraDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(auraDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                ClearIDs(auraDatabase);
            }
            if (GUILayout.Button("Open Aura Editor"))
            {
                EditorMethods.OpenDatabaseWindow(auraDatabase);

            }

        }

        void ClearIDs(AuraDatabase savesystem)
        {
            savesystem.SetSlots(new AuraDatabaseSlot[0]);



            EditorUtility.SetDirty(savesystem);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
       
    }
}
#endif
