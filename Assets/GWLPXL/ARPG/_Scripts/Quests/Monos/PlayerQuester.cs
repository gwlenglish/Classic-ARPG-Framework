


using UnityEngine;
using GWLPXL.ARPGCore.com;
namespace GWLPXL.ARPGCore.Quests.com
{

    public class PlayerQuester : MonoBehaviour, IQuestUser, ISubscribeEvents
    {
        [SerializeField]
        PlayerQuestEvents questEvents = new PlayerQuestEvents();
        [SerializeField]
        QuestLog QuestLogTemplate = null;

        QuestLog questLogRuntime = null;

        //[SerializeField]
        //bool autoEnableCanvasOnQuestAccept = true;
        //IUseQuesterCanvas questerui = null;
        void Awake()
        {
            QuestLog temp = Instantiate(QuestLogTemplate);
            SetRuntimeQuestLog(temp);
          //  questerui = GetComponent<IUseQuesterCanvas>();
        }

        public void SetRuntimeQuestLog(QuestLog neLog)
        {
            if (questLogRuntime != null)
            {
                UnSubscribeEvents();
            }

            questLogRuntime = neLog;
            questLogRuntime.Ini(this);

            if (questLogRuntime != null)
            {
                SubscribeEvents();
            }

        }

      
      
        public QuestLog GetQuestLogTemplate()
        {
            return QuestLogTemplate;
        }
        /// <summary>
        /// main call to read and record data to the quest log
        /// </summary>
        /// <returns></returns>
        public QuestLog GetQuestLogRuntime()
        {
            return questLogRuntime;
        }
        public Transform GetMyInstance()
        {
            return this.transform;
        }

        public void SetQuestGiver(IQuestGiver questGiver)
        {
            GetQuestLogRuntime().SetQuestGiver(questGiver);
        }

        public void SubscribeEvents()
        {
            GetQuestLogRuntime().OnQuestAvailable += OnQuestAvailable;
            GetQuestLogRuntime().OnQuestChainInProgress += OnQuestChainStarted;

            GetQuestLogRuntime().OnQuestInProgress += OnQuestInProgress;
            GetQuestLogRuntime().OnQuestComplete += OnQuestCompleted;
            GetQuestLogRuntime().OnRewardsCollected += OnQuestRewardsCollected;
            GetQuestLogRuntime().OnQuestAbandon += OnQuestAbandoned;

        }

        public void UnSubscribeEvents()
        {
            GetQuestLogRuntime().OnQuestAvailable -= OnQuestAvailable;
            GetQuestLogRuntime().OnQuestChainInProgress -= OnQuestChainStarted;

            GetQuestLogRuntime().OnQuestInProgress -= OnQuestInProgress;
            GetQuestLogRuntime().OnQuestComplete -= OnQuestCompleted;
            GetQuestLogRuntime().OnRewardsCollected -= OnQuestRewardsCollected;
            GetQuestLogRuntime().OnQuestAbandon -= OnQuestAbandoned;
        }

        void OnQuestChainStarted(Questchain started)
        {
            Debug.Log("Invoked");
            //if (autoEnableCanvasOnQuestAccept && questerui != null)
            //{
            //    bool enabled = questerui.GetQuesterUI().GetCanvasEnabled();
            //    if (enabled == false)
            //    {
            //        questerui.GetQuesterUI().ToggleQuesterUI();
            //    }
            //}
        }
        void OnQuestAvailable(Quest quest)
        {
            questEvents.SceneEvents.OnQuestAvailable.Invoke(quest);
        }
        void OnQuestInProgress(Quest quest)
        {
            questEvents.SceneEvents.OnQuestInProgress.Invoke(quest);
           

        }
        void OnQuestCompleted(Quest quest)
        {
            questEvents.SceneEvents.OnQuestCompleted.Invoke(quest);
        }
        void OnQuestRewardsCollected(Quest quest)
        {
            questEvents.SceneEvents.OnQuestRewardsCollected.Invoke(quest);
        }
        void OnQuestAbandoned(Quest quest)
        {
            questEvents.SceneEvents.OnQuestAbandoned.Invoke(quest);
        }

        public void SetTemplate(QuestLog newTemplate)
        {
            QuestLogTemplate = newTemplate;
        }
    }
}
