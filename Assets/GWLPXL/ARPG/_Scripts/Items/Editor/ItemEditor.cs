#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.com
{

    [CustomEditor(typeof(Item), true)]
    public class ItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Item item = (Item)target;
            GUILayout.Space(25);
            if (GUILayout.Button("Save Config"))
            {
                JsconConfig.SaveJson(item);

            }
            if (GUILayout.Button("Load Config"))
            {
                JsconConfig.LoadJson(item);

            }
            if (GUILayout.Button("Overwrite Config"))
            {
                JsconConfig.OverwriteJson(item);

            }
        }
    }
}
#endif