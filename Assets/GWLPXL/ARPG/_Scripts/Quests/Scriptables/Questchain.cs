using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Types.com;
using System.Text;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Quests.com
{
    
    /// <summary>
    ///
    /// Basic class for Quest Chains. Questchains should be the containers for Quests.
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/NEW Questchain")]

    public class Questchain : ScriptableObject, ISaveJsonConfig
    {
        [SerializeField]
        TextAsset config = null;
        [SerializeField]
        QuestchainID ID = default;
        [SerializeField]
        string questChainName = string.Empty;
        [SerializeField]
        bool isRepeatable = false;
        [Header("Start Options")]
        [SerializeField]
        QuestStartConditionType StartType = QuestStartConditionType.SpecificActor;
        [SerializeField]
        ActorAttributes[] questStartTemplates = new ActorAttributes[0];
        [SerializeField]
        QuestChainRequirement[] requirements = new QuestChainRequirement[0];
        [Header("State Options")]
        [SerializeField]
        QuestState[] states = new QuestState[4]
        {
            new QuestState(QuestStatusType.UnAvailable),
            new QuestState(QuestStatusType.Available),
            new QuestState(QuestStatusType.Completed),
            new QuestState(QuestStatusType.RewardsCollected)
        };
        [Header("Quests")]
        [Tooltip("The order in the array is the order they sequence.")]
        [SerializeField]
        Quest[] quests = new Quest[0];
        [Header("Reward Options")]
        [SerializeField]
        QuestRewardTurnInType rewardType = QuestRewardTurnInType.SpecificActor;
        [SerializeField]
        ActorAttributes[] questGiverTemplates = new ActorAttributes[0];
        [SerializeField]
        QuestReward[] chainRewards = new QuestReward[0];
        [System.NonSerialized]
        QuestStatusType status = QuestStatusType.Available;
        [System.NonSerialized]
        int index = 0;
        [System.NonSerialized]
        StringBuilder sb = new StringBuilder();

        public string GetQuestName() => questChainName;
        public QuestchainID GetID() => ID;
        public void SetID(QuestchainID newID) => ID = newID;
        public bool GetIsRepeatable() => isRepeatable;
        public Quest[] GetQuests() => quests;
        public QuestStatusType GetStatus() => status;
        public void SetStatus(QuestStatusType newStatus) => status = newStatus;
        public Quest GetCurrentQuestInChain() => quests[index];
       
        public string GetFullProgressDescription()
        {
            sb.Clear();
            for (int i = 0; i < quests.Length; i++)
            {
                sb.Append(quests[i].GetProgressDescription());
                sb.Append("/n|");
            }
            return sb.ToString();
        }
        public string GetRequirementDescription()
        {
            sb.Clear();
            for (int i = 0; i < requirements.Length; i++)
            {
                sb.Append(requirements[i].GetDescription() + "\n");
            }
            return sb.ToString();
        }

        public bool CanStartQuestChain(IActorHub forUser, IAttributeUser questkey)
        {
            bool can = true;
            switch (StartType)
            {
                case QuestStartConditionType.AnyQuestGiver:
                    if (questkey != null)
                    {
                        IQuestGiver questGiver = questkey.GetInstance().GetComponent<IQuestGiver>();
                        if (questGiver != null)
                        {
                            can = true;
                            break;
                        }
                    }
                    break;
                case QuestStartConditionType.SpecificActor:
                    ActorAttributes template = questkey.GetAttributeTemplate();
                    for (int i = 0; i < questGiverTemplates.Length; i++)
                    {
                        if (template == questGiverTemplates[i])
                        {
                            can = true;
                            break;
                        }
                    }
                    break;
            }

            for (int i = 0; i < requirements.Length; i++)
            {
                can = requirements[i].MeetsRequirement(forUser, questkey.GetInstance().GetComponent<IQuestGiver>());
                if (can == false)
                {
                    break;
                }
               
            }
            return can;
        }
        public QuestState GetQuestChainState(QuestStatusType atType)
        {
            for (int i = 0; i < quests.Length; i++)
            {
                quests[i].SetQuestChain(this);
            }

            QuestState copy = new QuestState(atType);
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].State == atType)
                {
                    copy = states[i];
                    
                }
            }
            return copy;
        }
        public bool CanTurnIn(IAttributeUser toActor)
        {
            switch (rewardType)
            {
                case QuestRewardTurnInType.SpecificActor:
                    ActorAttributes template = toActor.GetAttributeTemplate();
                    for (int i = 0; i < questGiverTemplates.Length; i++)
                    {
                        if (template == questGiverTemplates[i])
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
       
        public string GetQuestGiverText(QuestStatusType atStatus, IAttributeUser attrUser)
        {
            sb.Clear();
            ActorAttributes template = attrUser.GetAttributeTemplate();
            bool found = false;
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].State == atStatus)
                {
                    for (int j = 0; j < states[i].Lines.Length; j++)
                    {
                        if (template == states[i].Lines[j].Actor)
                        {
                            sb.Append(states[i].Lines[j].QuestGiverText);
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

  
     

       
        public void CollectChainRewards(IQuestUser forUser)
        {
            if (GetStatus() == QuestStatusType.RewardsCollected) return;//already got them. 
            SetStatus(QuestStatusType.RewardsCollected);
            for (int i = 0; i < chainRewards.Length; i++)
            {
                chainRewards[i].GiveRewards(forUser);
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
        private void OnValidate()
        {
            //renames quest lines to match the actor name in the attributes
            for (int i = 0; i < states.Length; i++)
            {
                for (int j = 0; j < states[i].Lines.Length; j++)
                {
                    if (states[i].Lines[j].Actor == null) continue;
                    if (states[i].Lines[j].Actor.ActorName == states[i].Lines[j].DescriptiveName) continue;
                    states[i].Lines[j].DescriptiveName = states[i].Lines[j].Actor.ActorName;//rename it to match actor
                }
            }
        }

#endif
    }
}