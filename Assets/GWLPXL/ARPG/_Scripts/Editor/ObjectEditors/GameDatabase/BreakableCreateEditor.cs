using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Creation.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class BreakableCreateEditor: ArpgWithCacheEditor
    {
        public override void OnInspectorGUI()
        {
            var model = (CreateBreakableEditorModel) target;
            var breakableOptions = model.options;
            
            model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType = 
                (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType);
            var physicsType = model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType;
            EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);

            ArpgEditorHelper.DrawPopupWithLabel(ref breakableOptions.Attributes, "Attributes", model.AttributesOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Attributes.GetDatabaseObjectBySlotIndex(breakableOptions.Attributes),
                ref model.AttributesOptionsExpanded, "Attributes View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawPopupWithLabel(ref breakableOptions.LootDrops, "Loot Drop", model.LootDropsOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Loot.GetDatabaseObjectBySlotIndex(breakableOptions.LootDrops),
                ref model.LootDropsOptionsExpanded, "Loot Drop View", EditorCache, true);
            EditorGUILayout.Space(5);

            ArpgEditorHelper.DrawPopupWithLabel(ref breakableOptions.HPResourceType, "Health Resource", model.ResourceTypes);
            
            ArpgEditorHelper.DrawHorizontalToggleField(ref breakableOptions.UseScaling, "Use Scaling?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref breakableOptions.UseMouseOverOutline, "Use Mouse Over Highlight?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref breakableOptions.UseGizmoIcons, "Use Custom Gizmos?");
            
            ArpgEditorHelper.DrawHorizontalObjectField(ref breakableOptions.OriginalMesh, "Original Mesh?");
            ArpgEditorHelper.DrawHorizontalObjectField(ref breakableOptions.BrokenVersion, "Broken Version?");
            ArpgEditorHelper.DrawHorizontalObjectField(ref breakableOptions.HighlightVersion, "Highlighted Version?");
            
            if (GUILayout.Button("Create"))
            {
                string assetname = model.AttributesOptions[breakableOptions.Attributes] + "_Prefab";
                //ask where to make it
                string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                if (folder.Length == 0)
                {
                    Debug.Log("Did not find a folder for the assets");
                    return;
                }
            
                UnityEngine.Object returnedAsset = BreakableDefinition.CreateBreakablePrefab(folder, breakableOptions, model.GameDatabase);
                Selection.activeObject = returnedAsset;
                EditorGUIUtility.PingObject(returnedAsset);
            }
        }
    }
}