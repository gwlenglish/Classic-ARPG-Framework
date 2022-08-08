
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{
    [System.Serializable]
    public class ItemDatabaseCSV
    {
        public string DatabaseName;
        public int DatabaseID;
        public ItemCSV[] Entries = new ItemCSV[0];
        public ItemDatabaseCSV(string dbName, int dbID, ItemCSV[] _entries)
        {
            DatabaseName = dbName;
            DatabaseID = dbID;
            Entries = _entries;
        }
    }

    [System.Serializable]
    public class PotionCSVValue
    {
        public string PotionTypeName;
        public int PotionType;


        public PotionCSVValue(string potiontypename, int potionType)
        {
            PotionTypeName = potiontypename;
            PotionType = potionType;
        }

    }

    [System.Serializable]
    public class WeaponCSVValue
    {

    }

    [System.Serializable]
    public class EquipmentCSVValue
    {
        public string EquipmentTypeName;
        public int EquipmentType;
        public string BaseName;

        public string[] EquipmentSlotNames;
        public int[] EquipmentSlotTypes;
        public string CombatName;
        public int CombatType;
        public int ILevel;

        public EquipmentCSVValue(string equipmenttypename, int equipmenttype, string basename, string[] slotNames, int[] slotTypes, string combatname, int combattype, int ilevel)
        {
            EquipmentTypeName = equipmenttypename;
            EquipmentType = equipmenttype;
            BaseName = basename;

            EquipmentSlotNames = slotNames;
            EquipmentSlotTypes = slotTypes;
            CombatName = combatname;
            CombatType = combattype;
            ILevel = ilevel;
        }
    }

    [System.Serializable]
    public class ItemCSVValue
    {
        public string ItemCategoryName;
        public int ItemCategoryType;
        public string ItemName;

        public ItemCSVValue(string catname, int cattype, string itemname)
        {
            ItemCategoryName = catname;
            ItemCategoryType = cattype;
            ItemName = itemname;

        }
    }

    [System.Serializable]
    public class ItemCSV
    {
        public int Entry = -1;
        public string Name = string.Empty;
        public ItemCSVValue ItemValue = null;
        public EquipmentCSVValue EquipmentValue = null;
        public PotionCSVValue PotionValue = null;


    }


    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Items")]
    public class ItemDatabase : ScriptableObject, IDatabase
    {
        public ItemDatabaseSlot[] Slots => slots;
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        ItemDatabaseSlot[] slots = new ItemDatabaseSlot[0];
        readonly int rowsize = 6;
        readonly string magicString = "Item";//used for database handling since item is abstract. If the class name item changes, you'll need to change this.
        public string GetMagicString() => magicString;
        public DatabaseID GetDatabaseEntry() => DatabaseID.Items;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;
        public ItemDatabaseSlot[] GetSlots() => slots;
        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(ItemDatabaseSlot[] newSlots) => slots = newSlots;
      

        public int GetItemID(Equipment forEquipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (forEquipment == slots[i].Item)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }
        public Equipment FindEquipmentByID(int itemID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (itemID == slots[i].ID.ID)
                {
                    return slots[i].Item as Equipment;
                }
            }
            return null;
        }
        public SocketItem FindSocketableByID(int itemID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (itemID == slots[i].ID.ID)
                {
                    return slots[i].Item as SocketItem;
                }
            }
            return null;
        }
        public Potion FindPotionByID(int itemID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (itemID == slots[i].ID.ID)
                {
                    return slots[i].Item as Potion;
                }
            }
            return null;
        }

        public string[] GetAllNames()
        {
            string[] names = new string[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                names[i] = slots[i].DescriptiveName;
            }
            return names;
        }
        public ISaveJsonConfig[] GetJsons()
        {
            ISaveJsonConfig[] jsons = new ISaveJsonConfig[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                jsons[i] = slots[i] as ISaveJsonConfig;
            }
            return jsons;
        }

        public Object GetDatabaseObjectBySlotIndex(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > slots.Length - 1) return null;
            return slots[slotIndex].Item;
        }

        public int GetWindowRowSize()
        {
            return rowsize;
        }

        public Object GetMyObject() => this;
       
    }

    [System.Serializable]
    public class ItemID
    {
        public string Name;
        public int ID;
        public Item Item;

        public ItemID(string name, int id, Item item)
        {
            Name = name;
            ID = id;
            Item = item;
        }
    }

    [System.Serializable]
    public class ItemDatabaseSlot
    {
        public string DescriptiveName;
        public ItemID ID;
        public Item Item;
        public ItemDatabaseSlot(ItemID id, Item item)
        {
            Item = item;
            ID = id;
            id.Item = item;
            DescriptiveName = item.GetBaseItemName();
            item.SetID(id);
        }
    }
}