using System;
using GWLPXL.ARPGCore.Creation.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class CreateBreakableEditorModel: ScriptableObject
    {
        public GWLPXL.ARPGCore.com.GameDatabase GameDatabase;
        public BreakableOptions options = new BreakableOptions();
        
        public string[] AttributesOptions;
        public string[] LootDropsOptions;
        public string[] ResourceTypes;

        
        public bool AttributesOptionsExpanded;
        public bool LootDropsOptionsExpanded;
        public void Setup(GWLPXL.ARPGCore.com.GameDatabase gameDatabase)
        {
            GameDatabase = gameDatabase;
            AttributesOptions = GameDatabase.Attributes.GetAllNames();
            LootDropsOptions = GameDatabase.Loot.GetAllNames();
            ResourceTypes = Enum.GetNames(typeof(ResourceType));
        }
    }
}