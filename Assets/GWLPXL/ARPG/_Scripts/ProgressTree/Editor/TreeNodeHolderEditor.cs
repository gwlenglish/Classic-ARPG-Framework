#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;

namespace GWLPXL.ARPGCore.ProgressTree.com
{


    [CustomEditor(typeof(TreeNodeHolder))]
    public class TreeNodeHolderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(25);
            TreeNodeHolder holder = (TreeNodeHolder)target;
            if (GUILayout.Button("Save as NEW Json Config"))
            {
                JSonAttributes.SaveJson(holder);
            }
            if (GUILayout.Button("Load Json Config"))
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