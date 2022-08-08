using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Creation.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class SearchableCreateEditor: ArpgWithCacheEditor
    {
        public override void OnInspectorGUI()
        {
            var model = (CreateSearchableEditorModel) target;
            var searchableOptions = model.options;
            
            model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType = 
                (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", model.GameDatabase.Settings.Templates.UnityDefaults.PhysicsType);
            EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);

            ArpgEditorHelper.DrawHorizontalTextField(ref searchableOptions.Name, "Name?");
                
            ArpgEditorHelper.DrawPopupWithLabel(ref searchableOptions.LootDrops, "Loot Drops?", model.LootDropsOptions);
            ArpgEditorHelper.DrawInlineEditorInFoldout(
                model.GameDatabase.Loot.GetDatabaseObjectBySlotIndex(searchableOptions.LootDrops),
                ref model.LootDropsOptionsExpanded, "Loot Drop View", EditorCache, true);
            EditorGUILayout.Space(5);
            
            ArpgEditorHelper.DrawHorizontalIntField(ref searchableOptions.UnscaledLevel, "Unscaled level?");
            ArpgEditorHelper.DrawHorizontalFloatField(ref searchableOptions.SearchDistance, "Search distance?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref searchableOptions.UseScaling, "Use Scaling?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref searchableOptions.UseMouseOverOutline, "Use Mouse Over Highlight?");
            ArpgEditorHelper.DrawHorizontalToggleField(ref searchableOptions.UseGizmoIcons, "Use Custom Gizmos?");
            ArpgEditorHelper.DrawHorizontalObjectField(ref searchableOptions.OriginalMesh, "Original Mesh?");
            ArpgEditorHelper.DrawHorizontalObjectField(ref searchableOptions.HighlightVersion, "Highlighted Version?");
            
            
            if (GUILayout.Button("Create"))
            {
            
                string assetname = searchableOptions.Name + "_Prefab";
                //ask where to make it
                string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                if (folder.Length == 0)
                {
                    Debug.Log("Did not find a folder for the assets");
                    return;
                }
            
                UnityEngine.Object returnedAsset = SearchableDefinition.CreateSearchablePrefab(folder, searchableOptions, model.GameDatabase);
                Selection.activeObject = returnedAsset;
                EditorGUIUtility.PingObject(returnedAsset);
            }
        }
    }
}