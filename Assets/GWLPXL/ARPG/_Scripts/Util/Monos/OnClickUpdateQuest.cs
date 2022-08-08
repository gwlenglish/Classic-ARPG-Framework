using GWLPXL.ARPGCore.Quests.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Demo.com
{

    public class OnClickUpdateQuest : BareClass, IMouseClickable
    {
        public Quest QuestToUpdate;
        public GameObject QuestUser;
        public QuestStatusType NewStatus;
        public void DoClick()
        {
            IQuestUser quests = QuestUser.GetComponent<IQuestUser>();
            quests.GetQuestLogRuntime().UpdateQuest(QuestToUpdate, NewStatus);
        }
    }
}
