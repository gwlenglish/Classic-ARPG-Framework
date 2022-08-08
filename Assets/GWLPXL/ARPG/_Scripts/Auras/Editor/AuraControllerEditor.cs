#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;


namespace GWLPXL.ARPGCore.Auras.com
{

    [CustomEditor(typeof(AuraController))]
    public class AuraControllerEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(25);
            AuraController holder = (AuraController)target;
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
