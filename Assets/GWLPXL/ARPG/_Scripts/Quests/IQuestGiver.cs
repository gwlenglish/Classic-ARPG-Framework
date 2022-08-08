using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{
    public interface IQuestGiver
    {
        void StartQuest();
        void DeclineQuest();
        void AbandonQuest();
        void CollectRewards();
        void Return();
        void IncrementQuests();
        void DecrementQuests();
        void ResetQuestChain();
        Questchain GetCurrentQuestchain();
        Transform GetInstance();
    }
}