#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Quests.com
{

    [CustomEditor(typeof(QuestchainDatabase))]

    public class QuestchainDatabaseEditor : UnityEditor.Editor
    {
      
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            QuestchainDatabase questDatabase = (QuestchainDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(questDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                ClearIDs(questDatabase);
            }
            if (GUILayout.Button("Open Quest Chan Editor"))
            {
                EditorMethods.OpenDatabaseWindow(questDatabase);

            }

        }
        void ClearIDs(QuestchainDatabase savesystem)
        {

            savesystem.SetSlots(new QuestchainDdatabaseSlot[0]);

            EditorUtility.SetDirty(savesystem);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


    }
}
#endif