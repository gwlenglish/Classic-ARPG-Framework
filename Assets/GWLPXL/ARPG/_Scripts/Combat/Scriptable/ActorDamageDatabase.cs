
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Saving.com;
using System.Text;

namespace GWLPXL.ARPGCore.Combat.com
{
 
    [System.Serializable]
    public class ActorDamageDatabaseCSV
    {
        public string DatabaseName;
        public int DatabaseID;
        public ActorDamageCSV[] Entries = new ActorDamageCSV[0];
        public ActorDamageDatabaseCSV(string dbName, int dbID, ActorDamageCSV[] _entries)
        {
            DatabaseID = dbID;
            Entries = _entries;
        }
    }

    [System.Serializable]
    public class ActorDamageCSV
    {
        public int Entry;
        public string Name;
        public DamageOptions DamageOptions;
        public DamageMultiplers_Actor DamageMultiplers;
        public StatusOverTimeOptions SoTOptions;
        public DamageOverTimeMultipliers SoTMultipliers;
    }






    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_ActorDamageDealers")]

    public class ActorDamageDatabase : ScriptableObject, IDatabase
    {
        public ActorDamageDatabaseSlot[] Slots => slots;
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        ActorDamageDatabaseSlot[] slots = new ActorDamageDatabaseSlot[0];
        int windowRowSize = 6;
        StringBuilder sb = new StringBuilder();
        ActorDamageDatabaseCSV csvEntries;
        #region window
        public DatabaseID GetDatabaseEntry() => DatabaseID.ActorDamageDealers;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;

        public int GetWindowRowSize() => windowRowSize;
        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(ActorDamageDatabaseSlot[] newSlots) => slots = newSlots;
        public ActorDamageDatabaseSlot[] GetSlots() => slots;
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
        public int GetSlotID(ActorDamageData forInstance)
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
        public ActorDamageData FindInstanceByID(int skillID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (skillID == slots[i].ID.ID)
                {
                    return slots[i].ID.Instance as ActorDamageData;
                }
            }
            return null;
        }

        public Object GetMyObject() => this;

       

      





        #endregion

    }





    [System.Serializable]
    public class ActorDamageID
    {
        public string Name;
        public int ID;
        public ActorDamageData Instance;
        public ActorDamageID(string name, int id, ActorDamageData instance)
        {
            Name = name;
            ID = id;
            Instance = instance;
            
        }
    }

    [System.Serializable]
    public class ActorDamageDatabaseSlot
    {
        public string DescriptiveName;
        public ActorDamageID ID;
        public ActorDamageData Instance;
        public ActorDamageDatabaseSlot(ActorDamageID id, ActorDamageData instance)
        {
            ID = id;
            Instance = instance;
            id.Instance = instance;
            DescriptiveName = id.Instance.name;
          
            Instance.SetID(id);

        }
    }

}
