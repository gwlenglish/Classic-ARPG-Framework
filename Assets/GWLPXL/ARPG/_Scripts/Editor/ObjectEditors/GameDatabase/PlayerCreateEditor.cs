using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Creation.com;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class PlayerCreateEditor: ArpgWithCacheEditor
    {
        public override void OnInspectorGUI()
        {
            var model = (CreatePlayerEditorModel) target;
            var playerOptions = model.options;
            
            model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType = 
                (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType);
            var physicsType = model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType;
            EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);

            ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.Attributes, "Attributes", model.AttributesOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Attributes.GetDatabaseObjectBySlotIndex(playerOptions.Attributes),
                ref model.AttributesOptionsExpanded, "Attributes View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawScalingAttributesInFoldout(ref model.ScalingViewerExpanded, "Scaling View", model.GameDatabase,
                (ActorAttributes)model.GameDatabase.Attributes.GetDatabaseObjectBySlotIndex(playerOptions.Attributes));
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.AbilityController, "Ability Controller", model.AbilityControllerOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.AbilityControllers.GetDatabaseObjectBySlotIndex(playerOptions.AbilityController),
                ref model.AbilityControllerOptionsExpanded, "Ability Controller View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.AuraController, "Aura Controller", model.AuraControllerOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.AuraControllers.GetDatabaseObjectBySlotIndex(playerOptions.AuraController),
                ref model.AuraControllerOptionsExpanded, "Aura Controller View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.Inventory, "Inventory", model.InventoryOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Inventories.GetDatabaseObjectBySlotIndex(playerOptions.Inventory),
                ref model.InventoryOptionsExpanded, "Inventory View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.Class, "Class", model.ClassOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Classes.GetDatabaseObjectBySlotIndex(playerOptions.Class),
                ref model.ClassOptionsExpanded, "Class View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.QuestLog, "Quest Log", model.QuestLogOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.QuestLog.GetDatabaseObjectBySlotIndex(playerOptions.QuestLog),
                ref model.QuestLogOptionsExpanded, "Quest Log View", EditorCache, true);
            EditorGUILayout.Space(5);

            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.UseCombat, "Can Fight?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.CanTakeDamage, "Can Take Damage?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.CanTakeStatusEffects, "Can Take Status Effects?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.UseLeveling, "Can Level Up?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.UseAuras, "Can Use Auras?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.ReceiveAuras, "Can Receive Auras?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.UseQuests, "Can Receive Quests?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.UseShopping, "Can Buy and Sell Items?");
            
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.UseBuiltInMoving, "Use Built-in Movement?");
            if (playerOptions.UseBuiltInMoving)
            {
                EditorGUI.indentLevel++;
                switch (physicsType)
                {
                    case EditorPhysicsType.Unity3D:
                        ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.Mover3DType, "Move Type", model.MoverTypes3d);
                        break;
                    case EditorPhysicsType.Unity2D:
                        ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.Mover2DType, "Move Type", model.MoverTypes2d);
                        break;
                }
                EditorGUI.indentLevel--;
            }
            
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.UseBuiltInInteraction, "Use Built-in Interaction Detection?");
            if (playerOptions.UseBuiltInInteraction)
            {
                EditorGUI.indentLevel++;
                switch (physicsType)
                {
                    case EditorPhysicsType.Unity3D:
                        ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.Interact3DType, "Interact Type", model.InteractTypes3d);
                        break;
                    case EditorPhysicsType.Unity2D:
                        ArpgEditorHelper.DrawPopupWithLabel(ref playerOptions.Interact2DType, "Interact Type", model.InteractTypes2d);
                        break;
                }
                EditorGUI.indentLevel--;
            }

            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.UseBuiltInCanvases, "Use Built-in Canvas UI?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.UseAnimatorController, "Use Built-in Animator Controller?");

            if (playerOptions.UseAnimatorController)
            {
                ArpgEditorHelper.DrawHorizontalObjectField(ref playerOptions.CharacterMesh, "Character Mesh?");
            }
            EditorGUILayout.HelpBox("Modify Existing will add the components to an already created prefab. Use this for things like InvectorLite where you make the controller first, then modify it here.", MessageType.Info);
            ArpgEditorHelper.DrawHorizontalToggleField(ref playerOptions.ModifyExisting, "Modify Existing GameObject?");

            if (playerOptions.ModifyExisting)
            {
                ArpgEditorHelper.DrawHorizontalObjectField(ref playerOptions.PrefabToModify, "Prefab to clone and modify?");
            }

            if (GUILayout.Button(playerOptions.ModifyExisting ? "Modify" : "Create"))
            {
                string assetname = model.AttributesOptions[playerOptions.Attributes] + "_Prefab";
                //ask where to make it
                string path = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                if (path.Length == 0)
                {
                    Debug.Log("Did not find a folder for the assets");
                    return;
                }
            
                if (playerOptions.ModifyExisting && playerOptions.PrefabToModify == null)
                {
                    Debug.Log("Can not modify without an object to modify");
                    return;
                }
                Object returnedAsset = ActorDefinitions.CreateNewPlayerPrefab(path, playerOptions, model.GameDatabase);
                Selection.activeObject = returnedAsset;
                EditorGUIUtility.PingObject(returnedAsset);
            }
        }
    }
}