
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.Statics.com
{

    /// <summary>
    /// rethink this static class, maybe now we can use it. 
    /// </summary>
    public class QuestHandler 
    {
      
        public static void CollectRewards(IQuestUser forUser, Questchain fromQuestchain)
        {
          

        }
        public static void CollectRewards(IQuestUser forUser, Quest fromQuest)
        {
            forUser.GetQuestLogRuntime().UpdateQuest(fromQuest, QuestStatusType.RewardsCollected);

        }
        public static void StartQuestchain(IQuestUser forUser, Questchain chain)
        {
            forUser.GetQuestLogRuntime().UpdateChain(chain, QuestStatusType.InProgress);
        }
        public static void StartQuest(IQuestUser forUser, Quest quest)
        {
            forUser.GetQuestLogRuntime().UpdateQuest(quest, QuestStatusType.InProgress);
        }
        public static void AbandonQuest(IQuestUser forUser, Quest quest)
        {
            forUser.GetQuestLogRuntime().UpdateQuest(quest, QuestStatusType.Available);
        }

        public static void QuestObjective(Quest forQuest, bool onlyRecordIfQUestInProgress)
        {
            if (forQuest == null) return;
            PlayerSceneReference[] players = DungeonMaster.Instance.GetAllSceneReferences();
            for (int i = 0; i < players.Length; i++)
            {
                IQuestUser questUser = players[i].SceneRef.GetComponent<IQuestUser>();
                if (questUser == null) continue;
                if (onlyRecordIfQUestInProgress)
                {
                    if (questUser.GetQuestLogRuntime().GetRuntimeQuestState(forQuest).State == QuestStatusType.InProgress)
                    {
                        forQuest.UpdateQuestProgress(questUser);
                    }
                }
                else
                {
                    forQuest.UpdateQuestProgress(questUser);

                }


            }
        }
    }
}