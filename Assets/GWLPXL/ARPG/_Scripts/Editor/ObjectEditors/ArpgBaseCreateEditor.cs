using GWLPXL.ARPG._Scripts.Editor.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com
{
    public class ArpgBaseCreateEditor: ArpgBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(25);
            
            if (GUILayout.Button("Create as New"))
            {
                GUI.FocusControl(null);
                var name = GetName();
                var asset = ArpgEditorHelper.TryCreateNew(name, target, AttachedContainer.SavePath);
                
                AttachedContainer.ReloadTrigger?.Reload();
                MakeBlankCopy();
            }
        }

        public virtual string GetName()
        {
            return ArpgEditorHelper.GetNameOfArpgObject(target);
        }
        public virtual void MakeBlankCopy()
        {
            AttachedContainer.Object = CreateInstance(target.GetType());
        }
    }
}