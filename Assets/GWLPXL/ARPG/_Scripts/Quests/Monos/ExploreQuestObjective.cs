using GWLPXL.ARPGCore.GameEvents.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{

    /// <summary>
    /// Trigger based system to tell when a quester has located a certain area/region.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    
    public class ExploreQuestObjective : MonoBehaviour
    {
        [SerializeField]
        protected Quest[] myQuests = null;
        [SerializeField]
        protected ExploreArea myArea = null;
        [SerializeField]
        protected bool onlyRecordIfActive = true;
        protected virtual void Awake()
        {
            //requirements of the trigger
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            IQuestUser questUser = other.GetComponentInParent<IQuestUser>();
            if (questUser == null) return;

            for (int i = 0; i < myQuests.Length; i++)
            {
                if (onlyRecordIfActive)
                {
                    QuestState state = questUser.GetQuestLogRuntime().GetRuntimeQuestState(myQuests[i]);
                    if (state.State == Types.com.QuestStatusType.InProgress)
                    {
                        ExploreEventVars newEvent = new ExploreEventVars(myArea, questUser, myQuests[i]);
                        EventManager.Instance.PlayerExploreEvent(newEvent);


                    }

                }
                else
                {
                    ExploreEventVars newEvent = new ExploreEventVars(myArea, questUser, myQuests[i]);
                    EventManager.Instance.PlayerExploreEvent(newEvent);


                }
            }
           
            

        }
    }
}