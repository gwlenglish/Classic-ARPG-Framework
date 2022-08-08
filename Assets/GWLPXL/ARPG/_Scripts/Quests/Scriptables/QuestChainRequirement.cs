using GWLPXL.ARPGCore.com;

using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{


    public abstract class QuestChainRequirement : ScriptableObject
    {
        public abstract bool MeetsRequirement(IActorHub forUser, IQuestGiver questGiver);
        public abstract string GetDescription();
    }
}