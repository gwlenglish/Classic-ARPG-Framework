using GWLPXL.ARPGCore.Items.com;
using UnityEngine;

namespace GWLPXL.ARPGCore.Quests.com
{

     [CreateAssetMenu(menuName = "GWLPXL/ARPG/Quests/Rewards/NEW_Currency Reward")]

    public class CurrencyReward : QuestReward
    {
        public int Currency = 100;
        public bool AutoName = true;

        public override void GetRewardPreview(IQuestGiver giver)
        {
            //
        }

        public override void GiveRewards(IQuestUser toUser)
        {
            IInventoryUser invUser = toUser.GetMyInstance().GetComponent<IInventoryUser>();
            invUser.GetInventoryRuntime().ModifyCurrency(Currency);
        }



#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (AutoName)
            {
                string path = UnityEditor.AssetDatabase.GetAssetPath(this);
                UnityEditor.AssetDatabase.RenameAsset(path, "Currency Reward " + Currency.ToString());
            }
        }

#endif


    }
}