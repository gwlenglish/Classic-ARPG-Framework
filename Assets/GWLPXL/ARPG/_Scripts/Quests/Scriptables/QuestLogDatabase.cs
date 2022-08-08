
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Database/NEW_Quest Logs")]
    public class QuestLogDatabase : ScriptableObject, IDatabase
    {
        [SerializeField]
        string[] searchFolders = new string[1] { "Assets/GWLPXL/ARPG" };
        [SerializeField]
        QuestLogdatabaseSlot[] slots = new QuestLogdatabaseSlot[0];
        readonly int rowsize = 6;
        public DatabaseID GetDatabaseEntry() => DatabaseID.QuestLogs;
        public void SetSearchFolders(string[] newfolders) => searchFolders = newfolders;
        public QuestLogdatabaseSlot[] GetSlots() => slots;

        public string[] GetSearchFolders() => searchFolders;
       public void SetSlots(QuestLogdatabaseSlot[] _slots)
        {
            slots = _slots;
        }

        public QuestLog FindQuestLogByID(int ID)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (ID == slots[i].ID.ID)
                {
                    return slots[i].Quest as QuestLog;
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
    public class QuestLogdatabaseSlot
    {
        public string DescriptiveName;
        public QuestLogID ID;
        public QuestLog Quest;
        public QuestLogdatabaseSlot(QuestLogID id, QuestLog questlog)
        {
            ID = id;
            Quest = questlog;
            string questName = questlog.GetID().Name;
            if (string.IsNullOrEmpty(questName))
            {
                questName = questlog.name;
                id.Name = questName;
            }
            id.QuestLog = questlog;
            DescriptiveName = questName;
            questlog.SetID(id);
        }
    }

    [System.Serializable]
    public class QuestLogID
    {
        public string Name;
        public int ID;
        public QuestLog QuestLog;

        public QuestLogID(string name, int id, QuestLog quest)
        {
            Name = name;
            ID = id;
            QuestLog = quest;
        }
    }

}