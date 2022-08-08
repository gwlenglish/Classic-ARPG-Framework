using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com
{
    public class ArpgBaseModifyEditor: ArpgBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(25);
            var holder = (ISaveJsonConfig)target;
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