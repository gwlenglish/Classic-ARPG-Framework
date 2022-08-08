#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace GWLPXL.ARPGCore.ProgressTree.com
{

    [CustomEditor(typeof(ProgressTreeHolder))]
    public class ProgressTreeHolderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(25);
            ProgressTreeHolder holder = (ProgressTreeHolder)target;
            if (GUILayout.Button("Save as NEW Json Config"))
            {
                JSonAttributes.SaveJson(holder);
            }
            if (GUILayout.Button("Load from Json Config"))
            {
                JSonAttributes.LoadJson(holder);
            }
            if (GUILayout.Button("Overwrite Json Config"))
            {
                JSonAttributes.OverwriteJson(holder);
            }
        }
    }
}

#endif