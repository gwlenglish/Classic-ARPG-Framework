using GWLPXL.ARPGCore.Creation.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class CreateQuestgiverEditorModel: ScriptableObject
    {
        public QuestGiverOptions options = new QuestGiverOptions();
        public GWLPXL.ARPGCore.com.GameDatabase GameDatabase;
        
        public string[] AttributesOptions;
        
        public bool AttributesOptionsExpanded;
        
        public void Setup(GWLPXL.ARPGCore.com.GameDatabase gameDatabase)
        {
            GameDatabase = gameDatabase;
            AttributesOptions = GameDatabase.Attributes.GetAllNames();
        }
    }
}