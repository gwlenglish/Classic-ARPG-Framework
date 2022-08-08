
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Attributes.com
{
 
    [System.Serializable]
    public class ActorAttributesDatabaseCSV
    {
        public string DatabaseName;
        public int DatabaseID;
        public ActorAttributesCSV[] Entries = new ActorAttributesCSV[0];
        public ActorAttributesDatabaseCSV(string dbName, int dbID, ActorAttributesCSV[] _entries)
        {
            DatabaseName = dbName;
            DatabaseID = dbID;
            Entries = _entries;
        }
    }

    [System.Serializable]
    public class AttributeCSVValue
    {
        public string AttributeCategoryName;
        public int AttributeCategoryType;
        public string AttributeName;
        public int AttributeSubType;
        public int StartingValue;
        public int MaxValue;

        public AttributeCSVValue(string attributecatname, int attributecattype, string attributename, int attributeSub, int starting, int max)
        {
            AttributeCategoryName = attributecatname;
            AttributeCategoryType = attributecattype;
            AttributeName = attributename;
            StartingValue = starting;
            AttributeSubType = attributeSub;
            MaxValue = max;
        }
    }

    [System.Serializable]
    public class ActorAttributesCSV
    {
        public int Entry;
        public string Name;
        public AttributeCSVValue[] AttributeValues;

    }

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_ActorAttributes")]

    public class ActorAttributesDatabase : ScriptableObject, IDatabase
    {
        public AttributesDatabaseSlot[] Slots => slots;
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        AttributesDatabaseSlot[] slots = new AttributesDatabaseSlot[0];
        readonly string saveFolder = "Assets/GWLPXL/ARPG/Data/ActorAttributes";
        readonly int rowsize = 6;
        public DatabaseID GetDatabaseEntry() => DatabaseID.Attributes;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;
        public AttributesDatabaseSlot[] GetSlots() => slots;
        public string GetSaveFolder() => saveFolder;
        public int GetWindowRowSize() => rowsize;
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
            return slots[slotIndex].ActorStats;
        }

  
       
        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(AttributesDatabaseSlot[] newSlots) => slots = newSlots;
      
    
        public int FindIDByActorStats(ActorAttributes forStats)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (forStats == slots[i].ActorStats)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }
        public ActorAttributes FindActorStatsByID(int itemID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (itemID == slots[i].ID.ID)
                {
                    return slots[i].ActorStats;
                }
            }
            return null;
        }

        public Object GetMyObject() => this;
      
    }

    #region helper classes
    [System.Serializable]
    public class AttributesID
    {
        public string Name;
        public int ID;
        public ActorAttributes ActorStats;
        public AttributesID(string name, int id, ActorAttributes stats)
        {
            Name = name;
            ID = id;
            ActorStats = stats;
        }
    }

    [System.Serializable]
    public class AttributesDatabaseSlot
    {
        public string DescriptiveName;
        public AttributesID ID;
        public ActorAttributes ActorStats;
        public AttributesDatabaseSlot(AttributesID id, ActorAttributes stats)
        {
           
            ID = id;
            ActorStats = stats;
            DescriptiveName = stats.ActorName;
            stats.SetStatsID(ID);
        }
    }

    #endregion

}
