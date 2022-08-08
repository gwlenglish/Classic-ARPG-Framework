using System.Collections.Generic;
using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Traits.com;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.ARPG._Scripts.Editor.ObjectEditors.GameDatabase.com
{
    public class EquipmentViewerEditor: ArpgWithCacheEditor
    {
        public override void OnInspectorGUI()
        {
            var model = (EquipmentViewerEditorModel) target;
            var gamedatabase = model.GameDatabase;
            
            gamedatabase.Settings.InspectObjects.Equipment.Equipment =
                (Equipment)EditorGUILayout.ObjectField(gamedatabase.Settings.InspectObjects.Equipment.Equipment, typeof(Equipment), false);
            
            string name = "";
            string json = "";

            EquipmentViewOptions eqinspectview = gamedatabase.Settings.InspectObjects.Equipment;
            if (eqinspectview.Equipment != null)
            {
                Equipment equipmentfocus = eqinspectview.Equipment;
                name = equipmentfocus.GetUserDescription();
                json = equipmentfocus.GetTextAsset().ToString();
                ArpgEditorHelper.DrawHorizontalTextField(ref name, "UserDescription");
                ArpgEditorHelper.DrawHorizontalTextField(ref json, "Json string");
                ArpgEditorHelper.DrawHorizontalIntField(ref gamedatabase.Settings.GeneratedTemp.Equipment.ILevel, "Item Level");
                //gamedatabase.Settings.GeneratedTemp.Equipment.ILevel = EditorGUILayout.IntField(, EditorStyles.miniTextField);
                if (GUILayout.Button("Generate Random", EditorStyles.toolbarButton))
                {
                    model.SelectedTrait = 0;//reset selection
                    //reset generated temp
                    GenerateOptions eqgenerateoptions = gamedatabase.Settings.GeneratedTemp;
                    eqgenerateoptions.Equipment.PowerCurves = new List<PowerCurves>();
                    eqgenerateoptions.Equipment.CurvedEq = new Equipment[gamedatabase.Settings.GeneratedTemp.Equipment.MaxILevelCurve];
                    eqgenerateoptions.Equipment.ScollableEq = Vector2.zero;
                    eqgenerateoptions.Equipment.Equipment = null;

                    eqgenerateoptions.Equipment.Equipment = ScriptableObject.Instantiate(equipmentfocus);
                    eqgenerateoptions.Equipment.Equipment.AssignEquipmentTraits(eqgenerateoptions.Equipment.ILevel);

                    //create copy at each level
                    for (int i = 0; i < gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq.Length; i++)
                    {

                        eqgenerateoptions.Equipment.CurvedEq[i] = Instantiate(gamedatabase.Settings.InspectObjects.Equipment.Equipment);
                        eqgenerateoptions.Equipment.CurvedEq[i].AssignEquipmentTraits(i);

                    }

                    //for display graphs
                   
                    string description = "Base Stat";
                    AnimationCurve curve = new AnimationCurve();                             
                    for (int i = 0; i < 100; i++)
                    {
                        Keyframe newframe = new Keyframe(i, gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[i].GetStats().GetBaseStat());
                        curve.AddKey(newframe);
                    }
                    PowerCurves powerCurve = new PowerCurves(curve,description);
                    eqgenerateoptions.Equipment.AddPowerCurve(powerCurve);

                   
                    string traitd = "First Trait";
                    AnimationCurve traitcurve = new AnimationCurve();
                    for (int i = 0; i < 100; i++)
                    {
                        if (gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[i].GetStats().GetRandomTraits() == null || gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[i].GetStats().GetRandomTraits().Length == 0) continue;
                        Keyframe newframe = new Keyframe(i, gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[i].GetStats().GetRandomTraits()[0].GetLeveledValue());
                        traitcurve.AddKey(newframe);
                    }
                    PowerCurves traitpowercurve = new PowerCurves(traitcurve, traitd);
                    eqgenerateoptions.Equipment.AddPowerCurve(traitpowercurve);

                }
                ArpgEditorHelper.DrawInlineEditorInFoldout(equipmentfocus, ref model.EquipmentExpanded, "Equipment View", EditorCache);
            }


            GUILayout.Space(5);


            //GUILayout.EndScrollView();
            //GUILayout.EndArea();

            //generated subwindow
            //Rect inspectoreq = new Rect(700, 240, 280, 720);
            //GUILayout.BeginArea(inspectoreq);
            //gamedatabase.Settings.GeneratedTemp.Equipment.ScollableEq = EditorGUILayout.BeginScrollView(gamedatabase.Settings.GeneratedTemp.Equipment.ScollableEq, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            model.GeneratedViewExpanded =
                EditorGUILayout.BeginFoldoutHeaderGroup(model.GeneratedViewExpanded, "Generated View");
            if (model.GeneratedViewExpanded)
            {
                if (gamedatabase.Settings.GeneratedTemp.Equipment.Equipment != null && gamedatabase.Settings.InspectObjects.Equipment != null && gamedatabase.Settings.GeneratedTemp.Equipment.PowerCurves.Count > 0)
                {
                    Equipment generated = gamedatabase.Settings.GeneratedTemp.Equipment.Equipment;
                    EditorGUILayout.LabelField("User Description", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(generated.GetUserDescription(), EditorStyles.wordWrappedLabel);
                    EditorGUILayout.LabelField("Equipment Name", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(generated.GetGeneratedItemName(), EditorStyles.wordWrappedLabel);
                    EditorGUILayout.LabelField("Equipment Base Description", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(generated.GetBaseTypeDescription(), EditorStyles.wordWrappedLabel);
                    EditorGUILayout.LabelField("Equipment Rarity", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(generated.GetRarityDescription(), EditorStyles.wordWrappedLabel);
                    EditorGUILayout.LabelField("Native Traits Description", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(generated.GetNativeTraitDescription(), EditorStyles.wordWrappedLabel);
                    EditorGUILayout.LabelField("Random Traits Description", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(generated.GetRandomTraitsDescription(), EditorStyles.wordWrappedLabel);
                    //GUILayout.Space(50);
                    //Editor editor = Editor.CreateEditor(gamedatabase.Settings.GeneratedTemp.Equipment.Equipment);
                    //editor.DrawDefaultInspector();
                    EditorGUILayout.LabelField("Power Curve Base Stat (Damage/Armor)", EditorStyles.boldLabel);
                    gamedatabase.Settings.GeneratedTemp.Equipment.PowerCurves[0].Curve = (AnimationCurve)EditorGUILayout.CurveField(gamedatabase.Settings.GeneratedTemp.Equipment.PowerCurves[0].Curve);
                    EditorGUILayout.LabelField("I Level 1", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[1].GetStats().GetBaseStat().ToString(), EditorStyles.wordWrappedLabel);
                    EditorGUILayout.LabelField("I Level 100", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq.Length - 1].GetStats().GetBaseStat().ToString(), EditorStyles.wordWrappedLabel);
                    EditorGUILayout.LabelField("I Level 1 Converted to Int", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[1].GetStats().GetBaseStateConverted().ToString(), EditorStyles.wordWrappedLabel);
                    EditorGUILayout.LabelField("I Level 100 Converted to Int", EditorStyles.boldLabel);
                    EditorGUILayout.TextArea(gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq.Length - 1].GetStats().GetBaseStateConverted().ToString(), EditorStyles.wordWrappedLabel);

                    //move this to a seperate thing where we input the triat
                    //EditorGUILayout.LabelField("Power Curve First Random Trait", EditorStyles.boldLabel);
                    //gamedatabase.Settings.GeneratedTemp.Equipment.PowerCurves[1].Curve = (AnimationCurve)EditorGUILayout.CurveField(gamedatabase.Settings.GeneratedTemp.Equipment.PowerCurves[1].Curve);
                    //EditorGUILayout.LabelField("I Level 1", EditorStyles.boldLabel);
                    //EditorGUILayout.TextArea(gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[1].GetStats().GetRandomTraits()[0].GetLeveledValue().ToString(), EditorStyles.wordWrappedLabel);
                    //EditorGUILayout.LabelField("I Level 100", EditorStyles.boldLabel);
                    //EditorGUILayout.TextArea(gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq[gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq.Length - 1].GetStats().GetRandomTraits()[0].GetLeveledValue().ToString(), EditorStyles.wordWrappedLabel);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            GUILayout.Space(5);
            
            model.TraitsViewExpanded =
                EditorGUILayout.BeginFoldoutHeaderGroup(model.TraitsViewExpanded, "Traits View");

            if (model.TraitsViewExpanded)
            {
                if (gamedatabase.Settings.GeneratedTemp.Equipment.Equipment != null )
                {
                    GUILayout.Label("Native Traits", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllNativeTraitNames().Length; i++)
                    {
                        EditorGUILayout.LabelField(gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllNativeTraitNames()[i], EditorStyles.miniLabel);
                    }
                    EditorGUI.indentLevel--;
                    //
                    GUILayout.Label("Random Traits", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllRandomTraitNames().Length; i++)
                    {
                        EditorGUILayout.LabelField(gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllRandomTraitNames()[i], EditorStyles.miniLabel);
                    }
                    EditorGUI.indentLevel--;
                    
                    ArpgEditorHelper.DrawPopupWithLabel(ref model.SelectedTrait, "Trait View", gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllTraitNames());
                    
                    if (gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetStats().GetAllTraits().Length > 0)
                    {
                        var trait =
                            gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetStats().GetAllTraits()[model.SelectedTrait];
                        EditorGUILayout.ObjectField(trait, typeof(EquipmentTrait), false);
                        
                        ArpgEditorHelper.DrawInlineEditor(trait, EditorCache);
                    }
                }
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            
            
            
            //GUILayout.EndScrollView();
            //GUILayout.EndArea();

            //traits subwindow
            //Rect traitsinspector = new Rect(1000, 240, 560, 720);
            //GUILayout.BeginArea(traitsinspector);
            
            // GUILayout.Label("Traits Area");
            // traits.Scroll = EditorGUILayout.BeginScrollView(traits.Scroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            //
        }
    }
}