
using UnityEngine;
using System.Text;
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using System.Collections.Generic;
namespace GWLPXL.ARPGCore.Quests.com
{
   

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Logic/NEW Explore Quest")]
    public class ExploreQuest : QuestLogic
    {
        public ExploreQuestGoal[] Goals => goals;
        [Header("Explore Quest Unique")]
        [SerializeField]
        ExploreQuestGoal[] goals = new ExploreQuestGoal[0];
        [System.NonSerialized]
        StringBuilder sb = new StringBuilder();


        public override string GetProgressDescription()
        {
            return sb.ToString();
        }

     
        public override void CheckProgress(IQuestUser forUser, Quest forquest)
        {

            PlayerPersistant[] players = DungeonMaster.Instance.GetPlayerPersist();

            QuestLog runtime = forUser.GetQuestLogRuntime();
            for (int i = 0; i < goals.Length; i++)
            {
                goals[i].Discovered = false;
            }

            int found = 0;
            sb.Clear();
            for (int i = 0; i < players.Length; i++)
            {
                if (runtime == players[i].PersistantQuestLog)
                {
                    //found it
                    for (int j = 0; j < goals.Length; j++)
                    {
                        runtime.QuestStats.ExploreAreasTracker.TryGetValue(forquest, out Dictionary<ExploreArea, bool> value);
                        if (value == null)
                        {
                            value = new Dictionary<ExploreArea, bool>();
                        }
                        value.TryGetValue(goals[j].Area, out bool discovered);
                        if (discovered == true)
                        {
                            found += 1;
                        }
                        goals[j].Discovered = discovered;
                        //Debug.Log("Discovered " + discovered);

                        value[goals[j].Area] = discovered;
                        runtime.QuestStats.ExploreAreasTracker[forquest] = value;

                        sb.Append(goals[j].GetDescription() + "\n");
                    }
                    break;
                }
            }



            if (found >= goals.Length)
            {
                forquest.CompletedQuest(forUser);
            }

            forUser.GetQuestLogRuntime().OnQuestLogUpdated?.Invoke();


        }

        public override void ResetTrackers(IQuestUser forUser, Quest forQuest)
        {
            PlayerPersistant[] players = DungeonMaster.Instance.GetPlayerPersist();
            QuestLog runtime = forUser.GetQuestLogRuntime();
         

            for (int i = 0; i < players.Length; i++)
            {
                if (runtime == players[i].PersistantQuestLog)
                {
                    //found it
                    for (int j = 0; j < goals.Length; j++)
                    {
                        runtime.QuestStats.ExploreAreasTracker.TryGetValue(forQuest, out Dictionary<ExploreArea, bool> value);
                        if (value == null) continue;
                        value.TryGetValue(goals[j].Area, out bool discovered);
                        discovered = false;
                        goals[j].Discovered = discovered;
                        value[goals[j].Area] = discovered;
                        runtime.QuestStats.ExploreAreasTracker[forQuest] = value;

                    }
                    break;
                }
            }

        }
    }


    #region 

    public class ExploreEventVars
    {
        public ExploreArea Area;
        public IQuestUser ForQuester;
        public Quest Quest;

        public ExploreEventVars(ExploreArea area, IQuestUser user, Quest quest)
        {
            Area = area;
            ForQuester = user;
            Quest = quest;
        }
    }
    [System.Serializable]
    public class ExploreQuestGoal
    {
        public ExploreArea Area = null;
        [HideInInspector]
        public bool Discovered = false;
        public ExploreQuestGoal(ExploreArea area, bool discovered)
        {
            Area = area;
            Discovered = discovered;
        }

        public string GetDescription()
        {
            return Area.ExporeName + " Discovered " + Discovered.ToString();
        }
    }


    #endregion
}