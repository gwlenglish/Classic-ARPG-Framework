#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Items.com
{

    [CustomEditor(typeof(Rarity))]
    public class RarityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Rarity rarity = (Rarity)target;
            if (GUILayout.Button("Save Config"))
            {
                JsconConfig.SaveJson(rarity);

            }
            if (GUILayout.Button("Load Config"))
            {
                JsconConfig.LoadJson(rarity);

            }
            if (GUILayout.Button("Overwrite Config"))
            {
                JsconConfig.OverwriteJson(rarity);

            }
        }
    }
}
#endif