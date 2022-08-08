using System.Linq;
using GWLPXL.ARPGCore.Types.com;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Statics.com;
namespace GWLPXL.ARPGCore.Quests.com
{
   
    /// <summary>
    /// For anything that wants to track questchains and their current states, typically used for players. 
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/NEW_QuestLog")]
    public class QuestLog : ScriptableObject, ISaveJsonConfig
    {
        [System.NonSerialized]
        public System.Action<Questchain> OnQuestChainInProgress;
        [System.NonSerialized]
        public System.Action<Quest> OnQuestComplete;
        [System.NonSerialized]
        public System.Action<Quest> OnQuestInProgress;
        [System.NonSerialized]
        public System.Action<Quest> OnQuestAvailable;
        [System.NonSerialized]
        public System.Action<Quest> OnQuestAbandon;
        [System.NonSerialized]
        public System.Action<Quest> OnRewardsCollected;
        [System.NonSerialized]
        public System.Action OnQuestLogUpdated;

        [System.NonSerialized]
        public QuestTrackingStats QuestStats = new QuestTrackingStats();

        [SerializeField]
        TextAsset config = null;
        [SerializeField]
        QuestLogID logID = new QuestLogID(string.Empty, 0, null);

        [System.NonSerialized]
        Dictionary<Quest, Questchain> questandchaindic = new Dictionary<Quest, Questchain>();
        [System.NonSerialized]
        Dictionary<Quest, QuestState> questsDic = new Dictionary<Quest, QuestState>();//template, runtime
        [System.NonSerialized]
        Dictionary<Questchain, QuestState> questchainDic = new Dictionary<Questchain, QuestState>();
        [System.NonSerialized]
        Dictionary<Questchain, int> questInChainDic = new Dictionary<Questchain, int>();
        [System.NonSerialized]
        Dictionary<Questchain, bool> completedQuestChainDic = new Dictionary<Questchain, bool>();
        [System.NonSerialized]
        Dictionary<Questchain, bool> repetableDic = new Dictionary<Questchain, bool>();
        [System.NonSerialized]
        IQuestUser questerInstance = null;
        [System.NonSerialized]
        IQuestGiver questGiver = null;

        public QuestLogID GetID() => logID;
        public void SetID(QuestLogID newID) => logID = newID;
        public void SetQuestGiver(IQuestGiver giver) => questGiver = giver;
        public QuestState GetQuestchainState(Questchain template)
        {
            return GetRuntimeQuestChain(template);
        }
        public QuestStatusType GetQuestStatus(Quest template)
        {
            return GetRuntimeQuestState(template).State;
        }
        public void Ini(IQuestUser quester)
        {
            questerInstance = quester;
            OnQuestLogUpdated += DebugInvoke;
        }
        public Quest[] GetAllRuntimeQuests() => questsDic.Keys.ToArray();
        public Questchain[] GetAllRuntimeQuestChains() => questchainDic.Keys.ToArray();

        public Quest[] GetQuestsInStatus(QuestStatusType type)
        {
            List<Quest> temp = new List<Quest>();
            foreach (var kvp in questsDic)
            {
                QuestStatusType status = kvp.Value.State;
                if (status == type)
                {
                    temp.Add(kvp.Key);
                }
            }
            return temp.ToArray();
        }
        public Questchain[] GetQuestChainsInStatus(QuestStatusType type)
        {
            List<Questchain> temp = new List<Questchain>();
            foreach (var kvp in questchainDic)
            {
                QuestStatusType status = kvp.Value.State;
                if (status == type)
                {
                    temp.Add(kvp.Key);
                }
            }
            return temp.ToArray();
        }


        public QuestState GetRuntimeQuestState(Quest template)
        {
            if (template == null)
            {
                Debug.LogWarning("Somehow the template is null");
                return new QuestState(QuestStatusType.UnAvailable);
            }
            questsDic.TryGetValue(template, out QuestState value);
            if (value == null)
            {
                QuestState temp = new QuestState(QuestStatusType.UnAvailable);
                value = temp;
                questsDic[template] = value;

            }

            return value;
        }
        public bool GetQuestChainCompleted(Questchain template)
        {
            completedQuestChainDic.TryGetValue(template, out bool value);
            return value;
        }
        public int GetQuestIndexInChain(Questchain template)
        {
            questInChainDic.TryGetValue(template, out int value);
            return value;
        }
        public void SetNextQuestInChain(Questchain template, int newIndex)
        {
            questInChainDic.TryGetValue(template, out int value);
            if (newIndex > template.GetQuests().Length - 1)
            {
                newIndex = template.GetQuests().Length - 1;
                completedQuestChainDic.TryGetValue(template, out bool completed);
                completed = true;
                completedQuestChainDic[template] = completed;
                UpdateChain(template, QuestStatusType.Completed);
                return;
            }
            value = newIndex;
            questInChainDic[template] = value;
            UpdateQuest(template.GetQuests()[value], QuestStatusType.Available);

            QuestStartConditionType type = template.GetQuests()[value].GetStartType();
            switch (type)
            {
                case QuestStartConditionType.AutoStart:
                    QuestState chainstate = GetRuntimeQuestChain(template);
                    if (chainstate.State == QuestStatusType.InProgress)
                    {
                        UpdateQuest(template.GetQuests()[value], QuestStatusType.InProgress);
                    }
                    break;
            }
        }
      /// <summary>
      /// will only allow reseet of repeatable quests
      /// </summary>
      /// <param name="template"></param>
        public void ResetQuestChain(Questchain template)
        {
            repetableDic.TryGetValue(template, out bool value);
            if (value)
            {
                //allow quest chain reset
                Quest[] quests = template.GetQuests();
                for (int i = 0; i < quests.Length; i++)
                {
                    quests[i].ResetQuestTrackers(questerInstance);
                    UpdateQuest(quests[i], QuestStatusType.UnAvailable);
                }
                SetNextQuestInChain(template, 0);
                UpdateChain(template, QuestStatusType.UnAvailable);
            }
        }

        /// <summary>
        /// to be used for editor, mark quests repeatable that you want to repeat at runtime
        /// </summary>
        /// <param name="template"></param>
        public void ForceResetQuestChain(Questchain template)
        {
            //allow quest chain reset
            Quest[] quests = template.GetQuests();
            for (int i = 0; i < quests.Length; i++)
            {
                quests[i].ResetQuestTrackers(questerInstance);
                UpdateQuest(quests[i], QuestStatusType.UnAvailable);
            }
            SetNextQuestInChain(template, 0);
            UpdateChain(template, QuestStatusType.UnAvailable);

        }
        public QuestState GetRuntimeQuestChain(Questchain template)
        {
            questchainDic.TryGetValue(template, out QuestState value);
            if (value == null)
            {
                //Debug.Log("chain is null");
                value = new QuestState(QuestStatusType.UnAvailable);
                questchainDic[template] = value;
                for (int i = 0; i < template.GetQuests().Length; i++)
                {
                    QuestState state = GetRuntimeQuestState(template.GetQuests()[i]);
                    questandchaindic[template.GetQuests()[i]] = template;
                }
                repetableDic[template] = template.GetIsRepeatable();
                SetNextQuestInChain(template, 0);
            }
            return value;
        }
        /// <summary>
        /// used to load saves, bypasses the normal udpate checks
        /// </summary>
        /// <param name="template"></param>
        /// <param name="newstatus"></param>
        public void ForceUpdateChain(Questchain template, QuestStatusType newstatus)
        {
            QuestState value = GetRuntimeQuestChain(template);
            value = template.GetQuestChainState(newstatus);
            questchainDic[template] = value;

        }
        /// <summary>
        /// used to load saves, bypasses the normal update checks
        /// </summary>
        /// <param name="template"></param>
        /// <param name="newstatus"></param>
        public void ForceUpdateQuest(Quest template, QuestStatusType newstatus)
        {
            QuestState value = GetRuntimeQuestState(template);
            value = template.GetQuestState(newstatus);
            questsDic[template] = value;
        }
        public void UpdateChain(Questchain template, QuestStatusType newStatus)
        {
            QuestState value = GetRuntimeQuestChain(template);
            value = template.GetQuestChainState(newStatus);
            questchainDic[template] = value;
          
            switch (value.State)
            {
                case QuestStatusType.InProgress:
                    OnQuestChainInProgress?.Invoke(template);
                    break;
            }
            OnQuestLogUpdated?.Invoke();

        }
        public void UpdateQuest(Quest template, QuestStatusType newStatus)
        {
            QuestState value = GetRuntimeQuestState(template);
            if (newStatus == value.State)
            {
                // return;//nothing to update
                return;
            }

            QuestStatusType oldStatus = value.State;

            value = template.GetQuestState(newStatus);
            questsDic[template] = value;

            //probably need to do some things so we can prevent status changes from completed to not started and other ones that dont make sense.
            QuestRewardTurnInType turnin = template.GetTurnInType();
            switch (newStatus)
            {
                case QuestStatusType.UnAvailable:
                    OnQuestAbandon?.Invoke(template);
                    break;
                case QuestStatusType.Available:
                    OnQuestAvailable?.Invoke(template);
                    break;
                case QuestStatusType.InProgress:
                    template.UpdateQuestProgress(questerInstance);
                    OnQuestInProgress?.Invoke(template);
                    break;
                case QuestStatusType.Completed:
                    switch (turnin)
                    {
                        case QuestRewardTurnInType.OnComplete:
                            template.CollectRewards(questerInstance);
                            UpdateQuest(template, QuestStatusType.RewardsCollected);
                            break;
                    }
                    OnQuestComplete?.Invoke(template);

                    break;
                case QuestStatusType.RewardsCollected:
                    questandchaindic.TryGetValue(template, out Questchain chainValue);
                    if (chainValue != null)
                    {
                        SetNextQuestInChain(chainValue, GetQuestIndexInChain(chainValue) + 1);//auto progress
                    }
                    OnRewardsCollected?.Invoke(template);
                    template.ResetQuestTrackers(questerInstance);//clear trackers for the quest
                    break;
            }

            OnQuestLogUpdated?.Invoke();
        }

        void DebugInvoke()
        {
            ARPGCore.DebugHelpers.com.ARPGDebugger.DebugMessage("LOG INVOKED", this);
        }

        public void SetTextAsset(TextAsset textAsset) => config = textAsset;
      

        public TextAsset GetTextAsset() => config;
      

        public Object GetObject() => this;
       
    }


    #region helpers

    /// <summary>
    /// tracks the quest progress using runtime dictionaries. Anything we need to track can be added here.
    /// </summary>
    /// 
    
    public class QuestTrackingStats
    {
        [System.NonSerialized]
        public Dictionary<Quest, Dictionary<ActorAttributes, int>> KillQuestsTracker = new Dictionary<Quest, Dictionary<ActorAttributes, int>>();
        [System.NonSerialized]
        public Dictionary<Quest, Dictionary<ExploreArea, bool>> ExploreAreasTracker = new Dictionary<Quest, Dictionary<ExploreArea, bool>>();

        
        public QuestTrackingStats()
        {
            KillQuestsTracker = new Dictionary<Quest, Dictionary<ActorAttributes, int>>();
            ExploreAreasTracker = new Dictionary<Quest, Dictionary<ExploreArea, bool>>();

        }

    }

    #endregion
}