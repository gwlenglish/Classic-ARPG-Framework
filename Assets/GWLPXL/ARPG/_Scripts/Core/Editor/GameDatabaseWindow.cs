using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.CanvasUI.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Portals.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Shopping.com;
using GWLPXL.ARPGCore.Statics.com;

using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Abilities.com;

using GWLPXL.ARPGCore.Creation.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using GWLPXL.ARPGCore.Wearables.com;

namespace GWLPXL.ARPGCore.com
{
   
    public class TypeOptions
    {
        public Vector2 Scroll;
        public Vector2 Scroll2;

        public string Type = string.Empty;
        public int Selected = 0;
        public string[] Options = new string[0];
        public string[] Options2 = new string[0];
        

    }
    /// <summary>
    /// this class has too much...
    /// </summary>
    public class GameDatabaseWindow : EditorWindow
    {
        protected string[] creations = new string[9] { "Type Viewer", "Player", "Enemy", "ShopKeeper", "Questgiver", "Breakable", "Searchable", "EquipmentViewer", "Wearables" };
        WearableWindow wearableWindow;
        AbilityEditorWindow abilityWindow;
        protected string[] modifiabletypes = new string[6] { "Stats", "Resources", "Accessories", "Weapons", "Armor Materials", "Equipment Slots" };
        protected string[] movertypes2d = new string[2] { "StateMachine", "Simple Topdown" };
        protected string[] movertypes3d = new string[1] { "NavMeshMouse" };
        protected string[] interactTypes2d = new string[2] { "Mouse Input", "OnTriggerEnter"};
        protected string[] interactTypes3d = new string[1] { "Mouse Input" };
        protected IDatabase source;
        protected UnityEngine.Object focused = null;
        protected int selected = 0;
        protected Vector2 scrollPosition;
        protected Vector2 defaultScrollPos;
        protected Rect rect;
        protected Vector2 scrollPositionActor;
        protected Vector2 scrollPositionOther;
        protected Vector2 scrollPositionactivefocused;
        protected GameDatabase gamedatabase = null;
        protected string[] dbtypes = new string[0];

        int rowsize = 8;
        int miniselected = 0;

        protected Vector2 playerOptionscroll;
        PlayerOptions playerOptions = new PlayerOptions();
        protected Vector2 enemyOptionscroll;
        EnemyOptions enemyOptions = new EnemyOptions();
        protected Vector2 shopkeeperscroll;
        ShopkeeperOptions shopOptions = new ShopkeeperOptions();
        protected Vector2 questkeeperscroll;
        QuestGiverOptions questgiverOptions = new QuestGiverOptions();
        protected Vector2 breakablescroll;
        BreakableOptions breakableoptions = new BreakableOptions();
        protected Vector2 searchablescroll;
        SearchableOptions searchableoptions = new SearchableOptions();

        TypeOptions typeoptions = new TypeOptions();

        string[] attributeoptions = new string[0];
        string[] abilitycontrolleroptions = new string[0];
        string[] auracontrolleroptions = new string[0];
        string[] inventoryoptions = new string[0];
        string[] classoptions = new string[0];
        string[] questlogoptions = new string[0];
        string[] lootdropoptions = new string[0];

        TypeOptions lootoptions = new TypeOptions();
        TypeOptions attributesoptions = new TypeOptions();
        TypeOptions attributescalingoptions = new TypeOptions();
        TypeOptions attributegeneration = new TypeOptions();
        TypeOptions actorclasstypoptions = new TypeOptions();
        TypeOptions abilitycontrollertypeoptions = new TypeOptions();
        TypeOptions auracontrollertypeoptions = new TypeOptions();
        TypeOptions inventorytypeptions = new TypeOptions();


        string[] itemTypes = new string[0];
        string[] resourcetypes = new string[0];
        TypeOptions resourcetype = new TypeOptions();
        string[] stattypes = new string[0];
        TypeOptions stattype = new TypeOptions();
        string[] accessoryTypes = new string[0];
        TypeOptions accessorytype = new TypeOptions();
        string[] weaponTypes = new string[0];
        TypeOptions weapontypes = new TypeOptions();
        string[] armormaterials = new string[0];
        TypeOptions armormaterialtypes = new TypeOptions();
        string[] equipmentslots = new string[0];
        TypeOptions equipmentslottypeoptions = new TypeOptions();
        TypeOptions socketItemtypeOptions = new TypeOptions();
        TypeOptions traits = new TypeOptions();
        int typeselection = 0;

        UnityEditor.Editor eqeditor;
        protected virtual void ReloadMessage()
        {
            EditorUtility.DisplayDialog("Reloaded", "Database reloaded", "Okay");
        }
        /// <summary>
        /// includes empty one at the beginning
        /// </summary>
        /// <param name="forStrings"></param>
        /// <returns></returns>
        protected virtual string[] GetToolbarOptions(string[] forStrings)
        {
            string[] options = forStrings;//
            string[] includeEmpty = new string[options.Length];
            for (int i = 0; i < includeEmpty.Length; i++)
            {
                //if (i == 0) continue;
                includeEmpty[i] = options[i];

            }
            //includeEmpty[0] = "Blank";
            return includeEmpty;
        }

      //  [System.Obsolete]
        protected virtual void OnGUI()
        {
            if (gamedatabase == null) Close();
            DefaultBehavior();
        }

      
        protected virtual void OnValidate()
        {

        }
        private void OnSelectionChange()
        {

        }
        protected void TryCreateNew(string assetname, UnityEngine.Object asset)
        {
            if (string.IsNullOrEmpty(assetname))
            {
                Debug.LogError("Need a name to save");
                return;

            }
            string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "asset", "asset");
            if (folder.Length == 0)
            {
                Debug.Log("Did not find a folder for the assets");
                return;
            }

            AssetDatabase.CreateAsset(asset, folder);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
        protected virtual void TryDelete(Object focused)
        {
            bool confirm = EditorUtility.DisplayDialog("Delete Entry", "This will delete the current entry from the project. Are you sure?", "Yes, Delete Asset", "No, cancel.");
            if (confirm && focused != null)
            {
                ISaveJsonConfig json = (ISaveJsonConfig)focused;
                if (json != null && json.GetTextAsset() != null)
                {
                    bool confirm2 = EditorUtility.DisplayDialog("Delete Config file as well?", "This will delete the current entry's config file from the project. Also delete the config file?", "Yes, delete config.", "No, keep config.");
                    if (confirm2)
                    {
                        string configfile = AssetDatabase.GetAssetPath(json.GetTextAsset());
                        AssetDatabase.DeleteAsset(configfile);
                    }
                }

                string path = AssetDatabase.GetAssetPath(focused);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }
        }
        protected virtual Object GetObject()
        {
            if (GetDatabase() == null) return null;
            focused = GetDatabase().GetDatabaseObjectBySlotIndex(selected);
            return focused;

        }
        public virtual void SetDatabase(Object database)
        {
            this.gamedatabase = database as GameDatabase;
            if (this.gamedatabase != null)
            {
                DatabaseHandler.ReloadDatabase(database);
                source = GetDatabase();

                DatabaseID[] ids = gamedatabase.GetDatabaseTypes();
                dbtypes = new string[ids.Length];
                for (int i = 0; i < dbtypes.Length; i++)
                {
                    dbtypes[i] = ids[i].ToString();
                }

                SetOptionStrings();
            }

        }

        public virtual void ShowWindow()
        {
           // EditorWindow.GetWindow(typeof(GameDatabaseWindow));
            
        }

        protected virtual IDatabase GetDatabase()
        {
            return gamedatabase as IDatabase;
        }




        protected virtual void CloseWindow()
        {
            //close.


            Close();


        }

        protected virtual void SaveAllChanges(ISaveJsonConfig[] jsons)
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
            EditorUtility.DisplayDialog("Saved", "All Changes saved", "Okay");

        }
        protected virtual void OnDestroy()
        {
            if (GetDatabase() != null && Application.isEditor)
            {
                bool confirmSave = EditorUtility.DisplayDialog("Save Changes?", "Do you want to save before closing?", "Yes, save.", "No.");
                if (confirmSave)
                {
                    
                    SaveAllChanges(GetDatabase().GetJsons());

                }
                else
                {
                    //nothing
                }
            }


        }




        protected virtual void DefaultBehavior()
        {

            //toolbar view
            Rect toolbar = EditorGUILayout.BeginVertical("Box");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            selected = GUILayout.SelectionGrid(selected, GetToolbarOptions(dbtypes), rowsize);

            if (GetObject() == gamedatabase)//focused on game database...
            {
                Rect defaultview = new Rect(10, 60, 640, 120);
                GUILayout.BeginArea(defaultview);
                defaultScrollPos = GUILayout.BeginScrollView(defaultScrollPos, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                focused = GetObject();
                if (focused != null)
                {
                    UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(focused);
                    editor.DrawDefaultInspector();
                }
                GUILayout.EndScrollView();
                GUILayout.EndArea();

                //NewLayout();
                Rect actorBar = new Rect(10, 240, 160, 640);
                GUILayout.BeginArea(actorBar);
                bool refresh = GUILayout.Button("Refresh Game Database");
                EditorGUILayout.LabelField("Create Actors: ", EditorStyles.boldLabel);
                miniselected = GUILayout.SelectionGrid(miniselected, creations, 1);
                EditorGUILayout.Space(16);
                EditorGUILayout.LabelField("Project Settings: ", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Layer Settings", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("These layers can only be changed in the PROJECT SETTINGS scriptable object assigned in your game database.", MessageType.Info);
                LayerMask mask = gamedatabase.Settings.LayerAssign.AttackableLayer;
                int layer = (int)Mathf.Log(mask.value, 2);

                string layername = LayerMask.LayerToName(layer);
                EditorGUILayout.LabelField("Attackable Layer:");
                EditorGUILayout.LabelField(layername, EditorStyles.layerMaskField);
                mask = gamedatabase.Settings.LayerAssign.InteractableLayer;
                layer = (int)Mathf.Log(mask.value, 2);
                layername = LayerMask.LayerToName(layer);
                EditorGUILayout.LabelField("Interactable Layer:");
                EditorGUILayout.LabelField(layername, EditorStyles.layerMaskField);

                mask = gamedatabase.Settings.LayerAssign.GroundLayer;
                layer = (int)Mathf.Log(mask.value, 2);
                layername = LayerMask.LayerToName(layer);
                EditorGUILayout.LabelField("Ground Layer:");
                EditorGUILayout.LabelField(layername, EditorStyles.layerMaskField);
                GUILayout.EndArea();

             
                if (refresh)
                {
                    SetDatabase(gamedatabase);
                    return;
                }




                switch (miniselected)
                {
                    case 0:
                        //type viewer
                        #region type viewer
                        Rect typecreation = new Rect(240, 240, 720, 720);
                        GUILayout.BeginArea(typecreation);
                        typeselection = GUILayout.SelectionGrid(typeselection, modifiabletypes, rowsize);
                        Rect statbar = new Rect(24, 48, 640, 420);
                        switch (typeselection)
                        {
                            case 0:
                                //stats
                                stattype = GameDatabaseWindowMaker.TypeCreator(statbar, stattypes, stattype, gamedatabase.Settings);
                                break;
                            case 1:
                                //resources
                                resourcetype = GameDatabaseWindowMaker.TypeCreator(statbar, resourcetypes, resourcetype, gamedatabase.Settings);
                                break;
                            case 2:
                                //accessories
                                accessorytype = GameDatabaseWindowMaker.TypeCreator(statbar, accessoryTypes, accessorytype, gamedatabase.Settings);
                                break;
                            case 3:
                                //weapons
                                weapontypes = GameDatabaseWindowMaker.TypeCreator(statbar, weaponTypes, weapontypes, gamedatabase.Settings);
                                break;
                            case 4:
                                //armor materials
                                armormaterialtypes = GameDatabaseWindowMaker.TypeCreator(statbar, armormaterials, armormaterialtypes, gamedatabase.Settings);
                                break;
                            case 5:
                                //equipment slots
                                // statbar = new Rect(statbar.x, statbar.y, statbar.width / 2, statbar.height / 2);
                                equipmentslottypeoptions = GameDatabaseWindowMaker.TypeCreator(statbar, equipmentslots, equipmentslottypeoptions, gamedatabase.Settings);
                                break;


                        }
                        GUILayout.EndArea();

                        #endregion
                        break;
                    case 1:
                        #region player
                        //player, to do - a create new option that opens the appropriate editor window.
                        Rect actordetails = new Rect(240, 240, 360, 720);
                        GUILayout.BeginArea(actordetails);
                        gamedatabase.Settings.Templates.UnityDefaults.PhysicsType = (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", gamedatabase.Settings.Templates.UnityDefaults.PhysicsType);
                        EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                            "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);
                        playerOptionscroll = GUILayout.BeginScrollView(playerOptionscroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        EditorGUILayout.LabelField(" Attributes ");
                        playerOptions.Attributes = EditorGUILayout.Popup(playerOptions.Attributes, attributeoptions);
                        EditorGUILayout.LabelField(" Ability Controller ");
                        playerOptions.AbilityController = EditorGUILayout.Popup(playerOptions.AbilityController, abilitycontrolleroptions);
                        EditorGUILayout.LabelField(" Aura Controller ");
                        playerOptions.AuraController = EditorGUILayout.Popup(playerOptions.AuraController, auracontrolleroptions);
                        EditorGUILayout.LabelField(" Inventory ");
                        playerOptions.Inventory = EditorGUILayout.Popup(playerOptions.Inventory, inventoryoptions);
                        EditorGUILayout.LabelField(" Class ");
                        playerOptions.Class = EditorGUILayout.Popup(playerOptions.Class, classoptions);
                        EditorGUILayout.LabelField(" Quest Log ");
                        playerOptions.QuestLog = EditorGUILayout.Popup(playerOptions.QuestLog, questlogoptions);


                        playerOptions.UseCombat = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Can Fight? ", playerOptions.UseCombat);//use this to override

                        playerOptions.CanTakeDamage = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Can Take Damage? ", playerOptions.CanTakeDamage);//use this to override

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Can Take Status Effects? ");
                        playerOptions.CanTakeStatusEffects = EditorGUILayout.Toggle(playerOptions.CanTakeStatusEffects);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Can Level Up? ");
                        playerOptions.UseLeveling = EditorGUILayout.Toggle(playerOptions.UseLeveling);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Can Use Auras? ");
                        playerOptions.UseAuras = EditorGUILayout.Toggle(playerOptions.UseAuras);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Can Receive Auras? ");
                        playerOptions.ReceiveAuras = EditorGUILayout.Toggle(playerOptions.ReceiveAuras);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Can Receive Quests? ");
                        playerOptions.UseQuests = EditorGUILayout.Toggle(playerOptions.UseQuests);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Can Buy and Sell Items? ");
                        playerOptions.UseShopping = EditorGUILayout.Toggle(playerOptions.UseShopping);
                        EditorGUILayout.EndHorizontal();


                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Use Built-in Movement? ");
                        playerOptions.UseBuiltInMoving = EditorGUILayout.Toggle(playerOptions.UseBuiltInMoving);
                        EditorGUILayout.EndHorizontal();

                        GameDatabaseWindowMaker.MoveTypeFoldout(playerOptions, movertypes2d, movertypes3d, gamedatabase.Settings);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Use Built-in Interaction Detection? ");
                        playerOptions.UseBuiltInInteraction = EditorGUILayout.Toggle(playerOptions.UseBuiltInInteraction);
                        EditorGUILayout.EndHorizontal();

                        GameDatabaseWindowMaker.InteractTypesFoldout(playerOptions, interactTypes2d, interactTypes3d, gamedatabase.Settings);


                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Use Built-in Canvas UI? ");
                        playerOptions.UseBuiltInCanvases = EditorGUILayout.Toggle(playerOptions.UseBuiltInCanvases);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Use Built-in Animator Controller? ");
                        playerOptions.UseAnimatorController = EditorGUILayout.Toggle(playerOptions.UseAnimatorController);
                        EditorGUILayout.EndHorizontal();

                        if (playerOptions.UseAnimatorController)
                        {
                            EditorGUILayout.LabelField(" Character Mesh? ");
                            playerOptions.CharacterMesh = (GameObject)EditorGUILayout.ObjectField(playerOptions.CharacterMesh, typeof(GameObject), false);
                        }


                        EditorGUILayout.LabelField(" Modify Existing GameObject? ");
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.HelpBox("Modify Existing will add the components to an already created prefab. Use this for things like InvectorLite where you make the controller first, then modify it here.", MessageType.Info);
                        EditorGUILayout.EndVertical();
                        playerOptions.ModifyExisting = EditorGUILayout.Toggle(playerOptions.ModifyExisting);
                        if (playerOptions.ModifyExisting)
                        {
                            EditorGUILayout.LabelField(" Prefab to clone and modify? ");
                            playerOptions.PrefabToModify = (GameObject)EditorGUILayout.ObjectField(playerOptions.PrefabToModify, typeof(GameObject), false);
                           
                        }
         

                        string createormodify = string.Empty;
                        if (playerOptions.ModifyExisting)
                        {
                            createormodify = "Modify";
                        }
                        else
                        {
                            createormodify = "Create";
                        }

                        GUILayout.Space(25);
                        if (GUILayout.Button(createormodify))
                        {
                            string assetname = attributeoptions[playerOptions.Attributes] + "_Prefab";
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
                            UnityEngine.Object returnedAsset = ActorDefinitions.CreateNewPlayerPrefab(path, playerOptions, gamedatabase);
                            Selection.activeObject = returnedAsset;
                            EditorGUIUtility.PingObject(returnedAsset);
                        }


                        GUILayout.EndScrollView();
                        GUILayout.EndArea();

                        Rect subpanel = new Rect(640, 280, 420, 720);
                        GUILayout.BeginArea(subpanel);
                        PlayerActorViewOptions playerview = gamedatabase.Settings.InspectObjects.Player;
                        playerview.ToolBarSelector = GUILayout.Toolbar(playerview.ToolBarSelector, playerview.PlayerViewOptions);
                        GUILayout.EndArea();


                        Rect subpanelrect = new Rect(640, 300, 420, 720);
                        switch (playerview.ToolBarSelector)
                        {
                            case 0:
                                //player attributes
                                
                                GameDatabaseWindowMaker.AttributesDatabaseWindowMaker(subpanelrect, attributesoptions, gamedatabase, playerOptions.Attributes, "Player Attributes");
                                Rect rect = new Rect(subpanel.x + 420, subpanel.y - 64, 420, 720);
                                GameDatabaseWindowMaker.AttributeScalingButton(rect, attributescalingoptions, gamedatabase, playerOptions.Attributes, "Player Scaling");
                                Rect subrect = new Rect(rect.x, rect.y + 60, rect.width + 196, rect.height);
                                GameDatabaseWindowMaker.AttributeScalingGenerationWindow(subrect, attributegeneration, gamedatabase, playerOptions.Attributes, "Generated");
                                break;
                            case 1:
                                //player inventory
                                GameDatabaseWindowMaker.InventoryDatabaseWindowMaker(subpanelrect, inventorytypeptions, gamedatabase, playerOptions.Class, "Player Inventory");
                                break;
                            case 2:
                                //player class
                                GameDatabaseWindowMaker.ClassDatabaseWindowMaker(subpanelrect, actorclasstypoptions, gamedatabase, playerOptions.Class, "Player Class");
                                break;
                            case 3:
                                //player abilities
                                GameDatabaseWindowMaker.AbilityControllerDatabase(subpanelrect, abilitycontrollertypeoptions, gamedatabase, playerOptions.AbilityController, "Player Ability Controller");
                                break;
                            case 4:
                                //player auras
                                GameDatabaseWindowMaker.AuraControllerDatabase(subpanelrect, auracontrollertypeoptions, gamedatabase, playerOptions.AuraController, "Player Aura Controller");
                                break;
                        }
                       // GUILayout.EndArea();
                        ////attributes subwindow


                      
                        #endregion
                        break;
                    case 2:
                        //enemy
                        #region enemy
                        Rect enemydetails = new Rect(240, 240, 360, 720);
                        GUILayout.BeginArea(enemydetails);
                        gamedatabase.Settings.Templates.UnityDefaults.PhysicsType = (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", gamedatabase.Settings.Templates.UnityDefaults.PhysicsType);
                        EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                            "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);
                        enemyOptionscroll = GUILayout.BeginScrollView(enemyOptionscroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            
                        EditorGUILayout.LabelField(" Attributes ");
                        enemyOptions.Attributes = EditorGUILayout.Popup(enemyOptions.Attributes, attributeoptions);
                        EditorGUILayout.LabelField(" Ability Controller ");
                        enemyOptions.AbilityController = EditorGUILayout.Popup(enemyOptions.AbilityController, abilitycontrolleroptions);
                        EditorGUILayout.LabelField(" Aura Controller ");
                        enemyOptions.AuraController = EditorGUILayout.Popup(enemyOptions.AuraController, auracontrolleroptions);
                        EditorGUILayout.LabelField(" Inventory ");
                        enemyOptions.Inventory = EditorGUILayout.Popup(enemyOptions.Inventory, inventoryoptions);
                        EditorGUILayout.LabelField(" Class ");
                        enemyOptions.Class = EditorGUILayout.Popup(enemyOptions.Class, classoptions);
                        EditorGUILayout.LabelField(" Loot Drops ");
                        enemyOptions.LootDrops = EditorGUILayout.Popup(enemyOptions.LootDrops, lootdropoptions);

                        enemyOptions.ScaleWithDungeonLevel = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Scale with Dungeon Level? ", enemyOptions.ScaleWithDungeonLevel);
                        enemyOptions.CanTakeDamage = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Can Take Damage? ", enemyOptions.CanTakeDamage);
                        enemyOptions.UseCombat = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Can Fight? ", enemyOptions.UseCombat);
                        enemyOptions.CanTakeStatusEffects = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Can Take Status Effects? ", enemyOptions.CanTakeStatusEffects);

                        enemyOptions.UseAuras = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Can Use Auras? ", enemyOptions.UseAuras);
                        enemyOptions.ReceiveAuras = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Can Receive Auras? ", enemyOptions.ReceiveAuras);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(" Use Built-in Movement? ");
                        enemyOptions.UseBuiltInMoving = EditorGUILayout.Toggle(enemyOptions.UseBuiltInMoving);
                        EditorGUILayout.EndHorizontal();

                        GameDatabaseWindowMaker.MoveTypeFoldout(enemyOptions, movertypes2d, movertypes3d, gamedatabase.Settings);

                        enemyOptions.UseAnimatorController = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Use Animator Controller? ", enemyOptions.UseAnimatorController);
                        enemyOptions.UseBuiltInHPInfo = GameDatabaseWindowMaker.CreateHorizontalToggleField(" Use HP Bar? ", enemyOptions.UseBuiltInHPInfo);
                        if (enemyOptions.UseAnimatorController)
                        {
                            EditorGUILayout.LabelField(" Character Mesh? ");
                            enemyOptions.CharacterMesh = (GameObject)EditorGUILayout.ObjectField(enemyOptions.CharacterMesh, typeof(GameObject), false);
                        }


                        GUILayout.Space(25);
                        if (GUILayout.Button("Create"))
                        {
                            string assetname = attributeoptions[enemyOptions.Attributes] + "_Prefab";
                            //ask where to make it
                            string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                            if (folder.Length == 0)
                            {
                                Debug.Log("Did not find a folder for the assets");
                                return;
                            }

                            UnityEngine.Object returnedAsset = EnemyDefinition.CreateNewEnemyPrefab(folder, enemyOptions, gamedatabase);
                            Selection.activeObject = returnedAsset;
                            EditorGUIUtility.PingObject(returnedAsset);
                        }


                        GUILayout.EndScrollView();
                        GUILayout.EndArea();



                        //mini selctor
                        Rect enemysubpanel = new Rect(640, 280, 420, 720);
                        GUILayout.BeginArea(enemysubpanel);
                        EnemyActorViewOptions enemyview = gamedatabase.Settings.InspectObjects.Enemy;
                        enemyview.ToolBarSelector = GUILayout.Toolbar(enemyview.ToolBarSelector, enemyview.EnemyViewOptions);
                        GUILayout.EndArea();


                        Rect enemysubsubpanelrect = new Rect(640, 300, 420, 720);
                        switch (enemyview.ToolBarSelector)
                        {
                            case 0:
                                //attributes

                                GameDatabaseWindowMaker.AttributesDatabaseWindowMaker(enemysubsubpanelrect, attributesoptions, gamedatabase, enemyOptions.Attributes, "Enemy Attributes");
                                Rect rect = new Rect(enemysubsubpanelrect.x + 420, enemysubsubpanelrect.y - 64, 420, 720);
                                GameDatabaseWindowMaker.AttributeScalingButton(rect, attributescalingoptions, gamedatabase, enemyOptions.Attributes, "Enemy Scaling");
                                Rect subrect = new Rect(rect.x, rect.y + 60, rect.width + 196, rect.height);
                                GameDatabaseWindowMaker.AttributeScalingGenerationWindow(subrect, attributegeneration, gamedatabase, enemyOptions.Attributes, "Generated");
                                break;
                            case 1:
                                //inventory
                                GameDatabaseWindowMaker.InventoryDatabaseWindowMaker(enemysubsubpanelrect, inventorytypeptions, gamedatabase, enemyOptions.Class, "Enemy Inventory");
                                break;
                            case 2:
                                //class
                                GameDatabaseWindowMaker.ClassDatabaseWindowMaker(enemysubsubpanelrect, actorclasstypoptions, gamedatabase, enemyOptions.Class, "Enemy Class");
                                break;
                            case 3:
                                //abilities
                                GameDatabaseWindowMaker.AbilityControllerDatabase(enemysubsubpanelrect, abilitycontrollertypeoptions, gamedatabase, enemyOptions.AbilityController, "Enemy Ability Controller");
                                break;
                            case 4:
                                //auras
                                GameDatabaseWindowMaker.AuraControllerDatabase(enemysubsubpanelrect, auracontrollertypeoptions, gamedatabase, enemyOptions.AuraController, "Enemy Aura Controller");
                                break;
                            case 5:
                                //lootdrops
                                GameDatabaseWindowMaker.LootDatabaseWindowMaker(enemysubsubpanelrect, lootoptions, gamedatabase, enemyOptions.LootDrops, "Enemy Loot Drops");
                                break;
                        }

                        #endregion

                        break;
                    case 3:
                        //shopkeeper
                        #region shopkeeper
                        Rect shopdetails = new Rect(240, 240, 360, 720);
                        GUILayout.BeginArea(shopdetails);
                        gamedatabase.Settings.Templates.UnityDefaults.PhysicsType = (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", gamedatabase.Settings.Templates.UnityDefaults.PhysicsType);
                        EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                            "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);
                        shopkeeperscroll = GUILayout.BeginScrollView(shopkeeperscroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                        EditorGUILayout.LabelField(" Attributes ");
                        shopOptions.Attributes = EditorGUILayout.Popup(shopOptions.Attributes, attributeoptions);
                        EditorGUILayout.LabelField(" Inventory ");
                        shopOptions.Inventory = EditorGUILayout.Popup(shopOptions.Inventory, inventoryoptions);
                        EditorGUILayout.LabelField(" Store Table ");
                        shopOptions.LootDrops = EditorGUILayout.Popup(shopOptions.LootDrops, lootdropoptions);
                        EditorGUILayout.LabelField(" Use Builtin Canvas? ");
                        shopOptions.UseBuiltInCanvas = EditorGUILayout.Toggle(shopOptions.UseBuiltInCanvas);
                        EditorGUILayout.LabelField(" Shop scales? ");
                        shopOptions.ShopScalesWithDungeonLevel = EditorGUILayout.Toggle(shopOptions.ShopScalesWithDungeonLevel);
                        EditorGUILayout.LabelField(" Shop rolls? ");
                        shopOptions.ItemRolls = EditorGUILayout.IntField(shopOptions.ItemRolls);


       

                        EditorGUILayout.LabelField(" Character Mesh? ");
                        shopOptions.CharacterMesh = (GameObject)EditorGUILayout.ObjectField(shopOptions.CharacterMesh, typeof(GameObject), false);
                        GUILayout.Space(25);
                        if (GUILayout.Button("Create"))
                        {
                            string assetname = attributeoptions[shopOptions.Attributes] + "_Prefab";
                            //ask where to make it
                            string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                            if (folder.Length == 0)
                            {
                                Debug.Log("Did not find a folder for the assets");
                                return;
                            }

                            UnityEngine.Object returnedAsset = ShopKeeperDefinition.CreateNewShopKeeperPrefab(folder, shopOptions, gamedatabase);
                            Selection.activeObject = returnedAsset;
                            EditorGUIUtility.PingObject(returnedAsset);
                        }


                        GUILayout.EndScrollView();
                        GUILayout.EndArea();

                        //break loot ins
                        Rect shoptable = new Rect(640, 280, 520, 720);
                        GameDatabaseWindowMaker.LootDatabaseWindowMaker(shoptable, attributesoptions, gamedatabase, shopOptions.LootDrops, "Shop Table");
                        #endregion
                        break;
                    case 4:
                        //quest giver
                        #region quest giver
                        Rect questdetails = new Rect(240, 240, 360, 720);
                        GUILayout.BeginArea(questdetails);
                        gamedatabase.Settings.Templates.UnityDefaults.PhysicsType = (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", gamedatabase.Settings.Templates.UnityDefaults.PhysicsType);
                        EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                            "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);
                        questkeeperscroll = GUILayout.BeginScrollView(questkeeperscroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                        EditorGUILayout.LabelField(" Attributes ");
                        questgiverOptions.Attributes = EditorGUILayout.Popup(questgiverOptions.Attributes, attributeoptions);

                        EditorGUILayout.LabelField(" Use Builtin Canvas? ");
                        questgiverOptions.UseBuiltInCanvas = EditorGUILayout.Toggle(questgiverOptions.UseBuiltInCanvas);

                        EditorGUILayout.LabelField(" Character Mesh? ");
                        questgiverOptions.CharacterMesh = (GameObject)EditorGUILayout.ObjectField(questgiverOptions.CharacterMesh, typeof(GameObject), false);


                        GUILayout.Space(25);
                        if (GUILayout.Button("Create"))
                        {
                            string assetname = attributeoptions[questgiverOptions.Attributes] + "_Prefab";
                            //ask where to make it
                            string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                            if (folder.Length == 0)
                            {
                                Debug.Log("Did not find a folder for the assets");
                                return;
                            }

                            UnityEngine.Object returnedAsset = QuestGiverDefinition.CreateQuestGiverPrefab(folder, questgiverOptions, gamedatabase);
                            Selection.activeObject = returnedAsset;
                            EditorGUIUtility.PingObject(returnedAsset);
                        }


                        GUILayout.EndScrollView();
                        GUILayout.EndArea();
                        #endregion
                        break;
                    case 5:
                        //breakable
                        #region breakable
                        Rect breakabledetails = new Rect(240, 240, 360, 720);
                        GUILayout.BeginArea(breakabledetails);
                        gamedatabase.Settings.Templates.UnityDefaults.PhysicsType = (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", gamedatabase.Settings.Templates.UnityDefaults.PhysicsType);
                        EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                            "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);
                        breakablescroll = GUILayout.BeginScrollView(breakablescroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                        EditorGUILayout.LabelField(" Attributes ");
                        breakableoptions.Attributes = EditorGUILayout.Popup(breakableoptions.Attributes, attributeoptions);
                        EditorGUILayout.LabelField(" Loot Drop ");
                        breakableoptions.LootDrops = EditorGUILayout.Popup(breakableoptions.LootDrops, lootdropoptions);
                        EditorGUILayout.LabelField(" Health Resource ");
                        breakableoptions.HPResourceType = EditorGUILayout.Popup(breakableoptions.HPResourceType, resourcetypes);
                        EditorGUILayout.LabelField(" Use Scaling? ");
                        breakableoptions.UseScaling = EditorGUILayout.Toggle(breakableoptions.UseScaling);
                        EditorGUILayout.LabelField(" Use Mouse Over Highlight? ");
                        breakableoptions.UseMouseOverOutline = EditorGUILayout.Toggle(breakableoptions.UseMouseOverOutline);
                        EditorGUILayout.LabelField(" Use Custom Gizmos? ");
                        breakableoptions.UseGizmoIcons = EditorGUILayout.Toggle(breakableoptions.UseGizmoIcons);
                        EditorGUILayout.LabelField(" Original Mesh? ");
                        breakableoptions.OriginalMesh = (GameObject)EditorGUILayout.ObjectField(breakableoptions.OriginalMesh, typeof(GameObject), false);
                        EditorGUILayout.LabelField(" Broken Version? ");
                        breakableoptions.BrokenVersion = (GameObject)EditorGUILayout.ObjectField(breakableoptions.BrokenVersion, typeof(GameObject), false);
                        EditorGUILayout.LabelField(" Highlighted Version? ");
                        breakableoptions.HighlightVersion = (GameObject)EditorGUILayout.ObjectField(breakableoptions.HighlightVersion, typeof(GameObject), false);

                        GUILayout.Space(25);
                        if (GUILayout.Button("Create"))
                        {

                            string assetname = attributeoptions[breakableoptions.Attributes] + "_Prefab";
                            //ask where to make it
                            string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                            if (folder.Length == 0)
                            {
                                Debug.Log("Did not find a folder for the assets");
                                return;
                            }

                            UnityEngine.Object returnedAsset = BreakableDefinition.CreateBreakablePrefab(folder, breakableoptions, gamedatabase);
                            Selection.activeObject = returnedAsset;
                            EditorGUIUtility.PingObject(returnedAsset);
                        }


                        GUILayout.EndScrollView();
                        GUILayout.EndArea();



                        //break loot ins
                        Rect breaklootins = new Rect(640, 280, 520, 720);
                        GameDatabaseWindowMaker.LootDatabaseWindowMaker(breaklootins, lootoptions, gamedatabase, breakableoptions.LootDrops, "Drop Table");

                        #endregion

                        break;
                    case 6:
                        //searchable
                        #region searchable
                        Rect searchable = new Rect(240, 240, 360, 720);
                        GUILayout.BeginArea(searchable);
                        gamedatabase.Settings.Templates.UnityDefaults.PhysicsType = (EditorPhysicsType)EditorGUILayout.EnumPopup("Detection Type", gamedatabase.Settings.Templates.UnityDefaults.PhysicsType);
                        EditorGUILayout.HelpBox("3D options will use Unity's 3D physics system for detection (e.g. Rigibody, BoxCollider). " +
                            "2D options will use unity's Physics2D system for detection (e.g. Rigidbody2D, BoxCollider2D).", MessageType.Info);
                        searchablescroll = GUILayout.BeginScrollView(searchablescroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                        EditorGUILayout.LabelField(" Name? ");
                        searchableoptions.Name = EditorGUILayout.TextField(searchableoptions.Name);
                        EditorGUILayout.LabelField(" Loot Drops? ");
                        searchableoptions.LootDrops = EditorGUILayout.Popup(searchableoptions.LootDrops, lootdropoptions);
                        EditorGUILayout.LabelField(" Unscaled level? ");
                        searchableoptions.UnscaledLevel = EditorGUILayout.IntField(searchableoptions.UnscaledLevel);
                        EditorGUILayout.LabelField(" Search distance? ");
                        searchableoptions.SearchDistance = EditorGUILayout.FloatField(searchableoptions.SearchDistance);
                        EditorGUILayout.LabelField(" Use Scaling? ");
                        searchableoptions.UseScaling = EditorGUILayout.Toggle(searchableoptions.UseScaling);
                        EditorGUILayout.LabelField(" Use Mouse Over Highlight? ");
                        searchableoptions.UseMouseOverOutline = EditorGUILayout.Toggle(searchableoptions.UseMouseOverOutline);
                        EditorGUILayout.LabelField(" Use Custom Gizmos? ");
                        searchableoptions.UseGizmoIcons = EditorGUILayout.Toggle(searchableoptions.UseGizmoIcons);
                        EditorGUILayout.LabelField(" Original Mesh? ");
                        searchableoptions.OriginalMesh = (GameObject)EditorGUILayout.ObjectField(searchableoptions.OriginalMesh, typeof(GameObject), false);
                        EditorGUILayout.LabelField(" Highlighted Version? ");
                        searchableoptions.HighlightVersion = (GameObject)EditorGUILayout.ObjectField(searchableoptions.HighlightVersion, typeof(GameObject), false);

                        GUILayout.Space(25);
                        if (GUILayout.Button("Create"))
                        {

                            string assetname = searchableoptions.Name + "_Prefab";
                            //ask where to make it
                            string folder = EditorUtility.SaveFilePanelInProject("Save Asset", assetname, "prefab", "prefab");//path is not valid...
                            if (folder.Length == 0)
                            {
                                Debug.Log("Did not find a folder for the assets");
                                return;
                            }

                            UnityEngine.Object returnedAsset = SearchableDefinition.CreateSearchablePrefab(folder, searchableoptions, gamedatabase);
                            Selection.activeObject = returnedAsset;
                            EditorGUIUtility.PingObject(returnedAsset);
                        }

                        GUILayout.EndScrollView();
                        GUILayout.EndArea();


                        //search loot ins
                        Rect searchlootinspectr = new Rect(640, 280, 520, 720);
                        GameDatabaseWindowMaker.LootDatabaseWindowMaker(searchlootinspectr, lootoptions, gamedatabase, searchableoptions.LootDrops, "Drop Table");

                        #endregion
                        break;
                    case 7:
                        //equipment viewer
                        #region equipment viewer
                        Rect equipment = new Rect(240, 240, 400, 720);
                        GUILayout.BeginArea(equipment);
                        gamedatabase.Settings.InspectObjects.Equipment.ScollableEq = EditorGUILayout.BeginScrollView(gamedatabase.Settings.InspectObjects.Equipment.ScollableEq, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        gamedatabase.Settings.InspectObjects.Equipment.Equipment = (Equipment)EditorGUILayout.ObjectField(gamedatabase.Settings.InspectObjects.Equipment.Equipment, typeof(Equipment), false);
                        string name = "";
                        string json = "";

                        EquipmentViewOptions eqinspectview = gamedatabase.Settings.InspectObjects.Equipment;
                        if (eqinspectview.Equipment != null)
                        {

                            Equipment equipmentfocus = eqinspectview.Equipment;
                            name = equipmentfocus.GetUserDescription();
                            json = equipmentfocus.GetTextAsset().ToString();
                            EditorGUILayout.TextField(name);
                            EditorGUILayout.TextField(json);
                            gamedatabase.Settings.GeneratedTemp.Equipment.ILevel = EditorGUILayout.IntField(gamedatabase.Settings.GeneratedTemp.Equipment.ILevel, EditorStyles.miniTextField);
                            bool click = GUILayout.Button("Generate Random", EditorStyles.toolbarButton);
                            if (click)
                            {
                                GenerateOptions eqgenerateoptions = gamedatabase.Settings.GeneratedTemp;

                                //clean up
                                for (int i = 0; i < gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq.Length; i++)
                                {
                                    DestroyImmediate(eqgenerateoptions.Equipment.CurvedEq[i]);
                                }

                                
                                if (eqgenerateoptions.Equipment.Equipment != null)
                                {
                                    for (int i = 0; i < eqgenerateoptions.Equipment.Equipment.GetStats().GetAllTraits().Length; i++)
                                    {
                                        DestroyImmediate(eqgenerateoptions.Equipment.Equipment.GetStats().GetAllTraits()[i]);
                                    }
                                    DestroyImmediate(eqgenerateoptions.Equipment.Equipment);
                                }

                                traits.Selected = 0;//reset selection
                                //reset generated temp
                               
                                eqgenerateoptions.Equipment.PowerCurves = new List<PowerCurves>();
                                eqgenerateoptions.Equipment.CurvedEq = new Equipment[gamedatabase.Settings.GeneratedTemp.Equipment.MaxILevelCurve];
                                eqgenerateoptions.Equipment.ScollableEq = Vector2.zero;
                                eqgenerateoptions.Equipment.Equipment = null;

                                eqgenerateoptions.Equipment.Equipment = ScriptableObject.Instantiate(equipmentfocus);
                                eqgenerateoptions.Equipment.Equipment.AssignEquipmentTraits(eqgenerateoptions.Equipment.ILevel);

                                //create copy at each level
                              
                                for (int i = 0; i < gamedatabase.Settings.GeneratedTemp.Equipment.CurvedEq.Length; i++)
                                {

                                    eqgenerateoptions.Equipment.CurvedEq[i] = ScriptableObject.Instantiate(gamedatabase.Settings.InspectObjects.Equipment.Equipment);
                                    eqgenerateoptions.Equipment.CurvedEq[i].AssignEquipmentTraits(i);
                            
                                }

                                string genname = EquipmentDescription.GenerateNewNameForItem(eqgenerateoptions.Equipment.Equipment, gamedatabase.AffixReader);
                                eqgenerateoptions.Equipment.Equipment.GetStats().SetGeneratedName(genname);
          
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
                                eqeditor = UnityEditor.Editor.CreateEditor(equipmentfocus);

                                eqeditor.DrawDefaultInspector();
                            }

                            GUILayout.Space(25);
          
                        }



                        GUILayout.EndScrollView();
                        GUILayout.EndArea();

                        //generated subwindow
                        Rect inspectoreq = new Rect(700, 240, 280, 720);
                        GUILayout.BeginArea(inspectoreq);
                        gamedatabase.Settings.GeneratedTemp.Equipment.ScollableEq = EditorGUILayout.BeginScrollView(gamedatabase.Settings.GeneratedTemp.Equipment.ScollableEq, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                        //open editor window or something
                        if (gamedatabase.Settings.GeneratedTemp.Equipment.Equipment != null && gamedatabase.Settings.InspectObjects.Equipment != null)
                        {
                            Equipment generated = gamedatabase.Settings.GeneratedTemp.Equipment.Equipment;
                            EditorGUILayout.LabelField("User Description", EditorStyles.boldLabel);
                            EditorGUILayout.TextArea(generated.GetUserDescription(), EditorStyles.wordWrappedLabel);
                            EditorGUILayout.LabelField("Equipment Name", EditorStyles.boldLabel);
                            EditorGUILayout.TextArea(generated.GetStats().GetGeneratedName(), EditorStyles.wordWrappedLabel);
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
                        GUILayout.EndScrollView();
                        GUILayout.EndArea();

                        //traits subwindow
                        Rect traitsinspector = new Rect(1000, 240, 560, 720);
                        GUILayout.BeginArea(traitsinspector);
                        GUILayout.Label("Traits Area");
                        traits.Scroll = EditorGUILayout.BeginScrollView(traits.Scroll, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                        if (gamedatabase.Settings.GeneratedTemp.Equipment.Equipment != null )
                        {


                            GUILayout.Label("Native Traits", EditorStyles.boldLabel);

                            for (int i = 0; i < gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllNativeTraitNames().Length; i++)
                            {
                                EditorGUILayout.LabelField(gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllNativeTraitNames()[i], EditorStyles.miniLabel);
                            }

                            GUILayout.Label("Random Traits", EditorStyles.boldLabel);
                            for (int i = 0; i < gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllRandomTraitNames().Length; i++)
                            {
                                EditorGUILayout.LabelField(gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllRandomTraitNames()[i], EditorStyles.miniLabel);

                            }


                            traits.Selected = GUILayout.SelectionGrid(traits.Selected, gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetAllTraitNames(), 3);//, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));

                            if (gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetStats().GetAllTraits().Length > 0)
                            {
                                EditorGUILayout.ObjectField(gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetStats().GetAllTraits()[traits.Selected], typeof(EquipmentTrait), false);
                                UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(gamedatabase.Settings.GeneratedTemp.Equipment.Equipment.GetStats().GetAllTraits()[traits.Selected]);
                                editor.DrawDefaultInspector();
                            }

                           


                        }

                        GUILayout.EndScrollView();
                        GUILayout.EndArea();

                        #endregion
                        break;
                    case 8:
                        //wearable creator
                        if (wearableWindow == null)
                        {
                            wearableWindow = EditorWindow.CreateInstance<WearableWindow>() as WearableWindow;
                            wearableWindow.SetGameDatabase(gamedatabase);
                            wearableWindow.Show();
                            miniselected = 0;
                        }
                        else
                        {
                            wearableWindow.Focus();
                            miniselected = 0;
                        }
                  
                  
                     
                        break;
                  
                }
              



            }
            else
            {
                //focused not on game database window
                Rect otherview = new Rect(10, 80, 600, 800);
                GUILayout.BeginArea(otherview);
                scrollPositionOther = GUILayout.BeginScrollView(scrollPositionOther, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                focused = GetObject();
                if (focused != null)
                {
                    UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(focused);
                    editor.DrawDefaultInspector();
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Open Editor Window"))
                {
                    IDatabase database = GetObject() as IDatabase;
                    EditorMethods.OpenDatabaseWindow(database);

                }

                if (GUILayout.Button("Apply Changes"))
                {
                    UnityEngine.Object database = GetObject();
                    IDatabase idatabase = database as IDatabase;
                    if (idatabase != null)
                    {
                        SaveAllChanges(idatabase.GetJsons());
                        DatabaseHandler.ReloadDatabase(GetObject());
                    }
                }

                if (GUILayout.Button("Close Window"))
                {
                    CloseWindow();
                }

                if (GUILayout.Button("Reload Database"))
                {
                    //close.
                    DatabaseHandler.ReloadDatabase(gamedatabase.GetDatabaseObjectBySlotIndex(selected));
                    ReloadMessage();

                }

                GUILayout.EndScrollView();
                GUILayout.EndArea();


                Rect selectedView = new Rect(720, 100, 600, 800);
                GUILayout.BeginArea(selectedView);
                scrollPositionactivefocused = GUILayout.BeginScrollView(scrollPositionactivefocused, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                Object[] activeScriptables = Selection.GetFiltered(typeof(ScriptableObject), SelectionMode.Assets);//doesnt seem to work
                if (activeScriptables.Length > 0)
                {
                    UnityEngine.Object activeselection = activeScriptables[0];
                    if (activeselection != null)
                    {
                        UnityEditor.Editor anothereditor = UnityEditor.Editor.CreateEditor(activeselection);
                        anothereditor.DrawDefaultInspector();
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.EndArea();

            }


            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void SetOptionStrings()
        {
            itemTypes = System.Enum.GetNames(typeof(ItemType));
            stattypes = System.Enum.GetNames(typeof(StatType));
            resourcetypes = System.Enum.GetNames(typeof(ResourceType));
            armormaterials = System.Enum.GetNames(typeof(ArmorMaterial));
            weaponTypes = System.Enum.GetNames(typeof(WeaponType));
            accessoryTypes = System.Enum.GetNames(typeof(AccessoryType));
            equipmentslots = System.Enum.GetNames(typeof(EquipmentSlotsType));
            if (gamedatabase == null) return;
            attributeoptions = gamedatabase.Attributes.GetAllNames();
            abilitycontrolleroptions = gamedatabase.AbilityControllers.GetAllNames();
            auracontrolleroptions = gamedatabase.AuraControllers.GetAllNames();
            inventoryoptions = gamedatabase.Inventories.GetAllNames();
            classoptions = gamedatabase.Classes.GetAllNames();
            questlogoptions = gamedatabase.QuestLog.GetAllNames();
            lootdropoptions = gamedatabase.Loot.GetAllNames();
        }

       

       
      

        protected virtual void ReloadDatabase()
        {
            DatabaseHandler.ReloadDatabase(gamedatabase);
            ReloadMessage();
        }
    }
}