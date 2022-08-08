using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Statics.com;
using UnityEngine;
using GWLPXL.ARPGCore.Looting.com;
using GWLPXL.ARPGCore.com;

namespace GWLPXL.ARPGCore.Quests.com
{

     [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Rewards/NEW_Item Reward")]

    public class ItemReward : QuestReward
    {
        public LootDrops Loot;
        [System.NonSerialized]
        Item instance = null;
        public int ItemLevel = 1;
        public bool AutoName = false;

        public override void GetRewardPreview(IQuestGiver giver)
        {
            if (instance == null)
            {
                instance = Loot.GetRandomDrop(ItemLevel);
            }
        }

        public override void GiveRewards(IQuestUser toUser)
        {
            if (instance == null)
            {
                instance = Loot.GetRandomDrop(ItemLevel);
            }
            IInventoryUser invUser = toUser.GetMyInstance().GetComponent<IInventoryUser>();
            if (invUser.GetInventoryRuntime().CanWeAddItem(instance))
            {
                invUser.GetInventoryRuntime().AddItemToInventory(instance);
            }
            else
            {
                //drop it on the floor
                ILoot newLoot = LootHandler.DropLoot(instance, invUser.GetMyInstance().transform.position, DungeonMaster.Instance.GetLootPrefab(), .25f);

            }
        }




#if UNITY_EDITOR
        private void OnValidate()
        {
            if (AutoName)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, "Item Reward " + Loot.name + " iLevel " + ItemLevel.ToString());
            }
        }

#endif
    }



}