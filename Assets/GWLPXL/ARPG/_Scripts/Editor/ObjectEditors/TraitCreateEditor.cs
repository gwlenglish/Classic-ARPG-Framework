using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com
{
    public class TraitCreateEditor: ArpgBaseEditor
    {
        public override void OnInspectorGUI()
        {
            var obj = (CreateTrait)target;
            
            obj.type = (TraitType)EditorGUILayout.EnumPopup("Trait type to create: ", obj.type);
            obj.traitName = EditorGUILayout.TextField("Trait name: ", obj.traitName);
            
            GUILayout.Space(25);
            
            if (GUILayout.Button("Create as New"))
            {
                GUI.FocusControl(null);

                EquipmentTrait temp = null;
                if (string.IsNullOrEmpty(obj.traitName))
                {
                    EditorUtility.DisplayDialog("Name Required", "A name is required in order to create new", "Okay");
                    return;
                }
                
                switch (obj.type)
                {
                    case TraitType.Stat:
                        temp = ScriptableObject.CreateInstance<StatTraitModifier>();
                        break;
                    case TraitType.Resource:
                        temp = ScriptableObject.CreateInstance<MaxResourceTraitModifier>();
                        break;
                    case TraitType.ElementAttack:
                        temp = ScriptableObject.CreateInstance<ElementAttackTraitModifier>();
                        break;
                    case TraitType.ElementResist:
                        temp = ScriptableObject.CreateInstance<ElementResistTraitModifier>();
                        break;
                    case TraitType.AbilityMod:
                        EditorUtility.DisplayDialog("Not Implemented", "TraitType.AbilityMod is not implemented yet", "Okay");
                        break;
                    //case TraitType.AbilityMod:
                    //    temp = ScriptableObject.CreateInstance<StatTrait>();//not yet implemented
                    //    break;
                }
                
                if (temp == null)
                {
                    EditorUtility.DisplayDialog("Type Required", "A name and type is both required.", "Okay");
                    return;
                }
                temp.SetTraitName(obj.traitName);
                var asset = ArpgEditorHelper.TryCreateNew(obj.traitName, temp, AttachedContainer.SavePath);
                AttachedContainer.ReloadTrigger?.Reload();
                
                obj.traitName = string.Empty;
            }
        }
    }
}