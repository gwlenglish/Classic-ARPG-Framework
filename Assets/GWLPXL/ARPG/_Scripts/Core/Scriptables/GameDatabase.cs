using GWLPXL.ARPGCore.Abilities.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Auras.com;
using GWLPXL.ARPGCore.Classes.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Traits.com;
using GWLPXL.ARPGCore.Combat.com;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GWLPXL.ARPGCore.com
{
   
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/NEW_GameDatabase")]
    public class GameDatabase : ScriptableObject, IDatabase
    {
        public AbilitiesDatabase Abilities;
        public AbilityControllerDatabase AbilityControllers;
        public ActorAttributesDatabase Attributes;
        public ActorDamageDatabase ActorDamageTypes;
        public AuraDatabase Auras;
        public AuraControllerDatabase AuraControllers;
        public ActorClassDatabase Classes;
        public InventoryDatabase Inventories;
        public ItemDatabase Items;
        public LootDropsDatabase Loot;
        public MeleeDataDatabase Melee;
        public ProjectileDataDatabase Projectiles;
        public QuestchainDatabase Questchains;
        public QuestDatabase Quests;
        public QuestLogDatabase QuestLog;
        public EquipmentTraitDatabase Traits;
        public string[] GetDatabaseFolders => databaseFolders;
        public ProjectSettings Settings;
        public AffixReaderSO AffixReader;

        [SerializeField]
        string[] databaseFolders = new string[1] { "Assets/GWLPXL/ARPG/Data/Databases" };
        
 
        public DatabaseID[] allPossibledatabaseTypes = new DatabaseID[0];
        public DatabaseID[] currentTypesInGame = new DatabaseID[0];
        public IDatabase[] databases = new IDatabase[0];
        public string[] names = new string[0];

        public DatabaseID[] GetDatabaseTypes() => currentTypesInGame;

        //this is really an editor only function

        public void SetSearchFolders(string[] newfolders) => databaseFolders = newfolders;



        public string[] GetSearchFolders() => databaseFolders;


        public string[] GetAllNames() => names;
      

        public ISaveJsonConfig[] GetJsons()
        {
            List<ISaveJsonConfig> jsons = new List<ISaveJsonConfig>();
            for (int i = 0; i < databases.Length; i++)
            {
                if (databases[i].GetDatabaseEntry() == DatabaseID.GameDatabase) continue;//skip it, or we will get a stack overflow

                for (int j = 0; j < databases[i].GetJsons().Length; j++)
                {
                    jsons.Add(databases[i].GetJsons()[j]);
                }
            
            }
            return jsons.ToArray();
        }
      
    
        public Object GetDatabaseObjectBySlotIndex(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > currentTypesInGame.Length - 1) return null;

            switch (currentTypesInGame[slotIndex])
            {
                case DatabaseID.Abilities:
                    return Abilities;
                case DatabaseID.AbilityControllers:
                    return AbilityControllers;
                case DatabaseID.ActorDamageDealers:
                    return ActorDamageTypes;
                case DatabaseID.Attributes:
                    return Attributes;
                case DatabaseID.Auras:
                    return Auras;
                case DatabaseID.Classes:
                    return Classes;
                case DatabaseID.EquipmentTraits:
                    return Traits;
                case DatabaseID.Inventories:
                    return Inventories;
                case DatabaseID.Items:
                    return Items;
                case DatabaseID.Melees:
                    return Melee;
                case DatabaseID.Projectiles:
                    return Projectiles;
                case DatabaseID.Questchains:
                    return Questchains;
                case DatabaseID.Quests:
                    return Quests;
                case DatabaseID.QuestLogs:
                    return QuestLog;
                case DatabaseID.GameDatabase:
                    return this;
                case DatabaseID.AuraControllers:
                    return AuraControllers;
                case DatabaseID.LootDrops:
                    return Loot;
            }

            return null;
        }

        public int GetWindowRowSize()
        {
            return allPossibledatabaseTypes.Length;
        }

        public DatabaseID GetDatabaseEntry()
        {
            return DatabaseID.GameDatabase;
        }

        public Object GetMyObject() => this;
        
    }
}