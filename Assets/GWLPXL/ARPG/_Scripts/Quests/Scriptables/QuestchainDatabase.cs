
using UnityEngine;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPGCore.Quests.com
{
    
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_QuestChains")]
    public class QuestchainDatabase : ScriptableObject, IDatabase
    {
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        QuestchainDdatabaseSlot[] slots = new QuestchainDdatabaseSlot[0];
        public DatabaseID GetDatabaseEntry() => DatabaseID.Questchains;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;
        public QuestchainDdatabaseSlot[] GetSlots() => slots;

        public string[] GetSearchFolders() => searchFolders;
        readonly int rowsize = 6;
       public void SetSlots(QuestchainDdatabaseSlot[] _slots)
        {
            slots = _slots;
        }

        public Questchain FindQuestByID(int ID)
        {

            for (int i = 0; i < slots.Length; i++)
            {
                if (ID == slots[i].ID.ID)
                {
                    return slots[i].Questchain as Questchain;
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
            return slots[slotIndex].Questchain;
        }

        public int GetWindowRowSize()
        {
            return rowsize;
        }

        public Object GetMyObject() => this;
     
    }

    [System.Serializable]
    public class QuestchainDdatabaseSlot
    {
        public string DescriptiveName;
        public QuestchainID ID;
        public Questchain Questchain;
        public QuestchainDdatabaseSlot(QuestchainID id, Questchain quest)
        {
            ID = id;
            Questchain = quest;
            DescriptiveName = quest.GetQuestName();
            Questchain.SetID(id);
         
        }
    }

    [System.Serializable]
    public class QuestchainID
    {
        public string Name;
        public int ID;
        public Questchain Questchain;

        public QuestchainID(string name, int id, Questchain quest)
        {
            Name = name;
            ID = id;
            Questchain = quest;
        }
    }

}