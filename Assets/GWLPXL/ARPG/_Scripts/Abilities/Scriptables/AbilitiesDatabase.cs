
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Saving.com;
using System.Text;

namespace GWLPXL.ARPGCore.Abilities.com
{
 
    [System.Serializable]
    public class AbilityDatabaseCSV
    {
        public string DatabaseName;
        public int DatabaseID;
        public AbilityCSV[] Entries = new AbilityCSV[0];
        public AbilityDatabaseCSV(string dbName, int dbID, AbilityCSV[] _entries)
        {
            DatabaseID = dbID;
            Entries = _entries;
        }
    }

    [System.Serializable]
    public class AbilityCSV
    {
        public int Entry;
        public string Name;
        public string Description;
        public float DamageMultipler;
        public float Range;
        public float ResourceCost;
        public string ResourceType;
    }






    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Abilities")]

    public class AbilitiesDatabase : ScriptableObject, IDatabase
    {
        public AbilityDatabaseSlot[] Slots => slots;
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        AbilityDatabaseSlot[] slots = new AbilityDatabaseSlot[0];
        int windowRowSize = 6;
        StringBuilder sb = new StringBuilder();
        AbilityDatabaseCSV csvEntries;
        #region window
        public DatabaseID GetDatabaseEntry() => DatabaseID.Abilities;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;

        public int GetWindowRowSize() => windowRowSize;
        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(AbilityDatabaseSlot[] newSlots) => slots = newSlots;
        public AbilityDatabaseSlot[] GetSlots() => slots;
        public UnityEngine.Object GetDatabaseObjectBySlotIndex(int slotIndex)
        {
            if (slotIndex > slots.Length - 1 || slotIndex < 0) return null;
            return (Object)slots[slotIndex].Ability as UnityEngine.Object;
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
        public int GetAbilityID(Ability forAbility)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (forAbility == slots[i].ID.Ability)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }
        public Ability FindAbilityByID(int skillID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (skillID == slots[i].ID.ID)
                {
                    return slots[i].ID.Ability as Ability;
                }
            }
            return null;
        }

        public Object GetMyObject() => this;

       

      





        #endregion

    }





    [System.Serializable]
    public class AbilityID
    {
        public string Name;
        public int ID;
        public Ability Ability;

        public AbilityID(string name, int id, Ability ability)
        {
            Name = name;
            ID = id;
            Ability = ability;
        }
    }

    [System.Serializable]
    public class AbilityDatabaseSlot
    {
        public string DescriptiveName;
        public AbilityID ID;
        public Ability Ability;
        public AbilityDatabaseSlot(AbilityID id, Ability ability)
        {
            ID = id;
            Ability = ability;
            ID.Ability = ability;
            DescriptiveName = Ability.GetName();
            Ability.SetID(id);

        }
    }

}
