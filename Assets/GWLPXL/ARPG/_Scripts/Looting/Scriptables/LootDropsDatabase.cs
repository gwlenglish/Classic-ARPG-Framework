
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Saving.com;

namespace GWLPXL.ARPGCore.Looting.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_LootDrops")]

    public class LootDropsDatabase : ScriptableObject, IDatabase
    {
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        LootDropsDatabaseSlot[] slots = new LootDropsDatabaseSlot[0];
        int windowRowSize = 6;
        #region window
        public DatabaseID GetDatabaseEntry() => DatabaseID.LootDrops;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;

        public int GetWindowRowSize() => windowRowSize;
        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(LootDropsDatabaseSlot[] newSlots) => slots = newSlots;
        public LootDropsDatabaseSlot[] GetSlots() => slots;
        public UnityEngine.Object GetDatabaseObjectBySlotIndex(int slotIndex)
        {
            if (slotIndex > slots.Length - 1 || slotIndex < 0) return null;
            return (Object)slots[slotIndex].LootDrops as UnityEngine.Object;
        }
        public ISaveJsonConfig[] GetJsons()
        {
            ISaveJsonConfig[] saves = new ISaveJsonConfig[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                saves[i] = slots[i] as ISaveJsonConfig;
            }
            return saves;
        }
        public string[] GetAllNames()
        {
            string[] newone = new string[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                newone[i] = slots[i].DescriptiveName;
            }
            return newone;
        }
        #endregion
        #region saving/loading
        public int GeLootID(LootDrops fordrops)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (fordrops == slots[i].ID.LootDrops)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }
        public LootDrops FindLootByID(int lootid)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (lootid == slots[i].ID.ID)
                {
                    return slots[i].ID.LootDrops as LootDrops;
                }
            }
            return null;
        }

        public Object GetMyObject() => this;
      


        #endregion

    }
    [System.Serializable]
    public class LootID
    {
        public string Name;
        public int ID;
        public LootDrops LootDrops;

        public LootID(string name, int id, LootDrops lootdrops)
        {
            Name = name;
            ID = id;
            LootDrops = lootdrops;
        }
    }

    [System.Serializable]
    public class LootDropsDatabaseSlot
    {
        public string DescriptiveName;
        public LootID ID;
        public LootDrops LootDrops;
        public LootDropsDatabaseSlot(LootID id, LootDrops lootdrops)
        {
            ID = id;
            LootDrops = lootdrops;
            ID.LootDrops = lootdrops;
            DescriptiveName = LootDrops.ID.Name;
            LootDrops.ID.ID = id.ID;

        }
    }

}
