
using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{


    public abstract class QuestLogic : ScriptableObject
    {
        /// <summary>
        /// Returns a string description of the current progress towards the goal. 
        /// </summary>
        /// <returns></returns>
        public abstract string GetProgressDescription();
        /// <summary>
        /// Checks the quest progress and should mark complete if the goals have been met. 
        /// </summary>
        /// <param name="forUser"></param>
        /// <param name="forQuest"></param>
        public abstract void CheckProgress(IQuestUser forUser, Quest forQuest);
        /// <summary>
        /// Since quests are temporary trackers, we should reset their tracking values. 
        /// </summary>
        /// <param name="forUser"></param>
        /// <param name="forQuest"></param>
        public abstract void ResetTrackers(IQuestUser forUser, Quest forQuest);
       

    }
}