
using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{


    public interface IQuestUser
    {
        QuestLog GetQuestLogTemplate();
        QuestLog GetQuestLogRuntime();
        void SetRuntimeQuestLog(QuestLog neLog);
        Transform GetMyInstance();
        void SetQuestGiver(IQuestGiver questGiver);
        void SetTemplate(QuestLog newTemplate);


    }
}