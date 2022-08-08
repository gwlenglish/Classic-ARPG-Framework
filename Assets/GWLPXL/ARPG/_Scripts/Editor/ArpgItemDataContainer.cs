using System;
using GWLPXL.ARPG._Scripts.Editor.Data.com;
using GWLPXL.ARPG._Scripts.Editor.ReloadProcessors.com;
using Object = UnityEngine.Object;

namespace GWLPXL.ARPG._Scripts.Editor.com
{
    public class ArpgItemDataContainer
    {
        private Object _object;
        public Object Object
        {
            get => _object;
            set
            {
                _object = value;
                IsDirty = true;
            }
        }
        public Type Type;
        public YMObjectEditorType EditType;
        public string SavePath;
        public IReloadProcessor ReloadTrigger;
        public bool IsDirty;
        
        public ArpgItemDataContainer(Object obj, string savePath = null, IReloadProcessor reload = null, YMObjectEditorType editorType = YMObjectEditorType.Modify, Type type = null)
        {
            Object = obj;
            SavePath = savePath;
            Type = type == null ? obj.GetType() : type;
            EditType = editorType;
            ReloadTrigger = reload;
        }
    }
}