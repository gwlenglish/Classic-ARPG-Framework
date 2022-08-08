
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GWLPXL.ARPGCore.States.com
{
    /// <summary>
    /// Allows for easy grouping. original source: https://answers.unity.com/questions/118306/grouping-objects-in-the-hierarchy.html
    /// </summary>
    
    public static class GroupCommand
    {
        [MenuItem("GameObject/Group Selected %g")]
        private static void GroupSelected()
        {
            if (!Selection.activeTransform) return;
            var go = new GameObject(Selection.activeTransform.name + " Group");
            Undo.RegisterCreatedObjectUndo(go, "Group Selected");
            go.transform.SetParent(Selection.activeTransform.parent, false);
            foreach (var transform in Selection.transforms) Undo.SetTransformParent(transform, go.transform, "Group Selected");
            Selection.activeGameObject = go;
        }
    }
}
#endif
