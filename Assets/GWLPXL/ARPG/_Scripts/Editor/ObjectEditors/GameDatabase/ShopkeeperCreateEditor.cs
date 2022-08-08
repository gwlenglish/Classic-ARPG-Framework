using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Creation.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class ShopkeeperCreateEditor: ArpgWithCacheEditor
    {
        public override void OnInspectorGUI()
        {
            var model = (CreateShopkeeperEditorModel) target;
            var shopkeeperOptions = model.options;
            
            model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType = 
                (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType);
            var physicsType = model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType;
            EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);

            ArpgEditorHelper.DrawPopupWithLabel(ref shopkeeperOptions.Attributes, "Attributes", model.AttributesOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Attributes.GetDatabaseObjectBySlotIndex(shopkeeperOptions.Attributes),
                ref model.AttributesOptionsExpanded, "Attributes View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref shopkeeperOptions.Inventory, "Inventory", model.InventoryOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Inventories.GetDatabaseObjectBySlotIndex(shopkeeperOptions.Inventory),
                ref model.InventoryOptionsExpanded, "Inventory View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref shopkeeperOptions.LootDrops, "Store Table", model.LootDropsOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Loot.GetDatabaseObjectBySlotIndex(shopkeeperOptions.LootDrops),
                ref model.LootDropsOptionsExpanded, "Store Table View", EditorCache, true);
            EditorGUILayout.Space(5);

            ArpgEditorHelper.DrawHorizontalToggleField(ref shopkeeperOptions.UseBuiltInCanvas, "Use Builtin Canvas?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref shopkeeperOptions.ShopScalesWithDungeonLevel, "Shop scales?");
            ArpgEditorHelper.DrawHorizontalIntField(ref shopkeeperOptions.ItemRolls, "Shop rolls?");
            
            ArpgEditorHelper.DrawHorizontalObjectField(ref shopkeeperOptions.CharacterMesh, "Character Mesh?");
            
            if (GUILayout.Button("Create"))
            {
                string assetname =  model.AttributesOptions[shopkeeperOptions.Attributes] + "_Prefab";
                //ask where to make it
                string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                if (folder.Length == 0)
                {
                    Debug.Log("Did not find a folder for the assets");
                    return;
                }
            
                UnityEngine.Object returnedAsset = ShopKeeperDefinition.CreateNewShopKeeperPrefab(folder, shopkeeperOptions, model.GameDatabase);
                Selection.activeObject = returnedAsset;
                EditorGUIUtility.PingObject(returnedAsset);
            }
        }
    }
}