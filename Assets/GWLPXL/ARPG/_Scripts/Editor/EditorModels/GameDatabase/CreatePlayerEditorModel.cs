using GWLPXL.ARPGCore.Creation.com;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com
{
    public class CreatePlayerEditorModel: ScriptableObject
    {
        public string[] AttributesOptions;
        public string[] AbilityControllerOptions;
        public string[] AuraControllerOptions;
        public string[] InventoryOptions;
        public string[] ClassOptions;
        public string[] QuestLogOptions;
        
        public string[] MoverTypes2d = { "StateMachine", "Simple Topdown" };
        public string[] MoverTypes3d = { "NavMeshMouse" };
        public string[] InteractTypes2d = { "Mouse Input", "OnTriggerEnter"};
        public string[] InteractTypes3d = { "Mouse Input" };
        public GWLPXL.ARPGCore.com.GameDatabase GameDatabase;

        public PlayerOptions options = new PlayerOptions();
        
        public bool AttributesOptionsExpanded;
        public bool AbilityControllerOptionsExpanded;
        public bool AuraControllerOptionsExpanded;
        public bool InventoryOptionsExpanded;
        public bool ClassOptionsExpanded;
        public bool QuestLogOptionsExpanded;
        public bool ScalingViewerExpanded;

        public void Setup(GWLPXL.ARPGCore.com.GameDatabase gameDatabase)
        {
            GameDatabase = gameDatabase;
            AttributesOptions = GameDatabase.Attributes.GetAllNames();
            AbilityControllerOptions = GameDatabase.AbilityControllers.GetAllNames();
            AuraControllerOptions = GameDatabase.AuraControllers.GetAllNames();
            InventoryOptions = GameDatabase.Inventories.GetAllNames();
            ClassOptions = GameDatabase.Classes.GetAllNames();
            QuestLogOptions = GameDatabase.QuestLog.GetAllNames();
        }
    }
}