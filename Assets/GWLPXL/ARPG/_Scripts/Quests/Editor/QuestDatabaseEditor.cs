#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Quests.com
{

    [CustomEditor(typeof(QuestDatabase))]

    public class QuestDatabaseEditor : UnityEditor.Editor
    {
        QuestWindow window;
      
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            QuestDatabase questDatabase = (QuestDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(questDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                ClearIDs(questDatabase);
            }
            if (GUILayout.Button("Open Quest Editor"))
            {
                EditorMethods.OpenDatabaseWindow(questDatabase);

            }

        }
        void ClearIDs(QuestDatabase savesystem)
        {

            savesystem.SetSlots(new QuestDdatabaseSlot[0]);
            EditorUtility.SetDirty(savesystem);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

       

    }
}
#endif