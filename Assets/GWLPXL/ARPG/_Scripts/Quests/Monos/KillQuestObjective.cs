
using UnityEngine;
using GWLPXL.ARPGCore.GameEvents.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Quests.com
{
    /// <summary>
    /// Tracks kill for questers.
    /// </summary>

    public class KillQuestObjective : MonoBehaviour, IKillTracked
    {

        [SerializeField]
        protected Quest[] myQuests = null;
        [SerializeField]
        [Tooltip("Only count the kills if the player has the quest in progress = true. Count always = false.")]
        protected bool onlyRecordIfQuestInProgress = false;
        protected IActorHub hub;
        


        public virtual void UpdateQuest(IQuestUser forUser)
        {
            Debug.Log("Update quest called");
            if (forUser == null) return;

            for (int i = 0; i < myQuests.Length; i++)
            {
                if (onlyRecordIfQuestInProgress)
                {
                    QuestState state = forUser.GetQuestLogRuntime().GetRuntimeQuestState(myQuests[i]);
                    if (state.State == Types.com.QuestStatusType.InProgress)
                    {
                        EventManager.Instance.EnemyDeathEvent(hub.MyStats, myQuests[i], forUser);



                    }

                }
                else
                {
                    EventManager.Instance.EnemyDeathEvent(hub.MyStats, myQuests[i], forUser);


                }
            }

        }

        public virtual void SetActorHub(IActorHub hub) => this.hub = hub;
       
    }
}