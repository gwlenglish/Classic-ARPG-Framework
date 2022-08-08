
using UnityEngine;
using GWLPXL.ARPGCore.Quests.com;
namespace GWLPXL.ARPGCore.GameEvents.com
{
    /// <summary>
    /// delete, just use the event manager
    /// </summary>
    [CreateAssetMenu(menuName = "GWLPXL/ARPG/GameEvents/NEW_Explore Event")]
    public class ExploreEvent : GameEvent
    {
        public ExploreArea ExploreArea { get; set; }
        public ExploreQuest ExploreQuest { get; set; }
        public bool ExploredValue { get; set; }
        public IQuestUser QuestUser { get; set; }
    }
}