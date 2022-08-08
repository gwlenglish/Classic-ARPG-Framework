
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Saving.com;
using System.Text;

namespace GWLPXL.ARPGCore.Combat.com
{
 
    [System.Serializable]
    public class ProjectileDataDatabaseCSV
    {
        public string DatabaseName;
        public int DatabaseID;
        public ProjectileDataCSV[] Entries = new ProjectileDataCSV[0];
        public ProjectileDataDatabaseCSV(string dbName, int dbID, ProjectileDataCSV[] _entries)
        {
            DatabaseID = dbID;
            Entries = _entries;
        }
    }

    [System.Serializable]
    public class ProjectileDataCSV
    {
        public int Entry;
        public string Name;
        public ProjectileOptions Options;
    }






    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_ProjectileData")]

    public class ProjectileDataDatabase : ScriptableObject, IDatabase
    {
        public ProjectileDataDatabaseSlot[] Slots => slots;
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        ProjectileDataDatabaseSlot[] slots = new ProjectileDataDatabaseSlot[0];
        int windowRowSize = 6;
        StringBuilder sb = new StringBuilder();
        ProjectileDataDatabaseCSV csvEntries;
        #region window
        public DatabaseID GetDatabaseEntry() => DatabaseID.Projectiles;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;


        public int GetWindowRowSize() => windowRowSize;
        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(ProjectileDataDatabaseSlot[] newSlots) => slots = newSlots;
        public ProjectileDataDatabaseSlot[] GetSlots() => slots;
        public UnityEngine.Object GetDatabaseObjectBySlotIndex(int slotIndex)
        {
            if (slotIndex > slots.Length - 1 || slotIndex < 0) return null;
            return (Object)slots[slotIndex].Instance as UnityEngine.Object;
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
        public int GetSlotID(ProjectileData forInstance)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (forInstance == slots[i].ID.Instance)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }
        public ProjectileData FindInstanceByID(int skillID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (skillID == slots[i].ID.ID)
                {
                    return slots[i].ID.Instance;
                }
            }
            return null;
        }

        public Object GetMyObject() => this;

       

      





        #endregion

    }





    [System.Serializable]
    public class ProjectileDataID
    {
        public string Name;
        public int ID;
        public ProjectileData Instance;
        public ProjectileDataID(string name, int id, ProjectileData instance)
        {
            Name = name;
            ID = id;
            Instance = instance;
            
        }
    }

    [System.Serializable]
    public class ProjectileDataDatabaseSlot
    {
        public string DescriptiveName;
        public ProjectileDataID ID;
        public ProjectileData Instance;
        public ProjectileDataDatabaseSlot(ProjectileDataID id, ProjectileData instance)
        {
            ID = id;
            Instance = instance;
            id.Instance = instance;
            DescriptiveName = id.Instance.name;
          
            Instance.SetID(id);

        }
    }

}
