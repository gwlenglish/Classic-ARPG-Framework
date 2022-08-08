using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Types.com;
using System;
using GWLPXL.ARPGCore.Creation.com;

public static class GameDatabaseWindowMaker 
{
    static UnityEditor.Editor AbilityControllerDatabaseWindowMakerEditor;
    public static TypeOptions AbilityControllerDatabase(Rect rect, TypeOptions typeoption, GameDatabase gamedatabase, int index, string header)
    {
        GUILayout.BeginArea(rect);
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        typeoption.Scroll = EditorGUILayout.BeginScrollView(typeoption.Scroll, false, false, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        AbilityControllerDatabase database = gamedatabase.AbilityControllers;
        UnityEngine.Object lootdbobj = database.GetDatabaseObjectBySlotIndex(index);

        EditorGUILayout.ObjectField(lootdbobj, typeof(AbilityControllerDatabase), false);
        if (lootdbobj != null)
        {
            AbilityControllerDatabaseWindowMakerEditor = UnityEditor.Editor.CreateEditor(lootdbobj);
            AbilityControllerDatabaseWindowMakerEditor.OnInspectorGUI();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        return typeoption;
    }

    static UnityEditor.Editor AuraControllerDatabaseWindowMakerEditor;
    public static TypeOptions AuraControllerDatabase(Rect rect, TypeOptions typeoption, GameDatabase gamedatabase, int index, string header)
    {
        GUILayout.BeginArea(rect);
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        typeoption.Scroll = EditorGUILayout.BeginScrollView(typeoption.Scroll, false, false, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        AuraControllerDatabase database = gamedatabase.AuraControllers;
        UnityEngine.Object lootdbobj = database.GetDatabaseObjectBySlotIndex(index);

        EditorGUILayout.ObjectField(lootdbobj, typeof(AuraControllersDatabase), false);
        if (lootdbobj != null)
        {
            AuraControllerDatabaseWindowMakerEditor = UnityEditor.Editor.CreateEditor(lootdbobj);
            AuraControllerDatabaseWindowMakerEditor.OnInspectorGUI();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        return typeoption;
    }

    static UnityEditor.Editor InventoryDatabaseWindowMakerEditor;
    public static TypeOptions InventoryDatabaseWindowMaker(Rect rect, TypeOptions typeoption, GameDatabase gamedatabase, int index, string header)
    {
        GUILayout.BeginArea(rect);
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        typeoption.Scroll = EditorGUILayout.BeginScrollView(typeoption.Scroll, false, false, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        InventoryDatabase database = gamedatabase.Inventories;
        UnityEngine.Object lootdbobj = database.GetDatabaseObjectBySlotIndex(index);

        EditorGUILayout.ObjectField(lootdbobj, typeof(InventoryDatabase), false);
        if (lootdbobj != null)
        {
            InventoryDatabaseWindowMakerEditor = UnityEditor.Editor.CreateEditor(lootdbobj);
            InventoryDatabaseWindowMakerEditor.OnInspectorGUI();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        return typeoption;
    }
    public static void MoveTypeFoldout(EnemyOptions options, string[] types2d, string[] types3d, ProjectSettings settings)
    {
        EditorGUILayout.BeginVertical();
        if (options.UseBuiltInMoving)
        {
            EditorGUI.indentLevel++;
            switch (settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:

                    EditorGUILayout.LabelField(" Move Type ");
                    options.Mover3DType = EditorGUILayout.Popup(options.Mover3DType, types3d);
                    break;
                case EditorPhysicsType.Unity2D:
                    EditorGUILayout.LabelField(" Move Type ");
                    options.Mover2DType = EditorGUILayout.Popup(options.Mover2DType, types2d);

                    break;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
    }

    public static void InteractTypesFoldout(PlayerOptions options, string[] types2d, string[] types3d, ProjectSettings settings)
    {
        EditorGUILayout.BeginVertical();
        if (options.UseBuiltInInteraction)
        {
            EditorGUI.indentLevel++;
            switch (settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:

                    EditorGUILayout.LabelField(" Interact Type ");
                    options.Interact3DType = EditorGUILayout.Popup(options.Interact3DType, types3d);
                    break;
                case EditorPhysicsType.Unity2D:
                    EditorGUILayout.LabelField(" Interact Type ");
                    options.Interact2DType = EditorGUILayout.Popup(options.Interact2DType, types2d);

                    break;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
    }
    public static void MoveTypeFoldout(PlayerOptions options,  string[] types2d, string[] types3d, ProjectSettings settings)
    {
        EditorGUILayout.BeginVertical();
        if (options.UseBuiltInMoving)
        {
            EditorGUI.indentLevel++;
            switch (settings.Templates.UnityDefaults.PhysicsType)
            {
                case EditorPhysicsType.Unity3D:

                    EditorGUILayout.LabelField(" Move Type ");
                    options.Mover3DType = EditorGUILayout.Popup(options.Mover3DType, types3d);
                    break;
                case EditorPhysicsType.Unity2D:
                    EditorGUILayout.LabelField(" Move Type ");
                    options.Mover2DType = EditorGUILayout.Popup(options.Mover2DType, types2d);

                    break;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
    }

    public static bool CreateHorizontalToggleField(string label, bool toggle)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label);
        toggle = EditorGUILayout.Toggle(toggle);
        EditorGUILayout.EndHorizontal();
        return toggle;
    }

    static UnityEditor.Editor ClassDatabaseWindowMakerEditor;
    public static TypeOptions ClassDatabaseWindowMaker(Rect rect, TypeOptions typeoption, GameDatabase gamedatabase, int index, string header)
    {
        GUILayout.BeginArea(rect);
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        typeoption.Scroll = EditorGUILayout.BeginScrollView(typeoption.Scroll, false, false, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        ActorClassDatabase database = gamedatabase.Classes;
        UnityEngine.Object lootdbobj = database.GetDatabaseObjectBySlotIndex(index);

        EditorGUILayout.ObjectField(lootdbobj, typeof(ActorClassDatabase), false);
        if (lootdbobj != null)
        {
            ClassDatabaseWindowMakerEditor = UnityEditor.Editor.CreateEditor(lootdbobj);
            ClassDatabaseWindowMakerEditor.OnInspectorGUI();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        return typeoption;
    }

    /// <summary>
    /// redo, just copy over the things already on the SO...
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="typeoption"></param>
    /// <param name="gamedatabase"></param>
    /// <param name="index"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static TypeOptions AttributeScalingGenerationWindow(Rect rect, TypeOptions typeoption, GameDatabase gamedatabase, int index, string header)
    {
        GenerateOptions generate = gamedatabase.Settings.GeneratedTemp;

        GUILayout.BeginArea(rect);
        typeoption.Scroll = EditorGUILayout.BeginScrollView(typeoption.Scroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        if (generate.Attributes.Attributes != null)
        {
            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);

            if (generate.Attributes.Attributes != null)
            {
                generate.Attributes.Attributes.LevelUp(generate.Attributes.Level);

                foreach (AttributeType type in System.Enum.GetValues(typeof(AttributeType)))
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


        GUILayout.EndScrollView();
        GUILayout.EndArea();


        return typeoption;
    }

    public static TypeOptions AttributeScalingButton(Rect rect, TypeOptions typeoption, GameDatabase gamedatabase, int index, string header)
    {
        GUILayout.BeginArea(rect);
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        typeoption.Scroll = EditorGUILayout.BeginScrollView(typeoption.Scroll, false, false, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        PlayerActorViewOptions viewoptions = gamedatabase.Settings.InspectObjects.Player;

        ActorAttributesDatabase database = gamedatabase.Attributes;
        UnityEngine.Object unityobj = database.GetDatabaseObjectBySlotIndex(index);
      
        viewoptions.Attributes = unityobj as ActorAttributes;


        if (gamedatabase.Settings.InspectObjects.Player.Attributes != null)
        {
            EditorGUILayout.IntField(gamedatabase.Settings.InspectObjects.Player.Attributes.MaxLevel - 1);
        }

        if (viewoptions != null && viewoptions.Attributes != null)
        {
            AttribtueGenerateOptions generate = gamedatabase.Settings.GeneratedTemp.Attributes;
            if (generate.Attributes != null)
            {
                UnityEngine.ScriptableObject.DestroyImmediate(generate.Attributes);
            }
            generate.Attributes = ScriptableObject.Instantiate(viewoptions.Attributes);
            gamedatabase.Settings.GeneratedTemp.Attributes.Level = EditorGUILayout.IntSlider(gamedatabase.Settings.GeneratedTemp.Attributes.Level, gamedatabase.Settings.GeneratedTemp.Attributes.MinILevelCurve, gamedatabase.Settings.InspectObjects.Player.Attributes.MaxLevel - 1);

            generate.Attributes.LevelUp(generate.Level);
           
        }
      

        GUILayout.EndScrollView();
        GUILayout.EndArea();


        return typeoption;
    }

    static UnityEditor.Editor AttributesDatabaseWindowMakerEditor;
    public static TypeOptions AttributesDatabaseWindowMaker(Rect rect, TypeOptions typeoption, GameDatabase gamedatabase, int index, string header)
    {
        GUILayout.BeginArea(rect);
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        typeoption.Scroll = EditorGUILayout.BeginScrollView(typeoption.Scroll, false, false, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        ActorAttributesDatabase database = gamedatabase.Attributes;
        UnityEngine.Object lootdbobj = database.GetDatabaseObjectBySlotIndex(index);

        EditorGUILayout.ObjectField(lootdbobj, typeof(ActorAttributes), false);
        if (lootdbobj != null)
        {

            AttributesDatabaseWindowMakerEditor = UnityEditor.Editor.CreateEditor(lootdbobj);
            AttributesDatabaseWindowMakerEditor.OnInspectorGUI();
        }

        
      
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        return typeoption;
    }
    static UnityEditor.Editor LootDatabaseWindowMakerEditor;
    public static TypeOptions LootDatabaseWindowMaker(Rect rect, TypeOptions typeoption, GameDatabase gamedatabase, int index, string header)
    {
        GUILayout.BeginArea(rect);
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        typeoption.Scroll = EditorGUILayout.BeginScrollView(typeoption.Scroll, false, false, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));


        LootDropsDatabase lootdatabase = gamedatabase.Loot;
        UnityEngine.Object lootdbobj = lootdatabase.GetDatabaseObjectBySlotIndex(index);
        EditorGUILayout.ObjectField(lootdbobj, typeof(LootDrops), false);
        if (lootdbobj != null)
        {
            LootDatabaseWindowMakerEditor = UnityEditor.Editor.CreateEditor(lootdbobj);
            LootDatabaseWindowMakerEditor.OnInspectorGUI();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        return typeoption;
    }

    public static TypeOptions TypeCreator(Rect rect, string[] types, TypeOptions typeoption, ProjectSettings settings)
    {
        GUILayout.BeginArea(rect);
        typeoption.Scroll = GUILayout.BeginScrollView(typeoption.Scroll, true, true, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
        typeoption.Selected = GUILayout.SelectionGrid(typeoption.Selected, types, 4);

        EditorGUILayout.LabelField("Resource Types", EditorStyles.boldLabel);
        GUILayout.Space(8);

        EditorGUILayout.HelpBox("These types can only be changed in its respective script. See below to change them.", MessageType.Info);
        GUILayout.BeginHorizontal();
        UnityEngine.Object pingobj = (PingObject)EditorGUILayout.ObjectField(settings.PingObjects.TypesPing, typeof(PingObject), false);

        if (typeoption.Selected != 0)
        {
            EditorUtility.DisplayDialog("Can't Modify Here", "To add or remove types, click on the TypePing object " +
                "and modify the appropriate script.For instance, Resource Types for changing resources.", "Fine");
            EditorGUIUtility.PingObject(pingobj);
            typeoption.Selected = 0;
        }



        GUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("To add or remove types, click on the TypePing object and modify the appropriate script. For instance, Resource Types for changing resources.", MessageType.Info);

        GUILayout.EndScrollView();
        GUILayout.EndArea();
        return typeoption;
    }
}
