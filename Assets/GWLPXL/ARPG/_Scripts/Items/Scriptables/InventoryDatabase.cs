
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Inventory")]

    public class InventoryDatabase : ScriptableObject, IDatabase
    {
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        InventoryDatabaseSlot[] slots = new InventoryDatabaseSlot[0];
        readonly int rowsize = 6;
        public DatabaseID GetDatabaseEntry() => DatabaseID.Inventories;
        public InventoryDatabaseSlot[] GetSlots() => slots;
        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(InventoryDatabaseSlot[] newSlots) => slots = newSlots;

        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;

        public int FindIDByInv(ActorInventory invID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (invID == slots[i].ID.SOReference)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }


        public ActorInventory FindInvByID(int classID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (classID == slots[i].ID.ID)
                {
                    return slots[i].ActorInv;
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
            return slots[slotIndex].ActorInv;
        }

        public int GetWindowRowSize()
        {
            return rowsize;
        }

        public Object GetMyObject() => this;
     
    }

    [System.Serializable]
    public class InventoryID
    {
        public string Name;
        public int ID;
        public ActorInventory SOReference;

        public InventoryID(string name, int id, ActorInventory actorInv)
        {
            Name = name;
            ID = id;
            SOReference = actorInv;
        }

    }
    [System.Serializable]
    public class InventoryDatabaseSlot
    {
        public string DescriptiveName;
        public InventoryID ID;
        public ActorInventory ActorInv;
        public InventoryDatabaseSlot(InventoryID id, ActorInventory actorInv)
        {
            ID = id;
            ActorInv = actorInv;
            DescriptiveName = actorInv.name;
            actorInv.SetDatabaseID(id);
        }
    }
}