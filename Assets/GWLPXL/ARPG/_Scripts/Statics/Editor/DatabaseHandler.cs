#if UNITY_EDITOR
using System;
using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Attributes.com;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Looting.com;
using System.Text;
using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Combat.com;
using System.IO;
using System.Linq;
using Attribute = GWLPXL.ARPGCore.Attributes.com.Attribute;
using Object = UnityEngine.Object;

/// <summary>
/// I dislike the repetition of code, but works for now. Maybe v2 will change. 
/// 
/// </summary>
namespace GWLPXL.ARPGCore.Statics.com
{
    #region reload classes

    public static class CommonReload
    {
        public class CommonReloadData
        {
            public Type Type;
            public string SearchType;
            public IDatabase Database;
            public Func<Object, int> GetId;
            public Action<Object, int> SetId;
            public Action<List<Object>> SetSlots;
        }
        public static void AddAllToList(CommonReloadData data)
        {
            var temp = FindAttributes(data);

            if (temp.Count != 0)
            {
                //resolve any conflicts
                List<Object> conflics = RemoveConflicts(data, temp);

                //re-sort without conflicts
                temp.Sort((x, y) => data.GetId(x).CompareTo(data.GetId(y)));

                var max = 1;
                if (temp.Count > 0) 
                    max = temp.Max(a => data.GetId(a)) + 1;

                foreach (var conflict in conflics)
                {
                    data.SetId(conflict, max++);
                    temp.Add(conflict);
                }

                foreach (var obj in temp)
                {
                    if (obj is ISaveJsonConfig jsonSave)
                        SaveToJson(jsonSave);
                }
            }

            //assign to db
            data.SetSlots(temp);

            //save
            EditorUtility.SetDirty(data.Database.GetMyObject());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        static List<Object> RemoveConflicts(CommonReloadData data, List<Object> temp)
        {
            List<Object> conflics = new List<Object>();
            List<int> used = new List<int>();
            for (int i = 0; i < temp.Count; i++)
            {
                if (used.Contains(data.GetId(temp[i])) || data.GetId(temp[i]) == 0)
                {
                    //conflict
                    conflics.Add(temp[i]);
                }
                else
                {
                    //not conflict
                    used.Add(data.GetId(temp[i]));
                }
            }

            //remove conflicts from sort
            for (int i = 0; i < conflics.Count; i++)
            {
                temp.Remove(conflics[i]);
            }

            return conflics;
        }
        static List<Object> FindAttributes(CommonReloadData data)
        {
            var temp = new List<Object>();
            string key = data.SearchType;
            string[] folders = data.Database.GetSearchFolders();
            if (folders.Length > 0)
            {
                for (int i = 0; i < folders.Length; i++)
                {
                    if (string.IsNullOrEmpty(folders[i]))
                    {
                        Debug.LogWarning("search folder on " + data.Type.Name + " is empty, defaulting to assets");
                        folders[i] = "Assets";//if thing is null...
                    }
                }
            }
     
           
            string[] percents = UnityEditor.AssetDatabase.FindAssets("t:" + key, folders);//specific if you want by putting t:armor or t:equipment, etc.
            foreach (var guid in percents)
            {
                string obj = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var newItem = UnityEditor.AssetDatabase.LoadAssetAtPath(obj, data.Type);
                if (newItem != null)
                {
                    temp.Add(newItem);
                }
            }
            return temp;
        }
        static void SaveToJson(ISaveJsonConfig jsonSave)
        {
            if (jsonSave.GetTextAsset() != null)
            {
                JsconConfig.OverwriteJson(jsonSave);
            }
            else
            {
                JsconConfig.SaveJson(jsonSave);
            }
        }
    }
    public static class MeleeDB
    {
        public static void AddAllToList(MeleeDataDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(MeleeData),
                SearchType = nameof(MeleeData),
                Database = saveSystem,
                GetId = obj => ((MeleeData)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((MeleeData)obj);
                    var newId = new MeleeDataID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new MeleeDataDatabaseSlot(((MeleeData)s).GetID(), (MeleeData)s)).ToArray())
            });
        }
    }
    public static class ProjecileDB
    {
        public static void AddAllToList(ProjectileDataDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(ProjectileData),
                SearchType = nameof(ProjectileData),
                Database = saveSystem,
                GetId = obj => ((ProjectileData)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((ProjectileData)obj);
                    var newId = new ProjectileDataID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new ProjectileDataDatabaseSlot(((ProjectileData)s).GetID(), (ProjectileData)s)).ToArray())
            });
        }
    }
    public static class ActorDamageDB
    {
        public static void AddAllToList(ActorDamageDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(ActorDamageData),
                SearchType = nameof(ActorDamageData),
                Database = saveSystem,
                GetId = obj => ((ActorDamageData)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((ActorDamageData)obj);
                    var newId = new ActorDamageID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new ActorDamageDatabaseSlot(((ActorDamageData)s).GetID(), (ActorDamageData)s)).ToArray())
            });
        }
    }
    public static class QuestChainDB
    {
        public static void AddAllToList(QuestchainDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(Questchain),
                SearchType = nameof(Questchain),
                Database = saveSystem,
                GetId = obj => ((Questchain)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((Questchain)obj);
                    var newId = new QuestchainID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new QuestchainDdatabaseSlot(((Questchain)s).GetID(), (Questchain)s)).ToArray())
            });
        }
    }
    /// <summary>
    /// trait is abstract, contains magic string
    /// </summary>
    public static class TraitDB
    {
        public static void AddAllToList(EquipmentTraitDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(EquipmentTrait),
                SearchType = nameof(EquipmentTrait),
                Database = saveSystem,
                GetId = obj => ((EquipmentTrait)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((EquipmentTrait)obj);
                    var newId = new EquipmentTraitID(data.name, id, data);
                    data.SetTraitID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new TraitDatabaseSlot(((EquipmentTrait)s).GetID(), (EquipmentTrait)s)).ToArray())
            });
        }
    }
    public static class QuestLogDB
    {
        public static void AddAllToList(QuestLogDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(QuestLog),
                SearchType = nameof(QuestLog),
                Database = saveSystem,
                GetId = obj => ((QuestLog)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((QuestLog)obj);
                    var newId = new QuestLogID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new QuestLogdatabaseSlot(((QuestLog)s).GetID(), (QuestLog)s)).ToArray())
            });
        }
    }
    public static class QuestDB
    {
        public static void AddAllToList(QuestDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(Quest),
                SearchType = nameof(Quest),
                Database = saveSystem,
                GetId = obj => ((Quest)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((Quest)obj);
                    var newId = new QuestID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new QuestDdatabaseSlot(((Quest)s).GetID(), (Quest)s)).ToArray())
            });
        }
    }
    /// <summary>
    /// item is abstract, contains magic string
    /// </summary>
    public static class ItemDB
    {
        public static void AddAllToList(ItemDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(Item),
                SearchType = nameof(Item),
                Database = saveSystem,
                GetId = obj => ((Item)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((Item)obj);
                    var newId = new ItemID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new ItemDatabaseSlot(((Item)s).GetID(), (Item)s)).ToArray())
            });
        }
    }
    public static class InvDatabase
    {
        public static void AddAllToList(InventoryDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(ActorInventory),
                SearchType = nameof(ActorInventory),
                Database = saveSystem,
                GetId = obj => ((ActorInventory)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((ActorInventory)obj);
                    var newId = new InventoryID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new InventoryDatabaseSlot(((ActorInventory)s).GetID(), (ActorInventory)s)).ToArray())
            });
        }
    }

    public static class ClassDatabase
    {
        public static void AddAllToList(ActorClassDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(ActorClass),
                SearchType = nameof(ActorClass),
                Database = saveSystem,
                GetId = obj => ((ActorClass)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((ActorClass)obj);
                    var newId = new ClassID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new ClassDatabaseSlot(((ActorClass)s).GetID(), (ActorClass)s)).ToArray())
            });
        }
    }

    public static class AuraControllersDatabase
    {
        public static void AddAllToList(AuraControllerDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(AuraController),
                SearchType = nameof(AuraController),
                Database = saveSystem,
                GetId = obj => ((AuraController)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((AuraController)obj);
                    var newId = new AuraControllerData(data.name, id, data);
                    data.AuraControllerData = newId;
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new AuraControllerDatabaseSlot(((AuraController)s).GetID(), (AuraController)s)).ToArray())
            });
        }
    }

    public static class LootDatabase
    {
        public static void AddAllToList(LootDropsDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(LootDrops),
                SearchType = nameof(LootDrops),
                Database = saveSystem,
                GetId = obj => ((LootDrops)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((LootDrops)obj);
                    var newId = new LootID(data.name, id, data);
                    data.ID = newId;
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new LootDropsDatabaseSlot(((LootDrops)s).GetID(), (LootDrops)s)).ToArray())
            });
        }
    }
    public static class AurasDatabase
    {
        public static void AddAllToList(AuraDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(Aura),
                SearchType = nameof(Aura),
                Database = saveSystem,
                GetId = obj => ((Aura)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((Aura)obj);
                    var newId = new AuraID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new AuraDatabaseSlot(((Aura)s).GetID(), (Aura)s)).ToArray())
            });
        }
    }

    public static class AttributesDatabase
    {
        public static void AddAllToList(ActorAttributesDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(ActorAttributes),
                SearchType = nameof(ActorAttributes),
                Database = saveSystem,
                GetId = obj => ((ActorAttributes)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((ActorAttributes)obj);
                    var newId = new AttributesID(data.name, id, data);
                    data.SetStatsID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new AttributesDatabaseSlot(((ActorAttributes)s).GetID(), (ActorAttributes)s)).ToArray())
            });
        }
    }
    public static class AbilityDatabase
    {
        public static void AddAllToList(AbilitiesDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(Ability),
                SearchType = nameof(Ability),
                Database = saveSystem,
                GetId = obj => ((Ability)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((Ability)obj);
                    var newId = new AbilityID(data.name, id, data);
                    data.SetID(newId);
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new AbilityDatabaseSlot(((Ability)s).GetID(), (Ability)s)).ToArray())
            });
        }
    }
    public static class AbilityControllersDatabase
    {
        public static void AddAllToList(AbilityControllerDatabase saveSystem)
        {
            CommonReload.AddAllToList(new CommonReload.CommonReloadData()
            {
                Type = typeof(AbilityController),
                SearchType = nameof(AbilityController),
                Database = saveSystem,
                GetId = obj => ((AbilityController)obj).GetID().ID,
                SetId = (obj, id) =>
                {
                    var data = ((AbilityController)obj);
                    data.GetID().ID = id;
                },
                SetSlots = (list) => saveSystem.SetSlots(list.Select(s => new AbilityControllerDatabaseSlot(((AbilityController)s).GetID(), (AbilityController)s)).ToArray())
            });
        }
    }
    public static class ARPGGameDatabase
    {
        public static void ReloadGameDatabase(GameDatabase gamedatabase)
        {
            List<IDatabase> databases = FindAttributes(gamedatabase);
            SetDatabase(databases.ToArray(), gamedatabase);
        }
        static void SetDatabase(IDatabase[] databases, GameDatabase gamedatabase)
        {
            List<DatabaseID> list = new List<DatabaseID>();
            List<IDatabase> databseslist = new List<IDatabase>();
            foreach (DatabaseID pieceType in DatabaseID.GetValues(typeof(DatabaseID)))
            {
                list.Add(pieceType);
            }

            gamedatabase.allPossibledatabaseTypes = list.ToArray();
            databseslist = new List<IDatabase>();
            databseslist.Add(gamedatabase);//force the game database to be first
            for (int i = 0; i < databases.Length; i++)
            {
                if (databases[i].GetDatabaseEntry() == DatabaseID.GameDatabase) continue;
                databseslist.Add(databases[i]);
            }
            gamedatabase.databases = new IDatabase[databseslist.Count];
            gamedatabase.databases = databseslist.ToArray();

            gamedatabase.names = new string[gamedatabase.databases.Length];
            gamedatabase.currentTypesInGame = new DatabaseID[gamedatabase.databases.Length];
            for (int i = 0; i < gamedatabase.names.Length; i++)
            {
                gamedatabase.names[i] = gamedatabase.databases[i].GetDatabaseEntry().ToString();
                gamedatabase.currentTypesInGame[i] = gamedatabase.databases[i].GetDatabaseEntry();
            }


            for (int i = 0; i < databases.Length; i++)
            {
                DatabaseID id = databases[i].GetDatabaseEntry();
                IDatabase idatabase = databases[i];
                bool assigned = AssignDatabase(id, idatabase, gamedatabase);

                if (assigned)
                {
                    list.Remove(id);
                }
            }

            //can only create in the editor
            for (int i = 0; i < list.Count; i++)
            {

                //does not have, wish to create?
                if (CreateDatabase(list[i], gamedatabase))
                {
                    Debug.LogWarning("Database not found. Creating a new one to add to the game database");

                }
            }
        }
        private static bool AssignDatabase(DatabaseID id, IDatabase idatabase, GameDatabase gamedatabase)
        {
            switch (id)
            {
                case DatabaseID.GameDatabase:
                    return true;
                case DatabaseID.AbilityControllers:
                    gamedatabase.AbilityControllers = idatabase as AbilityControllerDatabase;
                    return true;
                case DatabaseID.Abilities:
                    gamedatabase.Abilities = idatabase as AbilitiesDatabase;
                    return true;
                case DatabaseID.ActorDamageDealers:
                    gamedatabase.ActorDamageTypes = idatabase as ActorDamageDatabase;
                    return true;
                case DatabaseID.Attributes:
                    gamedatabase.Attributes = idatabase as ActorAttributesDatabase;
                    return true;
                case DatabaseID.Auras:
                    gamedatabase.Auras = idatabase as AuraDatabase;
                    return true;
                case DatabaseID.AuraControllers:
                    gamedatabase.AuraControllers = idatabase as AuraControllerDatabase;
                    return true;
                case DatabaseID.Classes:
                    gamedatabase.Classes = idatabase as ActorClassDatabase;
                    return true;
                case DatabaseID.Inventories:
                    gamedatabase.Inventories = idatabase as InventoryDatabase;
                    return true;
                case DatabaseID.Items:
                    gamedatabase.Items = idatabase as ItemDatabase;
                    return true;
                case DatabaseID.LootDrops:
                    gamedatabase.Loot = idatabase as LootDropsDatabase;
                    return true;
                case DatabaseID.Melees:
                    gamedatabase.Melee = idatabase as MeleeDataDatabase;
                    return true;
                case DatabaseID.Projectiles:
                    gamedatabase.Projectiles = idatabase as ProjectileDataDatabase;
                    return true;
                case DatabaseID.Questchains:
                    gamedatabase.Questchains = idatabase as QuestchainDatabase;
                    return true;
                case DatabaseID.Quests:
                    gamedatabase.Quests = idatabase as QuestDatabase;
                    return true;
                case DatabaseID.QuestLogs:
                    gamedatabase.QuestLog = idatabase as QuestLogDatabase;
                    return true;
                case DatabaseID.EquipmentTraits:
                    gamedatabase.Traits = idatabase as EquipmentTraitDatabase;
                    return true;

            }
            return false;
        }

        
        private static bool CreateDatabase(DatabaseID id, GameDatabase gamedatabase)
        {
            ScriptableObject scriptable = null;
            switch (id)
            {
                case DatabaseID.AuraControllers:
                    scriptable = ScriptableObject.CreateInstance<AuraControllerDatabase>() as AuraControllerDatabase;
                    gamedatabase.AuraControllers = scriptable as AuraControllerDatabase;
                    break;
                case DatabaseID.Abilities:
                    scriptable = ScriptableObject.CreateInstance<AbilitiesDatabase>() as AbilitiesDatabase;
                    gamedatabase.Abilities = scriptable as AbilitiesDatabase;
                    break;
                case DatabaseID.ActorDamageDealers:
                    scriptable = ScriptableObject.CreateInstance<ActorDamageDatabase>() as ActorDamageDatabase;
                    gamedatabase.ActorDamageTypes = scriptable as ActorDamageDatabase;
                    break;
                case DatabaseID.AbilityControllers:
                    scriptable = ScriptableObject.CreateInstance<AbilityControllerDatabase>() as AbilityControllerDatabase;
                    gamedatabase.AbilityControllers = scriptable as AbilityControllerDatabase;
                    break;
                case DatabaseID.Attributes:
                    scriptable = ScriptableObject.CreateInstance<ActorAttributesDatabase>() as ActorAttributesDatabase;
                    gamedatabase.Attributes = scriptable as ActorAttributesDatabase;
                    break;
                case DatabaseID.Auras:
                    scriptable = ScriptableObject.CreateInstance<AuraDatabase>() as AuraDatabase;
                    gamedatabase.Auras = scriptable as AuraDatabase;
                    break;
                case DatabaseID.Classes:
                    scriptable = ScriptableObject.CreateInstance<ActorClassDatabase>() as ActorClassDatabase;
                    gamedatabase.Classes = scriptable as ActorClassDatabase;
                    break;
                case DatabaseID.Inventories:
                    scriptable = ScriptableObject.CreateInstance<InventoryDatabase>() as InventoryDatabase;
                    gamedatabase.Inventories = scriptable as InventoryDatabase;
                    break;
                case DatabaseID.LootDrops:
                    scriptable = ScriptableObject.CreateInstance<LootDropsDatabase>() as LootDropsDatabase;
                    gamedatabase.Loot = scriptable as LootDropsDatabase;
                    break;
                case DatabaseID.Items:
                    scriptable = ScriptableObject.CreateInstance<ItemDatabase>() as ItemDatabase;
                    gamedatabase.Items = scriptable as ItemDatabase;
                    break;
                case DatabaseID.Melees:
                    scriptable = ScriptableObject.CreateInstance<MeleeDataDatabase>() as MeleeDataDatabase;
                    gamedatabase.Melee = scriptable as MeleeDataDatabase;
                    break;
                case DatabaseID.Projectiles:
                    scriptable = ScriptableObject.CreateInstance<ProjectileDataDatabase>() as ProjectileDataDatabase;
                    gamedatabase.Projectiles = scriptable as ProjectileDataDatabase;
                    break;
                case DatabaseID.Questchains:
                    scriptable = ScriptableObject.CreateInstance<QuestchainDatabase>() as QuestchainDatabase;
                    gamedatabase.Questchains = scriptable as QuestchainDatabase;
                    break;
                case DatabaseID.Quests:
                    scriptable = ScriptableObject.CreateInstance<QuestDatabase>() as QuestDatabase;
                    gamedatabase.Quests = scriptable as QuestDatabase;
                    break;
                case DatabaseID.QuestLogs:
                    scriptable = ScriptableObject.CreateInstance<QuestLogDatabase>() as QuestLogDatabase;
                    gamedatabase.QuestLog = scriptable as QuestLogDatabase;
                    break;
                case DatabaseID.EquipmentTraits:
                    scriptable = ScriptableObject.CreateInstance<EquipmentTraitDatabase>() as EquipmentTraitDatabase;
                    gamedatabase.Traits = scriptable as EquipmentTraitDatabase;
                    break;

            }

            if (scriptable != null)
            {
                //store the string in the same place as the game database
                IDatabase database = scriptable as IDatabase;

                string gamedatabaselocation = AssetDatabase.GetAssetPath(gamedatabase);
                //truncate path to just the folder

                string defaultdatabasename = "Database_" + database.GetDatabaseEntry().ToString();
                string defaultdatabaseextension = ".asset";

                string[] splitpath = gamedatabaselocation.Split('/');//split
                splitpath[splitpath.Length - 1] = string.Empty;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < splitpath.Length; i++)
                {
                    sb.Append(splitpath[i]);
                    if (i != splitpath.Length - 1)
                    {
                        sb.Append("/");
                    }
                }

                string truncatedsavepath = sb.ToString();
              
                string compiledstring = truncatedsavepath + defaultdatabasename + defaultdatabaseextension;

                string searchpath = truncatedsavepath.Substring(0, truncatedsavepath.LastIndexOf('/'));

                database.SetSearchFolders(new string[1] { searchpath });//default the search folders to where the database was created;

                UnityEditor.AssetDatabase.CreateAsset(scriptable, compiledstring);
                UnityEditor.AssetDatabase.SaveAssets();
                System.Array.Resize(ref gamedatabase.currentTypesInGame, gamedatabase.currentTypesInGame.Length + 1);
                UnityEngine.Object newDatabase = UnityEditor.AssetDatabase.LoadAssetAtPath(compiledstring, typeof(UnityEngine.Object));
                IDatabase databse = newDatabase as IDatabase;

                gamedatabase.currentTypesInGame[gamedatabase.currentTypesInGame.Length - 1] = databse.GetDatabaseEntry();
                return true;
            }
            return false;
        }


        static List<IDatabase> FindAttributes(GameDatabase saveSystem)
        {

            string[] folders = saveSystem.GetDatabaseFolders;
            string key = "ScriptableObject";
            string[] percents = UnityEditor.AssetDatabase.FindAssets("t:" + key, folders);//specific if you want by putting t:armor or t:equipment, etc.
            List<IDatabase> temp = new List<IDatabase>();
            foreach (var guid in percents)
            {
                string obj = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject newItem = UnityEditor.AssetDatabase.LoadAssetAtPath(obj, typeof(ScriptableObject)) as ScriptableObject;
                if (newItem is IDatabase)
                {
                    temp.Add(newItem as IDatabase);
                }
            }

            return temp;
        }
    }
    #endregion
    public static class DatabaseHandler
    {
        public static void CreateCopies(IDatabase database, string path)
        {
            string extension = ".asset";

            for (int i = 0; i < database.GetJsons().Length; i++)
            {
                Object objectToCopy = database.GetDatabaseObjectBySlotIndex(i);
                if (objectToCopy == null)
                {
                    UnityEngine.Debug.Log("Object is null at " + database.GetDatabaseEntry().ToString() + " index " + i.ToString());
                    continue;
                }
                Object obj = ScriptableObject.Instantiate(objectToCopy);
                string exportName = "\\" + obj.name + extension;
                AssetDatabase.CreateAsset(obj, path + exportName);

                //removes reference to text asset.
                Object ob = AssetDatabase.LoadAssetAtPath(path + exportName, typeof(Object));
                ISaveJsonConfig saver = ob as ISaveJsonConfig;
                saver.SetTextAsset(null);

            }
        }


          

        

        public static GameDatabase CreateNewGameDatabse(string pathwithname, bool withnewsubdatabases, string name)
        {
           GameDatabase gamedatase = ScriptableObject.CreateInstance<GameDatabase>();
           IDatabase idatabase = gamedatase as IDatabase;


            string[] splitpath = pathwithname.Split('/');//split
            splitpath[splitpath.Length - 1] = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < splitpath.Length; i++)
            {
                sb.Append(splitpath[i]);
                if (i != splitpath.Length - 1)
                {
                    sb.Append("/");
                }
            }

            string test = sb.ToString();
            string searchpath = test.Substring(0, test.LastIndexOf('/'));

            idatabase.SetSearchFolders(new string[1] { searchpath });

            UnityEditor.AssetDatabase.CreateAsset(gamedatase, pathwithname);
            UnityEditor.AssetDatabase.SaveAssets();


            //project settings
            string settingspathwithname = sb.ToString() + "Settings_" + name + ".asset";
            ProjectSettings newsettings = ScriptableObject.CreateInstance<ProjectSettings>();
            UnityEditor.AssetDatabase.CreateAsset(newsettings, settingspathwithname);
            UnityEditor.AssetDatabase.SaveAssets();


            UnityEditor.AssetDatabase.Refresh();
            UnityEngine.Object newgamedatabaseasset = UnityEditor.AssetDatabase.LoadAssetAtPath(pathwithname, typeof(UnityEngine.Object));
            GameDatabase gamed = newgamedatabaseasset as GameDatabase;
            UnityEngine.Object newgamesettings = UnityEditor.AssetDatabase.LoadAssetAtPath(settingspathwithname, typeof(UnityEngine.Object));
            ProjectSettings newsettingsgame = newgamesettings as ProjectSettings;
            gamed.Settings = newsettingsgame;

            UnityEditor.AssetDatabase.SaveAssets();
            if (withnewsubdatabases)
            {
                ReloadDatabase(newgamedatabaseasset);
            }

            return gamed;

        }
        public static void ReloadDatabase(UnityEngine.Object database)
        {
            if (database == null) return;
            IDatabase idatabase = (IDatabase)database;
            if (idatabase == null) return;
            DatabaseID id = idatabase.GetDatabaseEntry();
            switch (id)
            {
                case DatabaseID.Abilities:
                    ReloadDatabase((AbilitiesDatabase)database);
                    break;
                case DatabaseID.Attributes:
                    ReloadDatabase(database as ActorAttributesDatabase);
                    break;
                case DatabaseID.ActorDamageDealers:
                    ReloadDatabase(database as ActorDamageDatabase);
                    break;
                case DatabaseID.Auras:
                    ReloadDatabase(database as AuraDatabase);
                    break;
                case DatabaseID.Classes:
                    ReloadDatabase(database as ActorClassDatabase);
                    break;
                case DatabaseID.EquipmentTraits:
                    ReloadDatabase(database as EquipmentTraitDatabase);
                    break;
                case DatabaseID.GameDatabase:
                    ReloadDatabase(database as GameDatabase);
                    break;
                case DatabaseID.Inventories:
                    ReloadDatabase(database as InventoryDatabase);
                    break;
                case DatabaseID.Items:
                    ReloadDatabase(database as ItemDatabase);
                    break;
                case DatabaseID.LootDrops:
                    ReloadDatabase(database as LootDropsDatabase);
                    break;
                case DatabaseID.Melees:
                    ReloadDatabase(database as MeleeDataDatabase);
                    break;
                case DatabaseID.Projectiles:
                    ReloadDatabase(database as ProjectileDataDatabase);
                    break;
                case DatabaseID.Questchains:
                    ReloadDatabase(database as QuestchainDatabase);
                    break;
                case DatabaseID.Quests:
                    ReloadDatabase(database as QuestDatabase);
                    break;
                case DatabaseID.AuraControllers:
                    ReloadDatabase(database as AuraControllerDatabase);
                    break;
                case DatabaseID.AbilityControllers:
                    ReloadDatabase(database as AbilityControllerDatabase);
                    break;
                case DatabaseID.QuestLogs:
                    ReloadDatabase(database as QuestLogDatabase);
                    break;
                


            }
        }
        public static void ReloadDatabase(MeleeDataDatabase database)
        {
            MeleeDB.AddAllToList(database);
        }
        public static void ReloadDatabase(ProjectileDataDatabase database)
        {
            ProjecileDB.AddAllToList(database);
        }
        public static void ReloadDatabase(ActorDamageDatabase database)
        {
            ActorDamageDB.AddAllToList(database);
        }
        public static void ReloadDatabase(QuestLogDatabase database)
        {
            QuestLogDB.AddAllToList(database);
        }
        public static void ReloadDatabase(AbilityControllerDatabase database)
        {
            AbilityControllersDatabase.AddAllToList(database);
        }
        public static void ReloadDatabase(LootDropsDatabase database)
        {
            LootDatabase.AddAllToList(database);
          
        }
        public static void ReloadDatabase(AuraControllerDatabase database)
        {
            AuraControllersDatabase.AddAllToList(database);
        }
        public static void ReloadDatabase(GameDatabase database)
        {
            ARPGGameDatabase.ReloadGameDatabase(database);
        }
        public static void ReloadDatabase(AbilitiesDatabase database)
        {
            AbilityDatabase.AddAllToList(database);
        }
       
        public static void ReloadDatabase(ActorAttributesDatabase database)
        {
            AttributesDatabase.AddAllToList(database);
        }
        public static void ReloadDatabase(AuraDatabase database)
        {
            AurasDatabase.AddAllToList(database);
        }

        public static void ReloadDatabase(ActorClassDatabase database)
        {
            ClassDatabase.AddAllToList(database);
        }
        public static void ReloadDatabase(InventoryDatabase database)
        {
            InvDatabase.AddAllToList(database);
        }
        public static void ReloadDatabase(ItemDatabase database)
        {
            ItemDB.AddAllToList(database);
        }
        public static void ReloadDatabase(QuestchainDatabase database)
        {
            QuestChainDB.AddAllToList(database);
        }
        public static void ReloadDatabase(QuestDatabase database)
        {
            QuestDB.AddAllToList(database);
        }

        public static void ReloadDatabase(EquipmentTraitDatabase database)
        {
            TraitDB.AddAllToList(database);
        }


        public static void ReloadAll(GameDatabase gamedatabase)
        {
            DatabaseHandler.ReloadDatabase(gamedatabase.Abilities);
            DatabaseHandler.ReloadDatabase(gamedatabase.AbilityControllers);
            DatabaseHandler.ReloadDatabase(gamedatabase.ActorDamageTypes);
            DatabaseHandler.ReloadDatabase(gamedatabase.Attributes);
            DatabaseHandler.ReloadDatabase(gamedatabase.Auras);
            DatabaseHandler.ReloadDatabase(gamedatabase.AuraControllers);
            DatabaseHandler.ReloadDatabase(gamedatabase.Classes);
            DatabaseHandler.ReloadDatabase(gamedatabase.Inventories);
            DatabaseHandler.ReloadDatabase(gamedatabase.Items);
            DatabaseHandler.ReloadDatabase(gamedatabase.Loot);
            DatabaseHandler.ReloadDatabase(gamedatabase.Melee);
            DatabaseHandler.ReloadDatabase(gamedatabase.Projectiles);
            DatabaseHandler.ReloadDatabase(gamedatabase.Traits);
            DatabaseHandler.ReloadDatabase(gamedatabase.Questchains);
            DatabaseHandler.ReloadDatabase(gamedatabase.Quests);
            DatabaseHandler.ReloadDatabase(gamedatabase.QuestLog);

        }

        static string GetImportText()
        {
            string path = EditorUtility.OpenFilePanel("Import", "Assets", "txt");
            string text = System.IO.File.ReadAllText(path);
            //Debug.Log(text);
            //Debug.Log(path);
            return text;
        }

     
        


        public static string GetDBCSV(IDatabase forDatabase)
        {
            switch (forDatabase.GetDatabaseEntry())
            {
                case DatabaseID.Abilities:
                    return GetAbilityDBCSV(forDatabase as AbilitiesDatabase);
                case DatabaseID.ActorDamageDealers:
                    return GetActorDamageDBCSV(forDatabase as ActorDamageDatabase);
                case DatabaseID.Projectiles:
                    return GetProjectileDataDBCSV(forDatabase as ProjectileDataDatabase);
                case DatabaseID.Attributes:
                    return GetActorAttributesDBCSV(forDatabase as ActorAttributesDatabase);
                case DatabaseID.EquipmentTraits:
                    return GetTraitDBCSV(forDatabase as EquipmentTraitDatabase);
                case DatabaseID.Items:
                    return GetItemDBCSV(forDatabase as ItemDatabase);


            }

            return string.Empty;
        }

        public static void ImportDBCSV(IDatabase forDatabase)
        {
            switch (forDatabase.GetDatabaseEntry())
            {
                case DatabaseID.ActorDamageDealers:
                    ImportActorDamageDBCSV(forDatabase as ActorDamageDatabase);
                    break;
                case DatabaseID.Projectiles:
                    ImportProjectileDataDBCSV(forDatabase as ProjectileDataDatabase);
                    break;
                case DatabaseID.Abilities:
                    ImportAbilityDBText(forDatabase as AbilitiesDatabase);
                    break;
                case DatabaseID.Attributes:
                    ImportActorAttributesDBCSV(forDatabase as ActorAttributesDatabase);
                    break;
                case DatabaseID.EquipmentTraits:
                    ImportTraitDataDBCSV(forDatabase as EquipmentTraitDatabase);
                    break;
            }

            AssetDatabase.Refresh();
        }

        static void ImportAbilityDBText(AbilitiesDatabase database)
        {
            string text = GetImportText();
            AbilityDatabaseCSV empty = new AbilityDatabaseCSV(string.Empty, 0, null);
            UnityEngine.JsonUtility.FromJsonOverwrite(text, empty);

            //Debug.Log(empty);

            if (empty.DatabaseID == (int)database.GetDatabaseEntry())
            {
                //unpack
                for (int i = 0; i < empty.Entries.Length; i++)
                {
                    AbilityCSV csv = empty.Entries[i];
                    int id = csv.Entry;
                    Ability ability = database.FindAbilityByID(id);
                    if (ability == null)
                    {
                        Debug.LogWarning("No entry with ID " + id.ToString() + " found in database");
                        continue;
                    }
                    Debug.Log("Found " + ability);
                    ability.Data.Name = csv.Name;
                    ability.AutoName(csv.Name);

                    ability.Data.Description = csv.Description;
                    ability.Data.DamageMultiplier = csv.DamageMultipler;
                    ability.Data.Range = csv.Range;
                    ability.Data.ResourceCost = csv.ResourceCost;

                    ResourceType yourEnum;
                    if (System.Enum.TryParse<ResourceType>(csv.ResourceType, out yourEnum))
                    {
                        ability.Data.ResourceType = yourEnum;
                    }

                }
            }
        }

        static void ImportActorAttributesDBCSV(ActorAttributesDatabase database)
        {
            string textfile = EditorUtility.OpenFilePanel("Open File", "Assets", "txt");
            if (textfile.Length > 0)
            {
                ActorAttributesDatabaseCSV loadCSV = new ActorAttributesDatabaseCSV(null, 0, null);
                string read = System.IO.File.ReadAllText(textfile);
                UnityEngine.JsonUtility.FromJsonOverwrite(read, loadCSV);

                if (loadCSV.DatabaseID == (int)database.GetDatabaseEntry())
                {
                    //valid
                    for (int i = 0; i < loadCSV.Entries.Length; i++)
                    {
                        ActorAttributesCSV csv = loadCSV.Entries[i];
                        int id = csv.Entry;
                        ActorAttributes data = database.FindActorStatsByID(id);
                        if (data != null)
                        {
                            data.ActorName = csv.Name;
                            Dictionary<AttributeType, Attribute[]> all = data.GetAllAttributesByType();
                            AttributeCSVValue[] values = csv.AttributeValues;
                            for (int j = 0; j < values.Length; j++)
                            {
                                AttributeCSVValue value = values[j];
                                AttributeType type = (AttributeType)value.AttributeCategoryType;
                                int starting = value.StartingValue;
                                int max = value.MaxValue;
                                int subtype = value.AttributeSubType;

                                all.TryGetValue(type, out Attribute[] attvalues);
                                for (int k = 0; k < attvalues.Length; k++)
                                {
                                    if (attvalues[k].GetSubType() == subtype)
                                    {
                                        //found it
                                        attvalues[k].Level1Value = starting;
                                        attvalues[k].Level99Max = max;
                                    }
                                }
                            }


                            EditorUtility.SetDirty(data);
                        }
                        else
                        {
                            //print some warning
                        }
                    }
                }

            }
        }
        static void ImportActorDamageDBCSV(ActorDamageDatabase database)
        {
            string textfile = EditorUtility.OpenFilePanel("Open File", "Assets", "txt");
            if (textfile.Length > 0)
            {
                ActorDamageDatabaseCSV loadCSV = new ActorDamageDatabaseCSV(null, 0, null);
                string read = System.IO.File.ReadAllText(textfile);
                UnityEngine.JsonUtility.FromJsonOverwrite(read, loadCSV);

                if (loadCSV.DatabaseID == (int)database.GetDatabaseEntry())
                {
                    //valid
                    for (int i = 0; i < loadCSV.Entries.Length; i++)
                    {
                        int id = loadCSV.Entries[i].Entry;
                        ActorDamageData data = database.FindInstanceByID(id);
                        if (data != null)
                        {
                            DamageOptions newoptions = loadCSV.Entries[i].DamageOptions;
                            DamageMultiplers_Actor importdm = loadCSV.Entries[i].DamageMultiplers;
                            StatusOverTimeOptions sotoptions = loadCSV.Entries[i].SoTOptions;
                            DamageOverTimeMultipliers sotmulties = loadCSV.Entries[i].SoTMultipliers;
                            CombatFormulas handler = data.DamageVar.CombatHandler;
                            string newname = loadCSV.Entries[i].Name;
                            data.DamageVar = new DamageDealerForActor(newname, handler, newoptions, importdm, sotoptions, sotmulties);
                            EditorUtility.SetDirty(data);
                        }
                        else
                        {
                            //print some warning
                        }
                    }
                }

            }
        }
        static void ImportProjectileDataDBCSV(ProjectileDataDatabase database)
        {
            string textfile = EditorUtility.OpenFilePanel("Open File", "Assets", "txt");
            if (textfile.Length > 0)
            {
                ProjectileDataDatabaseCSV loadCSV = new ProjectileDataDatabaseCSV(null, 0, null);
                string read = System.IO.File.ReadAllText(textfile);
                UnityEngine.JsonUtility.FromJsonOverwrite(read, loadCSV);

                if (loadCSV.DatabaseID == (int)database.GetDatabaseEntry())
                {
                    //valid
                    for (int i = 0; i < loadCSV.Entries.Length; i++)
                    {
                        int id = loadCSV.Entries[i].Entry;
                        ProjectileData data = database.FindInstanceByID(id);
                        if (data != null)
                        {
                            ProjectileOptions impot = loadCSV.Entries[i].Options;
                            data.ProjectileVars = impot;
                            EditorUtility.SetDirty(data);
                        }
                        else
                        {
                            //print some warning
                        }
                    }
                }

            }
        }

        static void ImportTraitDataDBCSV(EquipmentTraitDatabase database)
        {
            string textfile = EditorUtility.OpenFilePanel("Open File", "Assets", "txt");
            if (textfile.Length > 0)
            {
                TraitDatabaseCSV loadCSV = new TraitDatabaseCSV(null, 0, null);
                string read = System.IO.File.ReadAllText(textfile);
                UnityEngine.JsonUtility.FromJsonOverwrite(read, loadCSV);

                if (loadCSV.DatabaseID == (int)database.GetDatabaseEntry())
                {
                    //valid
                    for (int i = 0; i < loadCSV.Entries.Length; i++)
                    {
                        int id = loadCSV.Entries[i].Entry;
                        EquipmentTrait data = database.FindTraitByID(id);
                        if (data != null)
                        {
                            TraitDatabaseCSVValue load = loadCSV.Entries[i].Value;

                            if (load.TraitCategoryType != (int)data.GetTraitType())
                            {
                                //warnign message, dont do the override unless permission granted
                            }

                            data.SetTraitName(load.TraitName);
                            data.SetPrefixes(load.Prefixes);
                            data.SetSuffixes(load.Suffixes);
                            data.SetMulti(load.ILevelMulti);
                            data.SetWeight(load.Weight);

                          
                            EditorUtility.SetDirty(data);
                        }
                        else
                        {
                            //print some warning
                        }
                    }
                }

            }
        }

        static string GetItemDBCSV(ItemDatabase database)
        {

            ItemCSV[] ntriesCSV = new ItemCSV[database.Slots.Length];
            for (int i = 0; i < database.Slots.Length; i++)
            {
                Item instance = database.Slots[i].Item;

                ItemCSV newcsv = new ItemCSV();

                newcsv.Entry = instance.GetID().ID;
                newcsv.Name = instance.GetID().Name;
                ItemCSVValue value = new ItemCSVValue(
                    instance.GetItemType().ToString(),
                    (int)instance.GetItemType(),
                    instance.GetBaseItemName());
                newcsv.ItemValue = value;

                ItemType type = instance.GetItemType();
                switch (type)
                {
                    case ItemType.Equipment:
                        Equipment equipment = instance as Equipment;

                        EquipmentType eqtype = equipment.GetEquipmentType();
                        switch (eqtype)
                        {
                            case EquipmentType.Weapon:
                                Weapon weapon = equipment as Weapon;
                                WeaponType wtype = weapon.GetWeaponType();

                                break;
                        }

                        EquipmentSlotsType[] slots = equipment.GetEquipmentSlot();
                        string[] slotnames = new string[slots.Length];
                        int[] slottypes = new int[slots.Length];
                        for (int j = 0; j < slots.Length; j++)
                        {
                            slotnames[j] = slots[j].ToString();
                            slottypes[j] = (int)slots[j];
                        }
                        EquipmentCSVValue eqvalue = new EquipmentCSVValue(
                            equipment.GetEquipmentType().ToString(),
                            (int)equipment.GetEquipmentType(),
                            equipment.GetBaseItemName(),
                            slotnames,
                            slottypes,
                            equipment.GetStats().GetBaseType().ToString(),
                            (int)equipment.GetStats().GetBaseType(),
                            equipment.GetStats().GetIlevel());

                        newcsv.EquipmentValue = eqvalue;
                        break;
                    case ItemType.Potions:
                        Potion potion = instance as Potion;
                        PotionCSVValue potvalue = new PotionCSVValue(
                            potion.GetPotionType().ToString(),
                            (int)potion.GetPotionType());
                        newcsv.PotionValue = potvalue;
                        break;

                }

                ntriesCSV[i] = newcsv;
            }

            ItemDatabaseCSV csvEntries = new ItemDatabaseCSV(database.GetDatabaseEntry().ToString(), (int)database.GetDatabaseEntry(), ntriesCSV);
            string savedJson = UnityEngine.JsonUtility.ToJson(csvEntries, true);


            WriteFile(savedJson, database.name);



            return savedJson;
        }
        static string GetTraitDBCSV(EquipmentTraitDatabase database)
        {

            TraitCSV[] ntriesCSV = new TraitCSV[database.Slots.Length];
            for (int i = 0; i < database.Slots.Length; i++)
            {
                EquipmentTrait instance = database.Slots[i].Trait;

                TraitCSV newcsv = new TraitCSV();

                newcsv.Entry = instance.GetID().ID;
                newcsv.Name = instance.GetID().Name;
                TraitDatabaseCSVValue value = new TraitDatabaseCSVValue(
                    instance.GetTraitType().ToString(),
                    (int)instance.GetTraitType(),
                    instance.GetTraitName(),
                    instance.GetPrefixes(),
                    instance.GetSuffixes(),
                    instance.GetMyLevelMultRaw(),
                    instance.GetWeight());

                newcsv.Value = value;


                ntriesCSV[i] = newcsv;
            }

            TraitDatabaseCSV csvEntries = new TraitDatabaseCSV(database.GetDatabaseEntry().ToString(), (int)database.GetDatabaseEntry(), ntriesCSV);
            string savedJson = UnityEngine.JsonUtility.ToJson(csvEntries, true);


            WriteFile(savedJson, database.name);



            return savedJson;
        }

        static string GetActorDamageDBCSV(ActorDamageDatabase database)
        {

            ActorDamageCSV[] ntriesCSV = new ActorDamageCSV[database.Slots.Length];
            for (int i = 0; i < database.Slots.Length; i++)
            {
                ActorDamageData instance = database.Slots[i].Instance;

                ActorDamageCSV newcsv = new ActorDamageCSV();

                newcsv.Entry = instance.GetID().ID;
                newcsv.Name = instance.GetName();
                newcsv.DamageOptions = instance.DamageVar.DamageOptions;
                newcsv.DamageMultiplers = instance.DamageVar.DamageMultipliers;
                newcsv.SoTOptions = instance.DamageVar.SoTOptions;
                newcsv.SoTMultipliers = instance.DamageVar.SoTOverTimeMultipliers;
                //save what i want here



                ntriesCSV[i] = newcsv;
            }

            ActorDamageDatabaseCSV csvEntries = new ActorDamageDatabaseCSV(database.GetDatabaseEntry().ToString(), (int)database.GetDatabaseEntry(), ntriesCSV);
            string savedJson = UnityEngine.JsonUtility.ToJson(csvEntries, true);


            WriteFile(savedJson, database.name);


            return savedJson;
        }
        static string GetProjectileDataDBCSV(ProjectileDataDatabase database)
        {

            ProjectileDataCSV[] ntriesCSV = new ProjectileDataCSV[database.Slots.Length];
            for (int i = 0; i < database.Slots.Length; i++)
            {
                ProjectileData instance = database.Slots[i].Instance;

                ProjectileDataCSV newcsv = new ProjectileDataCSV();

                newcsv.Entry = instance.GetID().ID;
                newcsv.Options = new ProjectileOptions
                    (
                    instance.ProjectileVars.Name,
                    instance.ProjectileVars.Description,
                    instance.ProjectileVars.Speed,
                    instance.ProjectileVars.DisableOnTouch,
                    instance.ProjectileVars.LifeTime,
                    instance.ProjectileVars.FriendlyFire
                    );

                //save what i want here



                ntriesCSV[i] = newcsv;
            }

            ProjectileDataDatabaseCSV csvEntries = new ProjectileDataDatabaseCSV(database.GetDatabaseEntry().ToString(), (int)database.GetDatabaseEntry(), ntriesCSV);
            string savedJson = UnityEngine.JsonUtility.ToJson(csvEntries, true);
            WriteFile(savedJson, database.name);
            return savedJson;
        }

       

        static string GetActorAttributesDBCSV(ActorAttributesDatabase database)
        {
      

            ActorAttributesCSV[] ntriesCSV = new ActorAttributesCSV[database.Slots.Length];
            for (int i = 0; i < database.Slots.Length; i++)
            {
                ActorAttributes instance = database.Slots[i].ActorStats;

                ActorAttributesCSV newcsv = new ActorAttributesCSV();

                newcsv.Entry = instance.GetID().ID;
                newcsv.Name = instance.ActorName;
                Dictionary<AttributeType, Attribute[]> attributes = instance.GetAllAttributesByType();
                List<AttributeCSVValue> _temp = new List<AttributeCSVValue>();
                foreach (var kvp in attributes)
                {
                    int id = (int)kvp.Key;
                    string attname = kvp.Key.ToString();
                    for (int j = 0; j < kvp.Value.Length; j++)
                    {
                        int starting = kvp.Value[j].Level1Value;
                        int max = kvp.Value[j].Level99Max;
                        int subtype = kvp.Value[j].GetSubType();
                        string attributeName = kvp.Value[j].GetDescriptiveName();
                        Attribute attribute = kvp.Value[j];
                       
                        AttributeCSVValue newvalues = new AttributeCSVValue(attname, id, attributeName, subtype, starting, max);
                        _temp.Add(newvalues);
                    }

                }

                newcsv.AttributeValues = _temp.ToArray();
                //save what i want here
                ntriesCSV[i] = newcsv;
            }

            ActorAttributesDatabaseCSV csvEntries = new ActorAttributesDatabaseCSV(database.GetDatabaseEntry().ToString(), (int)database.GetDatabaseEntry(), ntriesCSV);
            string savedJson = UnityEngine.JsonUtility.ToJson(csvEntries, true);

            string name = database.name;
            WriteFile(savedJson, name);

            return savedJson;
        }
        static string GetAbilityDBCSV(AbilitiesDatabase database)
        {

            AbilityCSV[] ntriesCSV = new AbilityCSV[database.Slots.Length];
            for (int i = 0; i < database.Slots.Length; i++)
            {
                Ability ability = database.Slots[i].Ability;

                AbilityCSV newcsv = new AbilityCSV();

                newcsv.Entry = ability.GetID().ID;
                newcsv.Name = ability.Data.Name;
                newcsv.Description = ability.Data.Description;
                newcsv.DamageMultipler = ability.Data.DamageMultiplier;
                newcsv.Range = ability.Data.Range;
                newcsv.ResourceCost = ability.Data.ResourceCost;
                newcsv.ResourceType = ability.Data.ResourceType.ToString();
                //save what i want here



                ntriesCSV[i] = newcsv;
            }

            AbilityDatabaseCSV csvEntries = new AbilityDatabaseCSV(database.GetDatabaseEntry().ToString(), (int)database.GetDatabaseEntry(), ntriesCSV);
            string savedJson = UnityEngine.JsonUtility.ToJson(csvEntries, true);

            string name = database.name;
            WriteFile(savedJson, name);

            return savedJson;
        }

        private static void WriteFile(string savedJson, string name)
        {
            string newPath = EditorUtility.SaveFilePanelInProject("Export Text", name + "_Export", "txt", "Export?");
            if (newPath.Length > 0)
            {
                System.IO.File.WriteAllText(newPath, savedJson);
            }

            AssetDatabase.SaveAssets();

        }
    }

  
}
#endif