
using UnityEngine;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Traits.com
{
    [System.Serializable]
    public class TraitDatabaseCSV
    {
        public string DatabaseName;
        public int DatabaseID;
        public TraitCSV[] Entries = new TraitCSV[0];
        public TraitDatabaseCSV(string dbName, int dbID, TraitCSV[] _entries)
        {
            DatabaseName = dbName;
            DatabaseID = dbID;
            Entries = _entries;
        }
    }

    [System.Serializable]
    public class TraitDatabaseCSVValue
    {
        public string TraitCategoryName = string.Empty;
        public int TraitCategoryType = 0;
        public string TraitName = string.Empty;
        public string[] Prefixes = new string[0];
        public string[] Suffixes = new string[0];
        public int ILevelMulti = 1;
        public int Weight = 1;

        public TraitDatabaseCSVValue(string categoryname, int categorytype, string traitname, string[] prefixes, string[] suffixes, int ilevelMult, int weight)
        {
            TraitCategoryName = categoryname;
            TraitCategoryType = categorytype;
            TraitName = traitname;

            Prefixes = prefixes;
            Suffixes = suffixes;
            ILevelMulti = ilevelMult;
            Weight = weight;

        }
    }

    [System.Serializable]
    public class TraitCSV
    {
        public int Entry;
        public string Name;
        public TraitDatabaseCSVValue Value;

    }

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Traits")]

    public class EquipmentTraitDatabase : ScriptableObject, IDatabase
    {
        public TraitDatabaseSlot[] Slots => slots;
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG/Data" };
        [SerializeField]
        TraitDatabaseSlot[] slots = new TraitDatabaseSlot[0];
        readonly int rowsize = 6;
        /// <summary>
        /// since traits are abstract, this is a magic string to find the abstract types. if you change the base class name, you'll need to change this string to match. 
        /// </summary>
        readonly string magicString = "equipmenttrait";
        public DatabaseID GetDatabaseEntry() => DatabaseID.EquipmentTraits;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;
        public TraitDatabaseSlot[] GetSlots() => slots;

        public string GetMagicString() => magicString;
        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(TraitDatabaseSlot[] newSlot)
        {
            slots = newSlot;
        }


        public int GetTraitID(EquipmentTrait forTrait)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (forTrait == slots[i].Trait)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }
        public EquipmentTrait FindTraitByID(int itemID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (itemID == slots[i].ID.ID)
                {
                    return slots[i].Trait as EquipmentTrait;
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
                jsons[i] = slots[i].Trait as ISaveJsonConfig;
            }
            return jsons;
        }

        public Object GetDatabaseObjectBySlotIndex(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > slots.Length - 1) return null;
            return slots[slotIndex].Trait;
        }

        public int GetWindowRowSize()
        {
            return rowsize;
        }

        public Object GetMyObject() => this;
        
    }

}

