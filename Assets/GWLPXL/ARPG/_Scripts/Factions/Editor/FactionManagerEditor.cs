#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Factions.com;

namespace GWLPXL.ARPGCore.com
{

    [CustomEditor(typeof(FactionRelations))]
    public class FactionRelationsEditor : UnityEditor.Editor
    {
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            FactionRelations stats = (FactionRelations)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Save Config"))
            {
                JsconConfig.SaveJson(stats);

            }
            if (GUILayout.Button("Load Config"))
            {
                JsconConfig.LoadJson(stats);

            }
            if (GUILayout.Button("Overwrite Config"))
            {
                JsconConfig.OverwriteJson(stats);

            }

          

        }
    }
}
#endif