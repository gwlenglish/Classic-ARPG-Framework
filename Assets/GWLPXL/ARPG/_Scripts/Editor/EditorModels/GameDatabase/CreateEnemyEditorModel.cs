using GWLPXL.ARPGCore.Creation.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class CreateEnemyEditorModel: ScriptableObject
    {
        public string[] AttributesOptions;
        public string[] AbilityControllerOptions;
        public string[] AuraControllerOptions;
        public string[] InventoryOptions;
        public string[] ClassOptions;
        public string[] LootDropsOptions;
        
        public string[] MoverTypes2d = { "StateMachine", "Simple Topdown" };
        public string[] MoverTypes3d = { "NavMeshMouse" };
        
        public GWLPXL.ARPGCore.com.GameDatabase GameDatabase;

        public EnemyOptions options = new EnemyOptions();
        
        public bool AttributesOptionsExpanded;
        public bool AbilityControllerOptionsExpanded;
        public bool AuraControllerOptionsExpanded;
        public bool InventoryOptionsExpanded;
        public bool ClassOptionsExpanded;
        public bool LootDropsOptionsExpanded;
        public bool ScalingViewerExpanded;

        private void OnEnable()
        {
        }

        public void Setup(GWLPXL.ARPGCore.com.GameDatabase gameDatabase)
        {
            GameDatabase = gameDatabase;
            AttributesOptions = GameDatabase.Attributes.GetAllNames();
            AbilityControllerOptions = GameDatabase.AbilityControllers.GetAllNames();
            AuraControllerOptions = GameDatabase.AuraControllers.GetAllNames();
            InventoryOptions = GameDatabase.Inventories.GetAllNames();
            ClassOptions = GameDatabase.Classes.GetAllNames();
            LootDropsOptions = GameDatabase.Loot.GetAllNames();
        }
    }
}