using System;
using System.Collections.Generic;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEditor;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class TypeViewerEditor: ArpgBaseEditor
    {
        private List<Type> ViewEnums = new List<Type>()
        {
            typeof(AbilityCategory), typeof(AccessoryType), typeof(ArmorMaterial),
            typeof(AttributeType),
            typeof(AuraCategory), typeof(AuraTargetGroup), typeof(CanvasType), typeof(ClassType), typeof(ColliderType),
            typeof(CombatGroupType), typeof(CombatStatType), typeof(DebugMessages), typeof(ElementType),
            typeof(EnemyState),
            typeof(EquipmentSlotsType), typeof(EquipmentType), typeof(FactionTypes), typeof(ItemRarityType),
            typeof(ItemType),
            typeof(OtherAttributeType), typeof(PotionType), typeof(QuestRewardTurnInType),
            typeof(QuestStartConditionType),
            typeof(QuestStatusType), typeof(ResourceType), typeof(StatType), typeof(TraitType), typeof(WeaponType)
        };

        private void OnEnable()
        {
            var model = (TypeViewerEditorModel) target;
            if (model.Foldout.Count != 0) return;
            foreach (var @enum in ViewEnums)
            {
                model.Foldout[@enum] = false;
            }
        }

        public override void OnInspectorGUI()
        {
            var model = (TypeViewerEditorModel) target;
            EditorGUILayout.LabelField("To add or remove types, click on the TypePing object and modify the appropriate script");
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Type ping object:", model.PingObject, typeof(PingObject), false);
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.LabelField("Types:");
            
            foreach (var @enum in ViewEnums)
            {
                var names = Enum.GetNames(@enum);

                model.Foldout[@enum] = EditorGUILayout.BeginFoldoutHeaderGroup(model.Foldout[@enum], @enum.Name);

                if (model.Foldout[@enum])
                {
                    foreach (var value in names)
                    {
                        EditorGUILayout.LabelField(value);
                    }
                }
                
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }
    }
}