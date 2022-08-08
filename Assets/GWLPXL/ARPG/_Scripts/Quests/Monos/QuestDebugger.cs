using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Types.com;
using System.Text;
namespace GWLPXL.ARPGCore.DebugHelpers.com
{

    /// <summary>
    /// used for debugging quests, must be attached to game object with the quest user
    /// </summary>
    public class QuestDebugger : MonoBehaviour
    {
        public Quest Quest;
        IQuestUser questUser = null;
        int frameCheck = 60;
        int counter = 0;
        StringBuilder sb = new StringBuilder();
        private void Awake()
        {
            questUser = GetComponent<IQuestUser>();
        }
   
        // Update is called once per frame
        void Update()
        {
            if (questUser == null || Quest == null) return;
            counter += 1;
            if (counter % frameCheck == 0)
            {
                sb.Clear();
                sb.Append(questUser.GetQuestLogRuntime().GetRuntimeQuestState(Quest).State.ToString());
                string status = sb.ToString();

                Debug.Log(status);
                counter = 0;
            }
        
        }
    }
}