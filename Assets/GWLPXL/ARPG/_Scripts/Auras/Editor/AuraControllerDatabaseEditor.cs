#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Auras.com
{

    [CustomEditor(typeof(AuraControllerDatabase))]
    public class AuraControllerDatabaseEditor : UnityEditor.Editor
    {
      
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AuraControllerDatabase auraDatabase = (AuraControllerDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(auraDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                auraDatabase.SetSlots(new AuraControllerDatabaseSlot[0]);
                EditorUtility.SetDirty(auraDatabase);
            }
            if (GUILayout.Button("Open Aura Editor"))
            {
                EditorMethods.OpenDatabaseWindow(auraDatabase);

            }

        }

      
       
    }
}
#endif
