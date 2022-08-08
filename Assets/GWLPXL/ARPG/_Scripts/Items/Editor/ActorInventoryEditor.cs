#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Items.com;

namespace GWLPXL.ARPGCore.com
{

    [CustomEditor(typeof(ActorInventory))]
    public class ActorInventoryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ActorInventory inv = (ActorInventory)target;
            if (GUILayout.Button("Save Config"))
            {
                JsconConfig.SaveJson(inv);

            }
            if (GUILayout.Button("Load Config"))
            {
                JsconConfig.LoadJson(inv);

            }
            if (GUILayout.Button("Overwrite Config"))
            {
                JsconConfig.OverwriteJson(inv);

            }
        }
    }
}
#endif