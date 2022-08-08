using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Creation.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class EnemyCreateEditor: ArpgWithCacheEditor
    {
        public override void OnInspectorGUI()
        {
            var model = (CreateEnemyEditorModel) target;
            var enemyOptions = model.options;
            
            model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType = 
                (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType);
            var physicsType = model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType;
            EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);

            ArpgEditorHelper.DrawPopupWithLabel(ref enemyOptions.Attributes, "Attributes", model.AttributesOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Attributes.GetDatabaseObjectBySlotIndex(enemyOptions.Attributes),
                ref model.AttributesOptionsExpanded, "Attributes View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawScalingAttributesInFoldout(ref model.ScalingViewerExpanded, "Scaling View", model.GameDatabase,
                (ActorAttributes)model.GameDatabase.Attributes.GetDatabaseObjectBySlotIndex(enemyOptions.Attributes));
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref enemyOptions.AbilityController, "Ability Controller", model.AbilityControllerOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.AbilityControllers.GetDatabaseObjectBySlotIndex(enemyOptions.AbilityController),
                ref model.AbilityControllerOptionsExpanded, "Ability Controller View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref enemyOptions.AuraController, "Aura Controller", model.AuraControllerOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.AuraControllers.GetDatabaseObjectBySlotIndex(enemyOptions.AuraController),
                ref model.AuraControllerOptionsExpanded, "Aura Controller View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref enemyOptions.Inventory, "Inventory", model.InventoryOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Inventories.GetDatabaseObjectBySlotIndex(enemyOptions.Inventory),
                ref model.InventoryOptionsExpanded, "Inventory View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref enemyOptions.Class, "Class", model.ClassOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Classes.GetDatabaseObjectBySlotIndex(enemyOptions.Class),
                ref model.ClassOptionsExpanded, "Class View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref enemyOptions.LootDrops, "Loot Drops", model.LootDropsOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Loot.GetDatabaseObjectBySlotIndex(enemyOptions.LootDrops),
                ref model.LootDropsOptionsExpanded, "Loot Drop View", EditorCache, true);
            EditorGUILayout.Space(5);

            ArpgEditorHelper.DrawHorizontalToggleField(ref enemyOptions.ScaleWithDungeonLevel, "Scale with Dungeon Level?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref enemyOptions.CanTakeDamage, "Can Take Damage?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref enemyOptions.UseCombat, "Can Fight?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref enemyOptions.CanTakeStatusEffects, "Can Take Status Effects?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref enemyOptions.UseAuras, "Can Use Auras?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref enemyOptions.ReceiveAuras, "Can Receive Auras?");
            
            
            ArpgEditorHelper.DrawHorizontalToggleField(ref enemyOptions.UseBuiltInMoving, "Use Built-in Movement?");
            if (enemyOptions.UseBuiltInMoving)
            {
                EditorGUI.indentLevel++;
                switch (physicsType)
                {
                    case EditorPhysicsType.Unity3D:
                        ArpgEditorHelper.DrawPopupWithLabel(ref enemyOptions.Mover3DType, "Move Type", model.MoverTypes3d);
                        break;
                    case EditorPhysicsType.Unity2D:
                        ArpgEditorHelper.DrawPopupWithLabel(ref enemyOptions.Mover2DType, "Move Type", model.MoverTypes2d);
                        break;
                }
                EditorGUI.indentLevel--;
            }
            
            ArpgEditorHelper.DrawHorizontalToggleField(ref enemyOptions.UseAnimatorController, "Use Animator Controller?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref enemyOptions.UseBuiltInHPInfo, "Use HP Bar?");

            if (enemyOptions.UseAnimatorController)
            {
                ArpgEditorHelper.DrawHorizontalObjectField(ref enemyOptions.CharacterMesh, "Character Mesh?");
            }
            
            if (GUILayout.Button("Create"))
            {
                string assetname = model.AttributesOptions[enemyOptions.Attributes] + "_Prefab";
                //ask where to make it
                string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                if (folder.Length == 0)
                {
                    Debug.Log("Did not find a folder for the assets");
                    return;
                }
            
                var returnedAsset = EnemyDefinition.CreateNewEnemyPrefab(folder, enemyOptions, model.GameDatabase);
                Selection.activeObject = returnedAsset;
                EditorGUIUtility.PingObject(returnedAsset);
            }
        }
    }
}