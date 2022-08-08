using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Types.com;
namespace GWLPXL.ARPGCore.DebugHelpers.com
{

    /// <summary>
    /// For debugging quest chains.
    /// </summary>
    public class QuestChainDebugger : MonoBehaviour
    {
        public Questchain Questchain;
        public QuestStatusType TypeToTest;
        public bool ChangeStatus = false;
        public KeyCode[] ForceCompleteCurrentQuest = new KeyCode [1]{ KeyCode.LeftShift};
        public KeyCode[] ForceChainAvailableAndStart = new KeyCode[1] { KeyCode.F1 };
        public KeyCode[] ForceChainReset = new KeyCode[1] { KeyCode.F2 };
        IQuestUser questUser = null;

        int counter = 0;
        int frameCheck = 60;
        private void Awake()
        {
            questUser = GetComponent<IQuestUser>();
        }

        int GetPressed(KeyCode[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (Input.GetKeyDown(arr[i]) == false)
                {
                    return i;
                }
            }
            return -1;

        }
        // Update is called once per frame
        void Update()
        {
            if (questUser == null || Questchain == null) return;
            counter += 1;

            int force = GetPressed(ForceCompleteCurrentQuest);

            int forceavailable = GetPressed(ForceChainAvailableAndStart);

            int forceReset = GetPressed(ForceChainReset);

            if (forceReset == -1)
            {
                questUser.GetQuestLogRuntime().ForceResetQuestChain(Questchain);
                return;
            }
            if (forceavailable == -1)
            {
                questUser.GetQuestLogRuntime().UpdateChain(Questchain, QuestStatusType.Available);//make it available.
                questUser.GetQuestLogRuntime().UpdateChain(Questchain, QuestStatusType.InProgress);//switch it to in progress
                int questIndex = questUser.GetQuestLogRuntime().GetQuestIndexInChain(Questchain);//grab our current chain in the quest. 
                Quest template = Questchain.GetQuests()[questIndex];//get the quest template
                questUser.GetQuestLogRuntime().UpdateQuest(template, QuestStatusType.InProgress);//update our runtime in progress. 
                return;
            }

            if (force == -1)
            {
                int questIndex = questUser.GetQuestLogRuntime().GetQuestIndexInChain(Questchain);//get current quest
                Quest template = Questchain.GetQuests()[questIndex];//find the template
                template.CompletedQuest(questUser);//tell it to complete
                return;
            }

            if (ChangeStatus)
            {
                questUser.GetQuestLogRuntime().UpdateChain(Questchain, TypeToTest);
                ChangeStatus = false;
                return;
            }


            if (counter % frameCheck == 0)
            {
                counter = 0;
                string status = questUser.GetQuestLogRuntime().GetQuestchainState(Questchain).State.ToString();
#if UNITY_EDITOR
                Debug.Log(status);
#endif
            }
           
        }
    }
}