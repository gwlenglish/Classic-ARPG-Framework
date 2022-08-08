#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

using GWLPXL.ARPGCore.Statics.com;


namespace GWLPXL.ARPGCore.com
{


    [CustomEditor(typeof(ProjectSettings))]
    public class ProjectSettingsEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ProjectSettings projectsettings = (ProjectSettings)target;
            if (GUILayout.Button("Save Config"))
            {
                JsconConfig.SaveJson(projectsettings);

            }
            if (GUILayout.Button("Load Config"))
            {
                JsconConfig.LoadJson(projectsettings);

            }
            if (GUILayout.Button("Overwrite Config"))
            {
                JsconConfig.OverwriteJson(projectsettings);

            }
        }





    }
}
#endif