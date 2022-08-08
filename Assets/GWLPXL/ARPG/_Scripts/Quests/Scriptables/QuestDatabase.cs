
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Quests")]
    public class QuestDatabase : ScriptableObject, IDatabase
    {
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        QuestDdatabaseSlot[] slots = new QuestDdatabaseSlot[0];
        readonly int rowsize = 6;
        public DatabaseID GetDatabaseEntry() => DatabaseID.Quests;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;
        public QuestDdatabaseSlot[] GetSlots() => slots;

        public string[] GetSearchFolders() => searchFolders;
       public void SetSlots(QuestDdatabaseSlot[] _slots)
        {
            slots = _slots;
        }

        public Quest FindQuestByID(int ID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (ID == slots[i].ID.ID)
                {
                    return slots[i].Quest as Quest;
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
            return slots[slotIndex].Quest;
        }

        public int GetWindowRowSize()
        {
            return rowsize;
        }

        public Object GetMyObject() => this;
       
    }

    [System.Serializable]
    public class QuestDdatabaseSlot
    {
        public string DescriptiveName;
        public QuestID ID;
        public Quest Quest;
        public QuestDdatabaseSlot(QuestID id, Quest quest)
        {
            ID = id;
            Quest = quest;
            string questName = quest.GetQuestName();
            if (string.IsNullOrEmpty(questName))
            {
                questName = quest.name;
                id.Name = questName;
            }
            DescriptiveName = questName;
            quest.SetID(id);
        }
    }

    [System.Serializable]
    public class QuestID
    {
        public string Name;
        public int ID;
        public Quest Quest;

        public QuestID(string name, int id, Quest quest)
        {
            Name = name;
            ID = id;
            Quest = quest;
        }
    }

}