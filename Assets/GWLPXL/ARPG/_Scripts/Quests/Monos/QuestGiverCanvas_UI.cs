using GWLPXL.ARPGCore.com;
using TMPro;

using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{
    /// <summary>
    /// Canvas for player interaction with the quester choices. 
    /// </summary>
    public class QuestGiverCanvas_UI : MonoBehaviour, IQuestGiverCanvas
    {

        [SerializeField]
        bool freezeDungeon = true;
        [SerializeField]
        GameObject mainPanel = null;
        [SerializeField]
        TextMeshProUGUI questText = null;
        [SerializeField]
        GameObject buttonsAcceptPanel = null;
        [SerializeField]
        GameObject buttonsInProgressPanel = null;
        [SerializeField]
        GameObject buttonsTurnInPanel = null;
        [SerializeField]
        GameObject buttonsResetPanel = null;
        IQuestGiver myGiver = null;
        private void Awake()
        {
            mainPanel.SetActive(false);
            buttonsResetPanel.SetActive(false);

        }

        public void SetUser(IQuestGiver giver)
        {
            myGiver = giver;
        }
        public void EnableCanvas(bool isEnabled)
        {
            mainPanel.SetActive(isEnabled);

            if (freezeDungeon && mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
            }
            else if (freezeDungeon && !mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);

            }

          
        }

        public void ResetQuestchain()
        {
            myGiver.ResetQuestChain();
        }
        public void Return()
        {
            myGiver.Return();
        }
        public void EnableInProgressButton(bool isEnabled)
        {
            buttonsInProgressPanel.SetActive(isEnabled);
        }
        public void EnableTurnInButton(bool isEnabled)
        {
            buttonsTurnInPanel.SetActive(isEnabled);
        }
        public void EnableAcceptQuestButtons(bool isEnabled)
        {
            buttonsAcceptPanel.SetActive(isEnabled);
        }
        public void SetQuestText(string newText)
        {
            questText.SetText(newText);
        }
        public void EnableResetQuestchainPanel(bool isEnabled)
        {
            buttonsResetPanel.SetActive(isEnabled);

        }
        public void AcceptQuest()
        {
            myGiver.StartQuest();

        }

        public void DeclineQuest()
        {
            myGiver.DeclineQuest();
        }

        public void CollectRewards()
        {
            myGiver.CollectRewards();
        }

        public void IncrementQuest()
        {
            myGiver.IncrementQuests();
        }
        public void DecrementQuest()
        {
            myGiver.DecrementQuests();
        }

       
    }
}