using GWLPXL.ARPGCore.Creation.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class CreateShopkeeperEditorModel: ScriptableObject
    {
        public GWLPXL.ARPGCore.com.GameDatabase GameDatabase;
        public ShopkeeperOptions options = new ShopkeeperOptions();
        
        public string[] AttributesOptions;
        public string[] InventoryOptions;
        public string[] LootDropsOptions;
        
        public bool AttributesOptionsExpanded;
        public bool InventoryOptionsExpanded;
        public bool LootDropsOptionsExpanded;
        
        public void Setup(GWLPXL.ARPGCore.com.GameDatabase gameDatabase)
        {
            GameDatabase = gameDatabase;
            AttributesOptions = GameDatabase.Attributes.GetAllNames();
            InventoryOptions = GameDatabase.Inventories.GetAllNames();
            LootDropsOptions = GameDatabase.Loot.GetAllNames();
        }
    }
}