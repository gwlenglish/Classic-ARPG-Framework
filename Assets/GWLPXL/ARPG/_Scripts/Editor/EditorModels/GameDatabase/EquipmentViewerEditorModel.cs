using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class EquipmentViewerEditorModel: ScriptableObject
    {
        public GWLPXL.ARPGCore.com.GameDatabase GameDatabase;
        public bool EquipmentExpanded;
        public bool GeneratedViewExpanded;
        public bool TraitsViewExpanded;
        public int SelectedTrait;

        public void Setup(GWLPXL.ARPGCore.com.GameDatabase database)
        {
            GameDatabase = database;
        }
    }
}