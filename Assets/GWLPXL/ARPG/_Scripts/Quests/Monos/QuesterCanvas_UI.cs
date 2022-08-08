using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using GWLPXL.ARPGCore.Types.com;
namespace GWLPXL.ARPGCore.Quests.com
{
    /// <summary>
    /// UI canvas that displays quest progress
    /// </summary>
    public class QuesterCanvas_UI : MonoBehaviour, IQuesterCanvas
    {
        [SerializeField]
        GameObject mainPanel = null;
        [SerializeField]
        TextMeshProUGUI inProgressText = null;
        [SerializeField]
        TextMeshProUGUI completedText = null;
        [SerializeField]
        TextMeshProUGUI collectedText = null;
        IUseQuesterCanvas myUser = null;
        StringBuilder sbInProgress = new StringBuilder();
        StringBuilder sbCompleted = new StringBuilder();
        StringBuilder sbCollected = new StringBuilder();

        QuestLog log = null;
        private void Awake()
        {
            mainPanel.SetActive(false);
        }


        //protected virtual void Update()
        //{
        //    if (myUser == null) return;
        //    if (myUser.ToggleCanvas())
        //    {
        //        ToggleQuesterUI();
        //    }

        //}

        public bool GetCanvasEnabled()
        {
            return mainPanel.activeInHierarchy;
        }

       
        public void RefreshQuesterUI()
        {
            sbInProgress.Clear();
            sbCompleted.Clear();
            sbCollected.Clear();
            inProgressText.SetText(sbInProgress.ToString());
            completedText.SetText(sbCompleted.ToString());
            collectedText.SetText(sbCollected.ToString());

            if (myUser == null) return;
            //update text

            Questchain[] activechains = myUser.GetQuester().GetQuestLogRuntime().GetQuestChainsInStatus(QuestStatusType.InProgress);
            if (activechains.Length == 0) return;

            Quest[] allQuests = activechains[0].GetQuests();//only getting 0, maybe later add a toggle to check the other states. 

            for (int i = 0; i < allQuests.Length; i++)
            {
                QuestState state = myUser.GetQuester().GetQuestLogRuntime().GetRuntimeQuestState(allQuests[i]);
                if (state.State == QuestStatusType.InProgress)
                {
                    sbInProgress.Append(allQuests[i].GetQuestJournalEntry(QuestStatusType.InProgress) + "\n");
                    sbInProgress.Append(allQuests[i].GetProgressDescription() + "\n");
                }

                else if (state.State == QuestStatusType.Completed)
                {
                    sbCompleted.Append(allQuests[i].GetQuestJournalEntry(QuestStatusType.Completed) + "\n");
                }

                else if (state.State == QuestStatusType.RewardsCollected)
                {
                    sbCollected.Append(allQuests[i].GetQuestJournalEntry(QuestStatusType.RewardsCollected) + "\n");
                }
            }

            inProgressText.SetText(sbInProgress.ToString());
            completedText.SetText(sbCompleted.ToString());
            collectedText.SetText(sbCollected.ToString());

        }


        public void SetUser(IUseQuesterCanvas newUser)
        {
            myUser = newUser;
            log = myUser.GetQuester().GetQuestLogRuntime();
            log.OnQuestLogUpdated += RefreshQuesterUI;

        }

        private void OnEnable()
        {
            if (log == null) return;
            log.OnQuestLogUpdated += RefreshQuesterUI;
        }
        private void OnDisable()
        {
            if (log == null) return;
            log.OnQuestLogUpdated -= RefreshQuesterUI;
        }

        public void ToggleQuesterUI()
        {
            mainPanel.SetActive(!mainPanel.activeInHierarchy);
          if (mainPanel.activeInHierarchy)
            {
                RefreshQuesterUI();
            }
        }

       
    }
}