using System;
using System.Collections.Generic;
using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Traits.com;
using Object = UnityEngine.Object;

namespace GWLPXL.ARPG._Scripts.Editor.Data.com
{
    public class ArpgObjectEditors
    {
        public Dictionary<(Type, YMObjectEditorType), Type> ObjectToEditorType
            = new Dictionary<(Type, YMObjectEditorType), Type>()
        {
            //all simple types for ArpgBaseModifyEditor and ArpgBaseCreateEditor
            {(typeof(Ability), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(Ability), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(AbilityController), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(AbilityController), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(ActorAttributes), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(ActorAttributes), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(AuraController), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(AuraController), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(Aura), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(Aura), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(ActorClass), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(ActorClass), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(ActorDamageData), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(ActorDamageData), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(MeleeData), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(MeleeData), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(ProjectileData), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(ProjectileData), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(ActorInventory), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(ActorInventory), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(LootDrops), YMObjectEditorType.Modify), typeof(LootModifyEditor)},
            {(typeof(LootDrops), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(Quest), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(Quest), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(Questchain), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(Questchain), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            {(typeof(QuestLog), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(QuestLog), YMObjectEditorType.CreateNew), typeof(ArpgBaseCreateEditor)},
            
            // hard ui
            {(typeof(Item), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(Item), YMObjectEditorType.CreateNew), typeof(ItemCreateEditor)},
            {(typeof(EquipmentTrait), YMObjectEditorType.Modify), typeof(ArpgBaseModifyEditor)},
            {(typeof(EquipmentTrait), YMObjectEditorType.CreateNew), typeof(TraitCreateEditor)},
            
            {(typeof(GameDatabase), YMObjectEditorType.Modify), typeof(GameDatabaseModifyEditor)},
            {(typeof(GameDatabase), YMObjectEditorType.CreateNew), typeof(GameDatabaseCreateEditor)},
            
            // all game database utilities with hard ui
            {(typeof(TypeViewerEditorModel), YMObjectEditorType.Modify), typeof(TypeViewerEditor)},
            {(typeof(EquipmentViewerEditorModel), YMObjectEditorType.Modify), typeof(EquipmentViewerEditor)},
            {(typeof(CreatePlayerEditorModel), YMObjectEditorType.Modify), typeof(PlayerCreateEditor)},
            {(typeof(CreateEnemyEditorModel), YMObjectEditorType.Modify), typeof(EnemyCreateEditor)},
            {(typeof(CreateBreakableEditorModel), YMObjectEditorType.Modify), typeof(BreakableCreateEditor)},
            {(typeof(CreateQuestgiverEditorModel), YMObjectEditorType.Modify), typeof(QuestgiverCreateEditor)},
            {(typeof(CreateSearchableEditorModel), YMObjectEditorType.Modify), typeof(SearchableCreateEditor)},
            {(typeof(CreateShopkeeperEditorModel), YMObjectEditorType.Modify), typeof(ShopkeeperCreateEditor)},
            {(typeof(CreateWearableEditorModel), YMObjectEditorType.Modify), typeof(WearableCreateEditor)},
            
            // all databases
            {(typeof(AbilitiesDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(AbilityControllerDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(ActorAttributesDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(ActorDamageDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(AuraDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(AuraControllerDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(ActorClassDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(InventoryDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(ItemDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(LootDropsDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(MeleeDataDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(ProjectileDataDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(QuestchainDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(QuestDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(QuestLogDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
            {(typeof(EquipmentTraitDatabase), YMObjectEditorType.Modify), typeof(ArpgBaseDatabaseEditor)},
        };

        public Dictionary<ArpgItemDataContainer, UnityEditor.Editor> CachedEditors 
            = new Dictionary<ArpgItemDataContainer, UnityEditor.Editor>();
        
        private Type SelectEditorType(Type selectedType, YMObjectEditorType editorType)
        {
            return ObjectToEditorType.TryGetValue((selectedType, editorType), out var value) ? value : null;;
        }

        public UnityEditor.Editor GetCustomEditor(ArpgItemDataContainer container)
        {
            UnityEditor.Editor editor = null;
            if (CachedEditors.TryGetValue(container, out editor) && !container.IsDirty) return editor;
            if (editor != null)
                Object.DestroyImmediate(editor);
                
            // create if not exist
            var editorType = SelectEditorType(container.Type, container.EditType);
            editor = UnityEditor.Editor.CreateEditor(container.Object, editorType);

            if (editor is ArpgBaseEditor baseEditor)
            {
                baseEditor.SetContainer(container);
            }
            
            CachedEditors[container] = editor;
            container.IsDirty = false;
            return editor;
        }
    }

    public enum YMObjectEditorType
    {
        CreateNew,
        Modify
    }
}