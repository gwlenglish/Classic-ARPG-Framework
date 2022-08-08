

using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Auras")]

    public class AuraDatabase : ScriptableObject, IDatabase
    {
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        AuraDatabaseSlot[] slots = new AuraDatabaseSlot[0];
        readonly int rowsize = 6;
        public DatabaseID GetDatabaseEntry() => DatabaseID.Auras;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;
        public AuraDatabaseSlot[] GetSlots() => slots;

        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(AuraDatabaseSlot[] newSlots) => slots = newSlots;
      


        public int GetIDByAura(Aura forSkill)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (forSkill == slots[i].Aura)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }
        public Aura FindAuraByID(int skillID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (skillID == slots[i].ID.ID)
                {
                    return slots[i].Aura as Aura;
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
            return slots[slotIndex].Aura;
        }

        public int GetWindowRowSize() => rowsize;

        public Object GetMyObject() => this;
        
    }

    [System.Serializable]
    public class AuraID
    {
        public string Name;
        public int ID;
        public Aura Aura;

        public AuraID(string name, int id, Aura aura)
        {
            Name = name;
            ID = id;
            Aura = aura;
        }
    }

    [System.Serializable]
    public class AuraDatabaseSlot
    {
        public string DescriptiveName;
        public AuraID ID;
        public Aura Aura;
        public AuraDatabaseSlot(AuraID id, Aura aura)
        {
            ID = id;
            Aura = aura;
            DescriptiveName = aura.name;
            id.Aura = Aura;
            aura.SetID(id);
        }
    }
}