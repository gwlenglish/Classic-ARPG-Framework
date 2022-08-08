#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Traits.com;

using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.com
{

    [CustomEditor(typeof(EquipmentTrait), true)]
    public class EquipmentTraitEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EquipmentTrait trait = (EquipmentTrait)target;

            GUILayout.Space(25);
            if (GUILayout.Button("Save Config"))
            {
                JsconConfig.SaveJson(trait);

            }
            if (GUILayout.Button("Load Config"))
            {
                JsconConfig.LoadJson(trait);

            }
            if (GUILayout.Button("Overwrite Config"))
            {
                JsconConfig.OverwriteJson(trait);

            }

        }
    }
}

#endif