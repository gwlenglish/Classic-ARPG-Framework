#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
namespace GWLPXL.ARPGCore.com
{

    [CustomEditor(typeof(CodeDocumentSO))]
    public class CodeDocumentSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Update Code Document"))
            {
                CodeDocumentSO doc = target as CodeDocumentSO;
                doc.UpdateDocumentation();
            }
        }
    }
}
#endif