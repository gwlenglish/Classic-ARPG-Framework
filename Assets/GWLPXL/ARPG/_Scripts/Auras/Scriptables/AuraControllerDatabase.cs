

using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Auras.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Aura Controllers")]

    public class AuraControllerDatabase : ScriptableObject, IDatabase
    {
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        AuraControllerDatabaseSlot[] slots = new AuraControllerDatabaseSlot[0];
        readonly int rowsize = 6;
        public DatabaseID GetDatabaseEntry() => DatabaseID.AuraControllers;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;

        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(AuraControllerDatabaseSlot[] newSlots) => slots = newSlots;

        public AuraControllerDatabaseSlot[] GetSlots() => slots;


        public int GetIDByAuraController(AuraController forcontroller)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (forcontroller == slots[i].Aura)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }
        public AuraController FindAuraControllerByID(int skillID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (skillID == slots[i].ID.ID)
                {
                    return slots[i].ID.ControllerRef;
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
                jsons[i] = slots[i].Aura as ISaveJsonConfig;
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
    public class AuraControllerID
    {
        public string Name;
        public int ID;
        public AuraController AuraController;

        public AuraControllerID(string name, int id, AuraController auracontroller)
        {
            Name = name;
            ID = id;
            AuraController = auracontroller;
        }
    }

    [System.Serializable]
    public class AuraControllerDatabaseSlot
    {
        public string DescriptiveName;
        public AuraControllerData ID;
        public AuraController Aura;
        public AuraControllerDatabaseSlot(AuraControllerData id, AuraController aura)
        {
            ID = id;
            Aura = aura;
            DescriptiveName = aura.name;
            id.ControllerRef = Aura;
            aura.AuraControllerData = id;
        }
    }
}