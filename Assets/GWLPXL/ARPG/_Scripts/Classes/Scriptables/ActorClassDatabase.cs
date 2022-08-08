
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Classes.com
{
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Classes")]

    public class ActorClassDatabase : ScriptableObject, IDatabase
    {
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        ClassDatabaseSlot[] slots = new ClassDatabaseSlot[0];
        readonly int rowsize = 6;
        public DatabaseID GetDatabaseEntry() => DatabaseID.Classes;

        public string[] GetSearchFolders() => searchFolders;
        public void SetSlots(ClassDatabaseSlot[] newSlots) => slots = newSlots;

        public ClassDatabaseSlot[] GetSlots() => slots;


        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;

        public int FindIDByClass(ActorClass classID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (classID == slots[i].ID.SOReference)
                {
                    return slots[i].ID.ID;
                }
            }
            return 0;
        }


        public ActorClass FindClassByID(int classID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (classID == slots[i].ID.ID)
                {
                    return slots[i].ActorClass as ActorClass;
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
            return slots[slotIndex].ActorClass;
        }

        public int GetWindowRowSize()
        {
            return rowsize;
        }

        public Object GetMyObject() => this;
      
    }
    [System.Serializable]
    public class ClassID
    {
        public string Name;
        public int ID;
        public ActorClass SOReference;

        public ClassID(string name, int id, ActorClass actorClass)
        {
            Name = name;
            ID = id;
            SOReference = actorClass;
        }

    }
    [System.Serializable]
    public class ClassDatabaseSlot
    {
        public string DescriptiveName;
        public ClassID ID;
        public ActorClass ActorClass;
        public ClassDatabaseSlot(ClassID id, ActorClass actorClass)
        {
            ID = id;
            ActorClass = actorClass;
            DescriptiveName = actorClass.GetClassName();
            actorClass.SetClassID(id);
        }
    }



}