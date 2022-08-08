using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using GWLPXL.ARPGCore.Statics.com;

using System.Text;
using GWLPXL.ARPGCore.Attributes.com;
namespace GWLPXL.ARPGCore.Quests.com
{

    /// <summary>
    /// Base class that defines a new quest. 
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/NEW Quest")]
    public class Quest : ScriptableObject, ISaveJsonConfig
    {
        [System.NonSerialized]
        public System.Action<IQuestUser> OnQuestRewards;
        public QuestLogic Logic => logic;
        [SerializeField]
        protected TextAsset config = null;
        [SerializeField]
        protected QuestID ID;
        [SerializeField]
        protected string questName;

        [Header("Start Options")]
        [SerializeField]
        [Tooltip("Auto is best for chained quests.")]
        protected QuestStartConditionType startType = QuestStartConditionType.AutoStart;
        [SerializeField]
        protected ActorAttributes[] questStartTemplates = new ActorAttributes[0];
        [Header("State Options")]
        [SerializeField]
        protected QuestState[] questStates = new QuestState[4] { new QuestState(QuestStatusType.Available), 
            new QuestState(QuestStatusType.InProgress), 
            new QuestState(QuestStatusType.Completed), 
            new QuestState(QuestStatusType.RewardsCollected) };

        [Header("Reward Options")]
        [SerializeField]
        protected QuestReward[] QuestRewards = new QuestReward[0];
        [SerializeField]
        protected QuestRewardTurnInType rewardType = QuestRewardTurnInType.SpecificActor;
        [SerializeField]
        protected ActorAttributes[] rewardGiverTemplate = new ActorAttributes[0];

        [Header("Quest Goal")]
        [SerializeField]
        protected QuestLogic logic = null;

        [System.NonSerialized]
        protected QuestStatusType status = QuestStatusType.Available;
        [System.NonSerialized]
        protected StringBuilder sb = new StringBuilder();
        [System.NonSerialized]
        protected Questchain questChain = null;

        public virtual QuestStatusType GetStatus() => status;

        public virtual void SetStatus(QuestStatusType newStatus) => status = newStatus;

        public virtual void SetID(QuestID newID) => ID = newID;

        public virtual QuestID GetID() => ID;

        public virtual string GetQuestName() => questName;
        public QuestStartConditionType GetStartType() => startType;

        public virtual QuestRewardTurnInType GetTurnInType() => rewardType;

        public virtual void SetQuestChain(Questchain myChain) => questChain = myChain;

        public virtual void ResetQuestTrackers(IQuestUser forUser)
        {
            logic.ResetTrackers(forUser, this);
        }
        public virtual QuestState GetQuestState(QuestStatusType state)
        {
            QuestState copy = new QuestState(state);
            for (int i = 0; i < questStates.Length; i++)
            {
                if (state == questStates[i].State)
                {
                    copy = questStates[i];
                    copy.QuestTemplate = this;
                    break;
                }
            }
            return copy;
            
        }
        public virtual string GetProgressDescription()
        {
            return logic.GetProgressDescription();
        }

        public virtual string GetQuestGiverText(QuestStatusType atStatus, IAttributeUser attrUser)
        {
            sb.Clear();
            ActorAttributes template = attrUser.GetAttributeTemplate();
            bool found = false;
            for (int i = 0; i < questStates.Length; i++)
            {
                if (questStates[i].State == atStatus)
                {
                    for (int j = 0; j < questStates[i].Lines.Length; j++)
                    {
                        if (template == questStates[i].Lines[j].Actor)
                        {
                            sb.Append(questStates[i].Lines[j].QuestGiverText);
                            found = true;
                            break;
                        }
                    }
                }
            }

            if (found == false)
            {
                sb.Append("LINE FOR ACTOR NOT FOUND");
            }

            return sb.ToString();
        }
        public virtual string GetQuestJournalEntry(QuestStatusType atType)
        {
            if (questStates == null || questStates.Length == 0)
            {
                return "NULL, NO LOG ENTRY FOUND AT TYPE " + atType.ToString();

            }
            for (int i = 0; i < questStates.Length; i++)
            {
                if (questStates[i].State == atType)
                {
                    return questStates[i].QuestLogEntry;
                }
            }
            return "NULL, NO LOG ENTRY FOUND AT TYPE " + atType.ToString();
        }
        /// <summary>
        /// to actor can be null if set to automatic
        /// </summary>
        /// <param name="toActor"></param>
        /// <returns></returns>
        /// 
        public virtual bool CanStart(IAttributeUser toActor)
        {
            switch (startType)
            {
                case QuestStartConditionType.SpecificActor:
                    ActorAttributes template = toActor.GetAttributeTemplate();
                    for (int i = 0; i < rewardGiverTemplate.Length; i++)
                    {
                        if (template == rewardGiverTemplate[i])
                        {
                            return true;
                        }
                    }
                    break;
                case QuestStartConditionType.AutoStart:
                    return true;
                case QuestStartConditionType.AnyQuestGiver:
                    if (toActor != null)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        public virtual bool CanTurnIn(IAttributeUser toActor)
        {
            switch (rewardType)
            {
                case QuestRewardTurnInType.SpecificActor:
                    ActorAttributes template = toActor.GetAttributeTemplate();
                    for (int i = 0; i < rewardGiverTemplate.Length; i++)
                    {
                        if (template == rewardGiverTemplate[i])
                        {
                            return true;
                        }
                    }
                    break;
                case QuestRewardTurnInType.OnComplete:
                    if (GetStatus() == QuestStatusType.Completed)
                    {
                        return true;
                    }
                    break;
                case QuestRewardTurnInType.AnyQuestGiver:
                    if (toActor != null)
                    {
                        IQuestGiver questGiver = toActor.GetInstance().GetComponent<IQuestGiver>();
                        if (questGiver != null)
                        {
                            return true;
                        }
                    }
                    break;
            }
           
            return false;
        }
        public virtual void UpdateQuestProgress(IQuestUser forUser)
        {
            logic.CheckProgress(forUser, this);
        }
        public virtual void StartQuest(IQuestUser forUser)
        {
            forUser.GetQuestLogRuntime().UpdateQuest(this, QuestStatusType.InProgress);
           
        }
        public virtual void AbandonQuest(IQuestUser forUser)
        {
            forUser.GetQuestLogRuntime().UpdateQuest(this, QuestStatusType.UnAvailable);

        }
        public virtual void CompletedQuest(IQuestUser forUser)
        {
            forUser.GetQuestLogRuntime().UpdateQuest(this, QuestStatusType.Completed);

        
           
        }
        public virtual void CollectRewards(IQuestUser forUser)
        {
            for (int i = 0; i < QuestRewards.Length; i++)
            {
                QuestRewards[i].GiveRewards(forUser);
            }
 

        }


        #region json interface

        public void SetTextAsset(TextAsset textAsset)
        {
            config = textAsset;
        }

        public TextAsset GetTextAsset()
        {
            return config;
        }

        public Object GetObject()
        {
            return this;
        }
        #endregion

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            //renames quest lines to match the actor name in the attributes
            for (int i = 0; i < questStates.Length; i++)
            {
                for (int j = 0; j < questStates[i].Lines.Length; j++)
                {
                    if (questStates[i].Lines[j].Actor == null) continue;
                    if (questStates[i].Lines[j].Actor.ActorName == questStates[i].Lines[j].DescriptiveName) continue;
                    questStates[i].Lines[j].DescriptiveName = questStates[i].Lines[j].Actor.ActorName;//rename it to match actor
                }
            }
        }

#endif
    }


    #region helpers
    [System.Serializable]
    public class QuestLine
    {
        public string DescriptiveName = string.Empty;
        [Tooltip("Used as a key to determine who says these lines.")]
        public ActorAttributes Actor = null;
        [TextArea(3, 5)]
        public string QuestGiverText = "Hi, here is a quest!";

    }

    [System.Serializable]
    public class QuestState
    {
        public string StateName;
        [HideInInspector]
        public Quest QuestTemplate = null;
        public QuestStatusType State;
        public QuestLine[] Lines = new QuestLine[0];
        [TextArea(2, 3)]
        public string QuestLogEntry = "Objective: Find a Unicorn.";

        public QuestState(QuestStatusType state)
        {
            State = state;
            StateName = state.ToString();
        }

    }

    #endregion

}