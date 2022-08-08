using System;
using System.Linq;
using GWLPXL.ARPG._Scripts.Editor.ArpgTree.com;
using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.com;
using GWLPXL.ARPG._Scripts.Editor.EditorModels.GameDatabase.com;
using GWLPXL.ARPG._Scripts.Editor.ReloadProcessors.com;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Traits.com;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GWLPXL.ARPG._Scripts.Editor.Data.com
{
    /// <summary>
    /// Data to trees
    /// </summary>
    public static class ArpgTreeData
    {
        public static void Setup(ArpgEditorWindow arpgEditorWindow, ArpgTreeView arpgTreeView, GameDatabase GameDatabase)
        {
            // game database
            {
                var gameDatabase = arpgTreeView.Add("Game Database",
                    new ArpgItemDataContainer(GameDatabase));

                var createPlayer = ScriptableObject.CreateInstance<CreatePlayerEditorModel>();
                createPlayer.Setup(GameDatabase);
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(createPlayer),
                    "Player Creator");
                
                var createEnemy = ScriptableObject.CreateInstance<CreateEnemyEditorModel>();
                createEnemy.Setup(GameDatabase);
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(createEnemy),
                    "Enemy Creator");

                var createShopkeeper = ScriptableObject.CreateInstance<CreateShopkeeperEditorModel>();
                createShopkeeper.Setup(GameDatabase);
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(createShopkeeper),
                    "Shopkeeper Creator");

                var createQuestgiver = ScriptableObject.CreateInstance<CreateQuestgiverEditorModel>();
                createQuestgiver.Setup(GameDatabase);
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(createQuestgiver),
                    "Questgiver Creator");
                
                var createBreakable = ScriptableObject.CreateInstance<CreateBreakableEditorModel>();
                createBreakable.Setup(GameDatabase);
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(createBreakable),
                    "Breakable Creator");
                
                var createSearchable = ScriptableObject.CreateInstance<CreateSearchableEditorModel>();
                createSearchable.Setup(GameDatabase);
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(createSearchable),
                    "Searchable Creator");
                
                var createWearable = ScriptableObject.CreateInstance<CreateWearableEditorModel>();
                createWearable.Setup(GameDatabase);
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(createWearable),
                    "Wearable Creator");

                var eqViewer = ScriptableObject.CreateInstance<EquipmentViewerEditorModel>();
                eqViewer.Setup(GameDatabase);
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(eqViewer),
                    "Equipment Viewer");

                var typeViewer = ScriptableObject.CreateInstance<TypeViewerEditorModel>();
                typeViewer.PingObject = GameDatabase.Settings.PingObjects.TypesPing;
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(typeViewer),
                    "Type Viewer");
                
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(GameDatabase.Settings),
                    "Project Settings");
                
                var model = ScriptableObject.CreateInstance<CreateGameDatabase>();
                arpgTreeView.AddToItemAsChild(gameDatabase, 
                    new ArpgItemDataContainer(model, null,
                        new ReloadGameDatabaseList(arpgEditorWindow, model), YMObjectEditorType.CreateNew, typeof(GameDatabase)),
                    "Create new database");
            }

            DefaultSetupItemPattern(arpgTreeView, "Abilities", "NEW ABILITY",
                GameDatabase.Abilities, ScriptableObject.CreateInstance<Ability>(),
                typeof(Ability));
            
            DefaultSetupItemPattern(arpgTreeView, "Ability Controllers", "NEW ABILITY CONTROLLER",
                GameDatabase.AbilityControllers, ScriptableObject.CreateInstance<AbilityController>(),
                typeof(AbilityController));
            
            DefaultSetupItemPattern(arpgTreeView, "Actor Attributes", "NEW ACTOR ATTRIBUTES",
                GameDatabase.Attributes, ScriptableObject.CreateInstance<ActorAttributes>(),
                typeof(ActorAttributes));
            
            DefaultSetupItemPattern(arpgTreeView, "Aura Controllers", "NEW AURA CONTROLLER",
                GameDatabase.AuraControllers, ScriptableObject.CreateInstance<AuraController>(),
                typeof(AuraController));
            
            DefaultSetupItemPattern(arpgTreeView, "Aura", "NEW AURA",
                GameDatabase.Auras, ScriptableObject.CreateInstance<Aura>(),
                typeof(Aura));
            
            DefaultSetupItemPattern(arpgTreeView, "Classes", "NEW CLASS",
                GameDatabase.Classes, ScriptableObject.CreateInstance<ActorClass>(),
                typeof(ActorClass));
            
            DefaultSetupItemPattern(arpgTreeView, "Damage Types", "NEW DAMAGE TYPE",
                GameDatabase.ActorDamageTypes, ScriptableObject.CreateInstance<ActorDamageData>(),
                typeof(ActorDamageData));
            
            DefaultSetupItemPattern(arpgTreeView, "Melee Data", "NEW MELEE DATA",
                GameDatabase.Melee, ScriptableObject.CreateInstance<MeleeData>(),
                typeof(MeleeData));
            
            DefaultSetupItemPattern(arpgTreeView, "Projectile Data", "NEW PROJECTILE DATA",
                GameDatabase.Projectiles, ScriptableObject.CreateInstance<ProjectileData>(),
                typeof(ProjectileData));
            
            DefaultSetupItemPattern(arpgTreeView, "Inventories", "NEW INVENTORY",
                GameDatabase.Inventories, ScriptableObject.CreateInstance<ActorInventory>(),
                typeof(ActorInventory));
            
            DefaultSetupItemPattern(arpgTreeView, "Items", "NEW ITEM",
                GameDatabase.Items, ScriptableObject.CreateInstance<CreateItem>(),
                typeof(Item));
            
            DefaultSetupItemPattern(arpgTreeView, "Loot", "NEW LOOT",
                GameDatabase.Loot, ScriptableObject.CreateInstance<LootDrops>(),
                typeof(LootDrops));
            
            DefaultSetupItemPattern(arpgTreeView, "Quests", "NEW QUEST",
                GameDatabase.Quests, ScriptableObject.CreateInstance<Quest>(),
                typeof(Quest));
            
            DefaultSetupItemPattern(arpgTreeView, "Quest Chains", "NEW QUEST CHAIN",
                GameDatabase.Questchains, ScriptableObject.CreateInstance<Questchain>(),
                typeof(Questchain));
            
            DefaultSetupItemPattern(arpgTreeView, "Quest Logs", "NEW QUEST LOG",
                GameDatabase.QuestLog, ScriptableObject.CreateInstance<QuestLog>(),
                typeof(QuestLog));
            
            DefaultSetupItemPattern(arpgTreeView, "Traits", "NEW TRAIT",
                GameDatabase.Traits, ScriptableObject.CreateInstance<CreateTrait>(),
                typeof(EquipmentTrait));
            
            
            // environment data?
            // factions?
            // status changes?
        }
        
        public static Func<object, Object> DefaultDragAndDropFunc =
            (obj) => ((ArpgItemDataContainer) obj).Object;

        public static Action<ArpgTreeItem> DefaultOnRightClickFunc = item =>
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, Delete);
            menu.AddItem(new GUIContent("Delete with json config"), false, DeleteWithJson);
            menu.ShowAsContext();

            void Delete()
            {
                var arpgContainer = (ArpgItemDataContainer) item.DataContainer;
                if (EditorUtility.DisplayDialog("Delete?",
                    $"Do you want to delete object {ArpgEditorHelper.GetNameOfArpgObject(arpgContainer.Object)}?",
                    "Yes", "No."))
                {
                    var reload = arpgContainer.ReloadTrigger;
                    var path = AssetDatabase.GetAssetPath(arpgContainer.Object);
                    AssetDatabase.DeleteAsset(path);
                    reload?.Reload();
                }
            }

            void DeleteWithJson()
            {
                var arpgContainer = (ArpgItemDataContainer) item.DataContainer;
                if (EditorUtility.DisplayDialog("Delete?",
                    $"Do you want to delete object {ArpgEditorHelper.GetNameOfArpgObject(arpgContainer.Object)} with json config?",
                    "Yes", "No."))
                {
                    var reload = arpgContainer.ReloadTrigger;
                    if (arpgContainer.Object is ISaveJsonConfig config)
                    {
                        var configPath = AssetDatabase.GetAssetPath(config.GetTextAsset());
                        AssetDatabase.DeleteAsset(configPath);
                    }

                    var path = AssetDatabase.GetAssetPath(arpgContainer.Object);
                    AssetDatabase.DeleteAsset(path);
                    reload.Reload();
                }
            }
        };
        
        private static void DefaultSetupItemPattern(ArpgTreeView arpgTreeView, string rootNodeLabel, string newItemLabel,
            IDatabase database, Object createModel, Type type)
        {
            var rootTree = arpgTreeView.Add(rootNodeLabel,
                new ArpgItemDataContainer(database.GetMyObject()));

            var reloadStrategy = new ReloadDatabaseAndTree {Root = arpgTreeView, Tree = rootTree, Database = database};

            arpgTreeView.AddToItemAsChild(rootTree, 
                new ArpgItemDataContainer(createModel,
                    database.GetSearchFolders()[0],
                    reloadStrategy,
                    YMObjectEditorType.CreateNew,
                    type),
                newItemLabel);
            
            arpgTreeView.AddToItemAsChildArray(rootTree,
                Enumerable.Range(0, database.GetAllNames().Length)
                    .Select(database.GetDatabaseObjectBySlotIndex)
                    .Where(w => w != null)
                    .Select(s => new ArpgItemDataContainer(s, reload: reloadStrategy)).ToArray(),
                obj => ArpgEditorHelper.GetNameOfArpgObject(obj.Object),
                DefaultDragAndDropFunc, DefaultOnRightClickFunc);
        }
        
    }
}