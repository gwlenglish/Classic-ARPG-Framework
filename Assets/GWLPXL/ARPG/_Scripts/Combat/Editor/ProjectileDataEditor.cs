#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Combat.com
{

    [CustomEditor(typeof(ProjectileData))]
    public class ProjectileDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(25);
            ProjectileData holder = (ProjectileData)target;
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