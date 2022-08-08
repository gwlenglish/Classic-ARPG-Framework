using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com
{
    public class ArpgWithCacheEditor: ArpgBaseEditor
    {
        public Dictionary<Object, UnityEditor.Editor>
            EditorCache = new Dictionary<Object, UnityEditor.Editor>();
        
        private void OnDestroy()
        {
            foreach (var editorCacheValue in EditorCache.Values)
            {
                DestroyImmediate(editorCacheValue);
            }
        }
    }
}