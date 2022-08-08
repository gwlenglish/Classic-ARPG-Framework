
using UnityEngine;
using GWLPXL.ARPGCore.Attributes.com;
using System.Text;
using System.Collections.Generic;
namespace GWLPXL.ARPGCore.Quests.com
{
    [System.Serializable]
    public class KillQuestGoals
    {
        public ActorAttributes GoalTemplate = null;
        public int KillAmount = 1;
        [HideInInspector]
        public int Current = 0;
        [HideInInspector]
        public bool Complete = false;
        public KillQuestGoals(ActorAttributes template, int amount)
        {
            GoalTemplate = template;
            KillAmount = amount;
        }

        public string GetProgressDescription()
        {
            return GoalTemplate.ActorName + " " + Current.ToString() + "/" + KillAmount.ToString();
        }
            

    }
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Logic/NEW Kill Quest")]
    public class KillQuest : QuestLogic
    {

        [Header("Kill Quest Unique")]
        [SerializeField]
        KillQuestGoals[] goals = new KillQuestGoals[0];
        [System.NonSerialized]
        StringBuilder sb = new StringBuilder();
        /// <summary>
         /// adds one to the kill
         /// </summary>
         /// <param name="template"></param>
      /// <param name="forUser"></param>
       

       

        public override string GetProgressDescription()
        {
            
            return sb.ToString();
        }

        public override void CheckProgress(IQuestUser forUser, Quest forQuest)
        {
            int complete = CheckComplete(forUser, forQuest);

            forUser.GetQuestLogRuntime().OnQuestLogUpdated?.Invoke();

            if (complete != -1) return;//not complete

            forQuest.CompletedQuest(forUser);


        }

        private int CheckComplete(IQuestUser forUser, Quest forQuest)
        {
            for (int i = 0; i < goals.Length; i++)
            {
                goals[i].Current = 0;
                goals[i].Complete = false;
            }

            QuestLog logruntime = forUser.GetQuestLogRuntime();
            sb.Clear();
           

            for (int i = 0; i < goals.Length; i++)
            {
                ActorAttributes enemyatt = goals[i].GoalTemplate;
                logruntime.QuestStats.KillQuestsTracker.TryGetValue(forQuest, out Dictionary<ActorAttributes, int> value);
                if (value == null)
                {
                    value = new Dictionary<ActorAttributes, int>();
                }
                value.TryGetValue(enemyatt, out int killValue);
                goals[i].Current = killValue;
               if (killValue >= goals[i].KillAmount)
                {
                    goals[i].Complete = true;
                }
                else
                {
                    goals[i].Complete = false;
                }


                sb.Append(goals[i].GetProgressDescription() + "\n");
            }

            int complete = -1;
            for (int i = 0; i < goals.Length; i++)
            {
                if (goals[i].Complete == false)
                {
                    complete = i;
                    break;
                }
            }
            return complete;
        }

        public override void ResetTrackers(IQuestUser forUser, Quest forQuest)
        {
            QuestLog logruntime = forUser.GetQuestLogRuntime();
            for (int i = 0; i < goals.Length; i++)
            {
                ActorAttributes enemyatt = goals[i].GoalTemplate;
                logruntime.QuestStats.KillQuestsTracker.TryGetValue(forQuest, out Dictionary<ActorAttributes, int> value);
                if (value == null) continue;

                value.TryGetValue(enemyatt, out int killValue);
                killValue = 0;
                value[enemyatt] = killValue;
                logruntime.QuestStats.KillQuestsTracker[forQuest] = value;

            }
        }
    }
}