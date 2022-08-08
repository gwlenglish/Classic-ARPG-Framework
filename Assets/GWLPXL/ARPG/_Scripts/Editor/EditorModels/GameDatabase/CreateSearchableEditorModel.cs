using GWLPXL.ARPGCore.Creation.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class CreateSearchableEditorModel: ScriptableObject
    {
        public GWLPXL.ARPGCore.com.GameDatabase GameDatabase;
        public SearchableOptions options = new SearchableOptions();
        
        public string[] LootDropsOptions;
        
        public bool LootDropsOptionsExpanded;
        
        public void Setup(GWLPXL.ARPGCore.com.GameDatabase gameDatabase)
        {
            GameDatabase = gameDatabase;
            LootDropsOptions = GameDatabase.Loot.GetAllNames();
        }
    }
}