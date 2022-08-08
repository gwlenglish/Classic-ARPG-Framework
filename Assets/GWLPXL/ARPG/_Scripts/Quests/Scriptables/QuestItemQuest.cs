
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using GWLPXL.ARPGCore.Items.com;
namespace GWLPXL.ARPGCore.Quests.com
{
   

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Logic/NEW Quest Item Quest")]

    public class QuestItemQuest : QuestLogic
    {
        [Header("Quest Item Unique")]
        [SerializeField]
        protected QuestItemGoal[] goals = new QuestItemGoal[0];
        [System.NonSerialized]
        protected StringBuilder sb = new StringBuilder();
        public override string GetProgressDescription()
        {
           
            return sb.ToString();
        }

        /// <summary>
        /// since we can update the quest from anywhere and we just have the template, we need to grab the user's quest log and find their runtime quest and update that using a private method.
        /// </summary>
        /// <param name="forUser"></param>
        public override void CheckProgress(IQuestUser forUser, Quest forQuest)
        {
            IInventoryUser inv = forUser.GetMyInstance().GetComponent<IInventoryUser>();
            if (inv == null) return;

            //reset goals
            for (int i = 0; i < goals.Length; i++)
            {
                goals[i].CurrentAmount = 0;
                goals[i].Complete = false;
            }

            //get actor inventory
            ActorInventory actorinv = inv.GetInventoryRuntime();
            List<ItemStack> stacks = new List<ItemStack>();
            for (int i = 0; i < goals.Length; i++)
            {
                int thisItemAmount = 0;
                stacks.Clear();
                //get all stacks and add the amounts
                stacks = actorinv.GetAllItemStacks(goals[i].ItemTemplate);
                for (int j = 0; j < stacks.Count; j++)
                {
                    thisItemAmount += stacks[j].CurrentStackSize;
                }

                //assign and check the amount of the item
                goals[i].CurrentAmount = thisItemAmount;
                if (goals[i].CurrentAmount >= goals[i].RequiredAmount)
                {
                    goals[i].Complete = true;
                }
                else
                {
                    goals[i].Complete = false;
                }



            }

            //assign description
            sb.Clear();
            for (int i = 0; i < goals.Length; i++)
            {
                sb.Append(goals[i].GetDescription() + "\n");
            }

            //check if goal is complete
            int complete = -1;
            for (int i = 0; i < goals.Length; i++)
            {
                if (goals[i].Complete == false)
                {
                    complete = i;
                    break;
                }
            }

            //if complete, set quest complete. If not, keep it in progress. 
            if (complete == -1)
            {
                forQuest.CompletedQuest(forUser);
               
            }
            else
            {
                forQuest.StartQuest(forUser);

            }

            forUser.GetQuestLogRuntime().OnQuestLogUpdated?.Invoke();


        }

        //quests are temporary trackers, so we should rest them after quest completion. If want to perma save, store the values elsewhere before erasing them. 
        public override void ResetTrackers(IQuestUser forUser, Quest forQuest)
        {
            IInventoryUser inv = forUser.GetMyInstance().GetComponent<IInventoryUser>();
            if (inv == null) return;

            for (int i = 0; i < goals.Length; i++)
            {

                for (int j = 0; j < goals[i].RequiredAmount; j++)
                {
                    inv.GetInventoryRuntime().RemoveFirstItemFromInventory(goals[i].ItemTemplate);
                }
               

            }
            //nothing here
        }
    }

    #region helpers

    [System.Serializable]
    public class QuestItemGoal
    {
        public QuestItem ItemTemplate = null;
        public int RequiredAmount = 1;
        public int CurrentAmount { get; set; }
        public bool Complete { get; set; }

        public QuestItemGoal(QuestItem item, int amount)
        {
            ItemTemplate = item;
            RequiredAmount = amount;
        }

        public string GetDescription()
        {
            return ItemTemplate.GetGeneratedItemName() + " " + CurrentAmount.ToString() + " / " + RequiredAmount.ToString();
        }
    }

    #endregion
}
