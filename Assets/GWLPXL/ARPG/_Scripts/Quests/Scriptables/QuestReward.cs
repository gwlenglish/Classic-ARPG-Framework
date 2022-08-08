using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{
    [System.Serializable]
    public abstract class QuestReward : ScriptableObject
    {
        /// <summary>
        /// Used to preview the rewards before receiving them. 
        /// </summary>
        /// <param name="giver"></param>
        public abstract void GetRewardPreview(IQuestGiver giver);
        /// <summary>
        /// Used to give the rewards to the quester. 
        /// </summary>
        /// <param name="toUser"></param>
        public abstract void GiveRewards(IQuestUser toUser);
    }
}