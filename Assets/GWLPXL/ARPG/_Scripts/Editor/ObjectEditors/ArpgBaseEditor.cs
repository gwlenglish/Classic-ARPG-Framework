using GWLPXL.ARPG._Scripts.Editor.com;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com
{
    public class ArpgBaseEditor: UnityEditor.Editor
    {
        public ArpgItemDataContainer AttachedContainer;
        public void SetContainer(ArpgItemDataContainer container)
        {
            AttachedContainer = container;
        }
    }
}