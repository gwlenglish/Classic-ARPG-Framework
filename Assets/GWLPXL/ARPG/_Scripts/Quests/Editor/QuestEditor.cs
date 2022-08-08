#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;


namespace GWLPXL.ARPGCore.Quests.com
{

    [CustomEditor(typeof(Quest), true)]
    public class QuestEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(25);
            Quest holder = (Quest)target;
            if (GUILayout.Button("Save as NEW Json Config"))
            {
                JsconConfig.SaveJson(holder);
            }
            if (GUILayout.Button("Load from Json Config"))
            {
                JsconConfig.LoadJson(holder);
            }
            if (GUILayout.Button("Overwrite Json Config"))
            {
                JsconConfig.OverwriteJson(holder);
            }
        }

      
    }
}
#endif
