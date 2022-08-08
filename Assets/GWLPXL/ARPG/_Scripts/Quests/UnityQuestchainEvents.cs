using UnityEngine.Events;
using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{
    #region scene specific unity events

    [System.Serializable]
    public class UnityQuestchainEvent : UnityEvent<Questchain> { }

    [System.Serializable]
    public class UnityQuestEvent : UnityEvent<Quest> { }

    [System.Serializable]
    public class UnityQuestingEvents
    {
        public UnityQuestEvent OnQuestAvailable;
        public UnityQuestEvent OnQuestInProgress;
        public UnityQuestEvent OnQuestCompleted;
        public UnityQuestEvent OnQuestRewardsCollected;
        public UnityQuestEvent OnQuestAbandoned;
    }

    [System.Serializable]
    public class QuestGiverEvents
    {
        [Header("Open and Close")]
        public UnityEvent OnQuestDialogueOpen;
        public UnityEvent OnQuestDialogueClose;
        [Header("Questchain Status")]
        public UnityQuestchainEvent OnQuestchainStarted;
        public UnityQuestchainEvent OnQuestChainRewardsCollected;
        public UnityQuestchainEvent OnQuestChainAbandoned;

    }
    [System.Serializable]
    public class PlayerQuestEvents
    {
        //  public LevelUpEvent GameEvents;//on the actual attributes
        public UnityQuestingEvents SceneEvents = new UnityQuestingEvents();
    }

    #endregion
}