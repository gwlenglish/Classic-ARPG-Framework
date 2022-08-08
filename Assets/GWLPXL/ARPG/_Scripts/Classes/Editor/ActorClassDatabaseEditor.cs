
#if UNITY_EDITOR

using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPGCore.Classes.com
{

    [CustomEditor(typeof(ActorClassDatabase))]
    public class ActorClassDatabaseEditor : UnityEditor.Editor
    {

     
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ActorClassDatabase actorclassDatabase = (ActorClassDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(actorclassDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                ClearIDs(actorclassDatabase);
            }
            if (GUILayout.Button("Open Class Editor"))
            {
                EditorMethods.OpenDatabaseWindow(actorclassDatabase);

            }

        }

        void ClearIDs(ActorClassDatabase savesystem)
        {
            savesystem.SetSlots(new ClassDatabaseSlot[0]);


            EditorUtility.SetDirty(savesystem);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

      
    }
}
#endif