#if UNITY_EDITOR


using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Quests.com
{

    [CustomEditor(typeof(QuestLogDatabase))]

    public class QuestLogDatabaseEditor : UnityEditor.Editor
    {
      
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            QuestLogDatabase questDatabase = (QuestLogDatabase)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Reload Database"))
            {
                DatabaseHandler.ReloadDatabase(questDatabase);
            }
            if (GUILayout.Button("Clear Database IDs"))
            {
                questDatabase.SetSlots(new QuestLogdatabaseSlot[0]);
            }
            if (GUILayout.Button("Open Quest Editor"))
            {
                EditorMethods.OpenDatabaseWindow(questDatabase);

            }

        }
      

       

    }
}
#endif