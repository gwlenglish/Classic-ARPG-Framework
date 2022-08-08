using GWLPXL.ARPGCore.Types.com;
using GWLPXL.ARPGCore.Quests.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Items.com
{

    [CreateAssetMenu(menuName = "GWLPXL/ARPG/QuestItems/NEW_")]

    public class QuestItem : Item
    {
        [SerializeField]
        Quest[] forQuests = new Quest[0];
        [SerializeField]
        string itemName = "Key Item";
        [SerializeField]
        [TextArea(3, 5)]
        string UserDescription = "This is a key item, never duplicated";

        ItemType type = ItemType.QuestItem;
        int stacking = 1;
        bool isStacking = true;

        public override string GetBaseItemName()
        {
            return itemName;
        }
        public void UpdateQuest(IQuestUser forUser)
        {
            if (forUser == null) return;
            for (int i = 0; i < forQuests.Length; i++)
            {
                forQuests[i].UpdateQuestProgress(forUser);

            }
        }
        public override string GetGeneratedItemName()
        {
            return itemName;
        }

        public override ItemType GetItemType()
        {
            return type;
        }

        public override int GetStackingAmount()
        {
            return stacking;
        }

        public override string GetUserDescription()
        {
            return UserDescription;
        }

        public override bool IsStacking()
        {
            return isStacking;
        }

        public override void SetGeneratedItemName(string newName)
        {
            itemName = newName;
        }
    }
}