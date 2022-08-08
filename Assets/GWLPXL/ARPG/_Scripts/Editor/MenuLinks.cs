using UnityEditor;

namespace GWLPXL.ARPG._Scripts.Editor.com
{
    public class MenuLinks
    {
        [MenuItem("GWLPXL/ARPG Editor")]
        public static void OpenWindow()
        {
            ArpgEditorWindow window = EditorWindow.GetWindow<ArpgEditorWindow>("ARPG Editor");
            window.Show();
        }
    }
}