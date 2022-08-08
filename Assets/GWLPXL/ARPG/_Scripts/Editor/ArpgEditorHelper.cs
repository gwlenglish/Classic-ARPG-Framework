using System;
using System.Collections.Generic;
using GWLPXL.ARPG._Scripts.Editor.Data.com;
using GWLPXL.ARPG._Scripts.Editor.ObjectEditors.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GWLPXL.ARPG._Scripts.Editor.com
{
    using Editor = UnityEditor.Editor;
    public static class ArpgEditorHelper
    {
        public static Vector2 SlideRect(Rect rect, MouseCursor cursor = MouseCursor.SlideArrow)
        {
            if (!GUI.enabled)
                return Vector2.zero;
            EditorGUIUtility.AddCursorRect(rect, cursor);
            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            if (GUI.enabled && Event.current.type == UnityEngine.EventType.MouseDown && (Event.current.button == 0 && rect.Contains(Event.current.mousePosition)))
            {
                GUIUtility.hotControl = controlId;
                EditorGUIUtility.SetWantsMouseJumping(1);
                Event.current.Use();
            }
            else if (GUIUtility.hotControl == controlId)
            {
                if (Event.current.type == UnityEngine.EventType.MouseDrag)
                {
                    Event.current.Use();
                    GUI.changed = true;
                    return Event.current.delta;
                }
                if (Event.current.type == UnityEngine.EventType.MouseUp)
                {
                    GUIUtility.hotControl = 0;
                    EditorGUIUtility.SetWantsMouseJumping(0);
                    Event.current.Use();
                }
            }
            return Vector2.zero;
        }
        
        public static void DrawHorizontalSeparator(Rect rect, float alpha = 0.5f)
        {
            Color first = ArpgStyle.BorderLineColorFirst;
            first.a *= alpha;
            Color second = ArpgStyle.BorderLineColorSecond;
            second.a *= alpha;
            Rect rect2 = new Rect(rect.x, rect.y - 1f, rect.width, 1f);
            EditorGUI.DrawRect(rect2, first);
            rect2.y++;
            EditorGUI.DrawRect(rect2, second);
        }
        
        
        public static Object TryCreateNew(string assetname, UnityEngine.Object asset, string folderPath)
        {
            if (string.IsNullOrEmpty(assetname))
            {
                Debug.LogError("Need a name to save");
                return null;
        
            }
        
            string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "asset", "asset", folderPath);
            if (folder.Length == 0)
            {
                Debug.Log("Did not find a folder for the assets");
                return null;
            }
            
            AssetDatabase.CreateAsset(asset, folder);
            Object newasset = AssetDatabase.LoadAssetAtPath(folder, typeof(Object));
            return newasset;
        }

        public static Texture2D LoadTextureFromBase64(string str)
        {
            var imageBytes = Convert.FromBase64String(str);
            Texture2D tex = new Texture2D(30, 30, TextureFormat.ARGB32, false, true);
            tex.LoadImage(imageBytes);
            return tex;
        }

        public static void SaveAllJsonToDisk(ISaveJsonConfig[] jsons)
        {
            for (int i = 0; i < jsons.Length; i++)//brute force saving, meh
            {
                JsconConfig.OverwriteJson(jsons[i]);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            for (int i = 0; i < jsons.Length; i++)
            {
                JsconConfig.LoadJson(jsons[i]);
            } 
            // EditorUtility.DisplayDialog("Saved", "All Changes saved", "Okay");
        }

        public static List<GameDatabase> GetAllGameDatabasesInProject()
        {
            var list = new List<GameDatabase>();
            var key = nameof(GWLPXL.ARPGCore.com.GameDatabase);
            var assets = AssetDatabase.FindAssets("t:" + key);//specific if you want by putting t:armor or t:equipment, etc.
            foreach (var guid in assets)
            {
                var obj = AssetDatabase.GUIDToAssetPath(guid);
                var newItem = AssetDatabase.LoadAssetAtPath(obj, typeof(GameDatabase)) as GameDatabase;
                if (newItem != null && list.Contains(newItem) == false)
                {
                    list.Add(newItem);
                }
            }

            return list;
        }

        // TODO. Maybe use DescriptiveName in database slot or something else
        public static string GetNameOfArpgObject(Object obj)
        {
            if (obj == null) return "";
            switch (obj)
            {
                case Ability ability:
                    return ability.Data?.Name;
                case AbilityController abilityController:
                    return abilityController.Data?.Name;
                case ActorAttributes actorAttributes:
                    return actorAttributes.ActorName;
                case AuraController auraController:
                    return auraController.AuraControllerData?.Name;
                case Aura aura:
                    return aura.AuraData?.AuraName;
                case ActorClass actorClass:
                    return actorClass.GetClassName();
                case ActorDamageData actorDamageData:
                    return actorDamageData.DamageVar?.Name;
                case MeleeData meleeData:
                    return meleeData.MeleeVars?.Name;
                case ProjectileData meleeData:
                    return meleeData.ProjectileVars?.Name;
                case ActorInventory actorInventory:
                    return actorInventory.GetName();
                case Item item:
                    var baseItemName = item.GetBaseItemName();
                    return string.IsNullOrEmpty(baseItemName) ? item.name : baseItemName;
                // todo name
                case LootDrops lootDrops:
                    return lootDrops.name;
                case Quest quest:
                    return quest.GetQuestName();
                case Questchain questchain:
                    return questchain.GetQuestName();
                // todo name
                case QuestLog questLog:
                    return questLog.name;
                // todo name
                case EquipmentTrait equipmentTrait:
                    return equipmentTrait.name;
                default:
                    return obj.name;
            }
        }

        public static void DrawPopupWithLabel(ref int value, string label, string[] options)
        {
            EditorGUILayout.LabelField(label);
            value = EditorGUILayout.Popup(value, options);
        }
        
        public static void DrawHorizontalTextField(ref string value, string label)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            value = EditorGUILayout.TextField(value);
            EditorGUILayout.EndHorizontal();
        }
        
        public static void DrawHorizontalIntField(ref int value, string label)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            value = EditorGUILayout.IntField(value);
            EditorGUILayout.EndHorizontal();
        }
        
        public static void DrawHorizontalFloatField(ref float value, string label)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            value = EditorGUILayout.FloatField(value);
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawHorizontalToggleField(ref bool toggle, string label)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            toggle = EditorGUILayout.Toggle(toggle);
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawHorizontalObjectField(ref GameObject obj, string label)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            obj = (GameObject) EditorGUILayout.ObjectField(obj, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw default unity editor with prevent unity memory leaks
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="editors"></param>
        /// <param name="disabled"></param>
        /// <param name="withIndent"></param>
        public static void DrawInlineEditor(Object obj, Dictionary<Object, Editor> editors, bool disabled = false, bool withIndent = false)
        {
            if (!editors.TryGetValue(obj, out var editor))
            {
                editor = Editor.CreateEditor(obj, typeof(DefaultEditor));
                editors[obj] = editor;
            }
            EditorGUI.BeginDisabledGroup(disabled);
            if (withIndent) EditorGUI.indentLevel++;
            editor.OnInspectorGUI();
            if (withIndent) EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Draw default unity editor in foldout with prevent unity memory leaks
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="editors"></param>
        /// <param name="disabled"></param>
        /// <param name="withIndent"></param>
        public static void DrawInlineEditorInFoldout(Object obj, ref bool expanded, string name, Dictionary<Object, Editor> editors, bool disabled = false, bool withIndent = true)
        {
            expanded = EditorGUILayout.BeginFoldoutHeaderGroup(expanded, name);
            if (expanded)
            {
                DrawInlineEditor(obj, editors, disabled, withIndent);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        public static void DrawScalingAttributesInFoldout(ref bool expanded, string label, GameDatabase gamedatabase, ActorAttributes attributes)
        {
            expanded = EditorGUILayout.BeginFoldoutHeaderGroup(expanded, label);
            if (expanded)
            {
                //scaling slider
                var viewoptions = gamedatabase.Settings.InspectObjects.Player;
                viewoptions.Attributes = attributes;

                if (viewoptions != null && viewoptions.Attributes != null)
                {
                    var generatedTempAttributes = gamedatabase.Settings.GeneratedTemp.Attributes;
                    generatedTempAttributes.Attributes = ScriptableObject.Instantiate(viewoptions.Attributes);
                    gamedatabase.Settings.GeneratedTemp.Attributes.Level = EditorGUILayout.IntSlider(gamedatabase.Settings.GeneratedTemp.Attributes.Level, gamedatabase.Settings.GeneratedTemp.Attributes.MinILevelCurve, gamedatabase.Settings.InspectObjects.Player.Attributes.MaxLevel - 1);
                    generatedTempAttributes.Attributes.LevelUp(generatedTempAttributes.Level);
                }
                
                var generate = gamedatabase.Settings.GeneratedTemp;

                if (generate.Attributes.Attributes != null)
                {
                    EditorGUILayout.LabelField("Generated", EditorStyles.boldLabel);

                    if (generate.Attributes.Attributes != null)
                    {
                        generate.Attributes.Attributes.LevelUp(generate.Attributes.Level);

                        foreach (AttributeType type in Enum.GetValues(typeof(AttributeType)))
                        {
                            GWLPXL.ARPGCore.Attributes.com.Attribute[] butes = generate.Attributes.Attributes.GetAttributes(type);
                            for (int i = 0; i < butes.Length; i++)
                            {
                                EditorGUILayout.LabelField(butes[i].GetDescriptiveName(), EditorStyles.miniLabel);
                                EditorGUILayout.CurveField(butes[i].LevelCurve);
                                Vector3Int minmax = new Vector3Int(butes[i].Level1Value, butes[i].NowValue, butes[i].Level99Max);

                                GUIStyle style = new GUIStyle();
                                style = EditorStyles.miniLabel;
                                style.stretchWidth = false;
                                style.padding = new RectOffset();

                                GUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField("Level 0 Value: ", style);
                                EditorGUILayout.LabelField("Current Level " + generate.Attributes.Level.ToString() + " Value: ");// + butes[i].NowValue, style);
                                EditorGUILayout.LabelField("Max Level " + generate.Attributes.Attributes.MaxLevel.ToString() + " Value: ");// + butes[i].Level99Max, style);
                                GUILayout.EndHorizontal();

                                style = new GUIStyle();
                                style = EditorStyles.boldLabel;
                                int fontize = style.fontSize;
                                FontStyle original = new FontStyle();
                                original = style.fontStyle;
                                style.fontSize = fontize;
                                GUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(butes[i].Level1Value.ToString(), style);
                                style.fontSize = 16;
                                EditorGUILayout.LabelField(butes[i].NowValue.ToString(), style);
                                style.fontSize = fontize;
                                EditorGUILayout.LabelField(butes[i].Level99Max.ToString(), style);
                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}